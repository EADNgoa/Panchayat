using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using Panchayat;
/// <summary>
/// This Controller handels teh receipts side for Form 6,9, SD and EMD recievables. 
/// There is a counterpart controller for issues
/// </summary>
namespace Panchayat.Controllers
{
    public class SD9EMD6Controller : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: SD9EMD6
        public ActionResult Index(int? page, int rt, string PropName, int? yr)
        {
            ViewBag.RegisterTypeID = rt;
            ViewBag.YearBox = MyExtensions.MakeYrRq(5, 1, DateTime.Now.Year);
            yr = MyExtensions.GetFinYr(DateTime.Parse("1 April" + yr??DateTime.Now.Year.ToString()));
            var Recpts = db.InOutRegsRecpts.Where(r =>r.RegisterTypeID==rt && ((DateTime)r.TDate).Year == yr).OrderByDescending(r => r.TDate).Include(i => i.RegisterType).ToList();
            var Issues = db.InOutRegsIssues.Where(i => i.RegisterTypeID == rt && ((DateTime)i.TDate).Year == yr).OrderByDescending(r => r.TDate).Include(i => i.RegisterType).ToList();

            if (PropName?.Length>0)
            {//for Form 6
                Recpts = Recpts.Where(r => r.PropertyParticulars.Contains(PropName)).ToList();
                page = 1;
            }

            List<rpt1to1io> RptData = new List<rpt1to1io>();
            Recpts.ForEach(r =>
            {
                rpt1to1io item = new rpt1to1io
                {
                    IORecptID = r.IORecptID,
                    TDate = (DateTime)r.TDate,
                    Value = r.Value ?? 0,
                    RVno = r.RVno ?? 0,
                    PropertyParticulars = r.PropertyParticulars,
                    SituatedWhere = r.SituatedWhere,
                    DepositPurpose = r.DepositPurpose,
                    ValuationDetails = r.ValuationDetails,
                    SanctionDateNo = r.SanctionDateNo,
                    SanctionByWhom = r.SanctionByWhom,
                    PeriodToSpendYrs = r.PeriodToSpendYrs ?? 0,
                    TreasureVoucherDetails = r.TreasureVoucherDetails
                };

                //Fetch the issue details
                item.IssuesDets = new List<rpt1To1ioIssue>();
                var SubIssues= Issues.Where(i => i.IORecptID == r.IORecptID).ToList();
                SubIssues.ForEach(iDet =>
                {
                    rpt1To1ioIssue itemDetail = new rpt1To1ioIssue
                    {
                        IOissueID = iDet.IOissueID,
                        IORecptID = (int)iDet.IORecptID,
                        TDate = (DateTime)iDet.TDate,
                        Value = iDet.Value ?? 0,
                        IssuedTo = iDet.IssuedTo,
                        Balance = iDet.Balance ?? 0,
                        Remarks = iDet.Remarks,
                        RVno = (int)iDet.RVno,
                        RefundReason = iDet.RefundReason
                    };
                    item.IssuesDets.Add(itemDetail);
                });

                RptData.Add(item);
            });

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(RptData.OrderByDescending(a => a.TDate).ToList().ToPagedList(pageNumber, pageSize));

           
        }

     
        // GET: SD9EMD6/Create
        public ActionResult CreateAquisition(int rt)
        {
            ViewBag.RegisterTypeID = rt;

            if (rt==22) return View("CreateAquisition6");

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: SD9EMD6/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAquisition([Bind(Include = "IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,RVno,PropertyParticulars,SituatedWhere,DepositPurpose,ValuationDetails,SanctionDateNo,SanctionByWhom,PeriodToSpendYrs,TreasureVoucherDetails")] InOutRegsRecpt inOutRegsRecpt)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        string UserID = User.Identity.GetUserName();
                        var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
                                                
                        //Create a voucher for Form6
                        if (inOutRegsRecpt.RegisterTypeID == 22)
                        {
                            var item = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsRecpt.Value, ActualAmount = inOutRegsRecpt.Value, For = inOutRegsRecpt.PropertyParticulars, PayDate = DateTime.Today, CBfolio = null, ResNo = null, HeldOn = inOutRegsRecpt.TDate, Meeting = "N/A", LedgerID = 10, SubLedgerID = 69 };
                            db.Vouchers.Add(item);
                            db.SaveChanges();
                            inOutRegsRecpt.RVno = item.VoucherID;
                            
                        }
                        db.InOutRegsRecpts.Add(inOutRegsRecpt);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                return RedirectToAction("Index", new { rt=inOutRegsRecpt.RegisterTypeID});
            }
            
            return View(inOutRegsRecpt);
        }

        public ActionResult CreateDisposal(int rt, int IORecptID)
        {
            
            InOutRegsIssue model = new InOutRegsIssue { IORecptID = IORecptID, RegisterTypeID=rt  };

            if (rt == 22) return View("CreateDisposal6",model);

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: SD9EMD6/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDisposal([Bind(Include = "IOissueID,IORecptID,RegisterTypeID,TDate,Value,IssuedTo,Remarks,RefundReason")] InOutRegsIssue inOutRegsIssue)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.InOutRegsIssues.Add(inOutRegsIssue);

                        //Create a receipt for Form6
                        if (inOutRegsIssue.RegisterTypeID == 22 && inOutRegsIssue.Value>0)
                        {
                            var item = new Form4 { Amount = inOutRegsIssue.Value, LedgerID = 10, PayDate = DateTime.Today, RecvdFrom = "Disposal of Property", SubLedgerID = 69 };
                            db.Form4.Add(item);
                            db.SaveChanges();
                            inOutRegsIssue.RVno = item.RecieptNo;
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            return RedirectToAction("Index", new { rt = inOutRegsIssue.RegisterTypeID });
            }
                        
            return View(inOutRegsIssue);
        }

        // GET: SD9EMD6/Edit/5
        public ActionResult EditAquisition(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsRecpt inOutRegsRecpt = db.InOutRegsRecpts.Find(id);
            if (inOutRegsRecpt == null)
            {
                return HttpNotFound();
            }
            if (inOutRegsRecpt.RegisterTypeID == 22)
            {
                ViewBag.isRVtoday = db.Vouchers.Find(inOutRegsRecpt.RVno).PayDate == DateTime.Today;
                return View("EditAquisition6", inOutRegsRecpt);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: SD9EMD6/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAquisition([Bind(Include = "IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,RVno,PropertyParticulars,SituatedWhere,DepositPurpose,ValuationDetails,SanctionDateNo,SanctionByWhom,PeriodToSpendYrs,TreasureVoucherDetails")] InOutRegsRecpt inOutRegsRecpt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inOutRegsRecpt).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { rt = inOutRegsRecpt.RegisterTypeID });
            }
            
            return View(inOutRegsRecpt);
        }

        // GET: SD9EMD6/Edit/5
        public ActionResult EditDisposal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsIssue inOutRegsIssues = db.InOutRegsIssues.Find(id);
            if (inOutRegsIssues == null)
            {
                return HttpNotFound();
            }

            if (inOutRegsIssues.RegisterTypeID == 22)
            {
                ViewBag.isRVtoday = db.Form4.Find(inOutRegsIssues.RVno).PayDate == DateTime.Today;
                return View("EditDisposal6", inOutRegsIssues);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: SD9EMD6/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDisposal([Bind(Include = "IOissueID,IORecptID,RegisterTypeID,TDate,Value,IssuedTo,RVno,Remarks,RefundReason")] InOutRegsIssue inOutRegsIssue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inOutRegsIssue).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { rt = inOutRegsIssue.RegisterTypeID });
            }
            
            return View(inOutRegsIssue);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
