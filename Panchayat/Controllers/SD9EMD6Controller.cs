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
    public class SD9EMD6Controller : EAController
    {
        

        // GET: SD9EMD6
        public ActionResult Index(int? page, int rt, string PropName, int? yr)
        {
            ViewBag.RegisterTypeID = rt;
            ViewBag.YearBox = MyExtensions.MakeYrRq(5, 1, DateTime.Now.Year);
            yr = MyExtensions.GetFinYr(DateTime.Parse("1 April" + yr??DateTime.Now.Year.ToString()));
            var Recpts = db.InOutRegsRecpts.Where(r =>r.RegisterTypeID==rt && ((DateTime)r.TDate).Year == yr).OrderByDescending(r => r.TDate).Include(i => i.RegisterType).ToList();
            var Issues = db.InOutRegsIssues.Where(i => i.RegisterTypeID == rt && ((DateTime)i.TDate).Year == yr).OrderByDescending(r => r.TDate).Include(i => i.RegisterType).ToList();

            if (PropName?.Length>0 && rt==22 || rt==1040)
            {//for Form 6
                Recpts = Recpts.Where(r => r.PropertyParticulars.Contains(PropName)).ToList();
                page = 1;
            }

            if (PropName?.Length > 0 && (rt == 21 || rt==23 ||rt==24))
            {//for Form 9 and SD and EMD
                Recpts = Recpts.Where(r => r.DepositPurpose.Contains(PropName)).ToList();
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
            if (rt == 21) return View("Index9", RptData.OrderByDescending(a => a.TDate).ToList().ToPagedList(pageNumber, pageSize));
            if (rt == 22 || rt == 1040) return View("Index6",RptData.OrderByDescending(a => a.TDate).ToList().ToPagedList(pageNumber, pageSize));
            if (rt == 23) return View("IndexSD", RptData.OrderByDescending(a => a.TDate).ToList().ToPagedList(pageNumber, pageSize));
            if (rt == 24) return View("IndexEMD", RptData.OrderByDescending(a => a.TDate).ToList().ToPagedList(pageNumber, pageSize));
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

     
        // GET: SD9EMD6/Create
        public ActionResult CreateAquisition(int rt, int? wrk)
        {
            ViewBag.RegisterTypeID = rt;

            if (rt == 21) return View("CreateAquisition9");
            if (rt==22) return View("CreateAquisition6");
            if (rt == 23) return View("CreateAquisitionSD", initiateForWrk(wrk));            
            if (rt == 24) return View("CreateAquisitionEMD", initiateForWrk(wrk));

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
            private InOutRegsRecpt initiateForWrk(int? w)
            {
                //Save the user from having to enter the Contractor and Project name again
                if (w == null) return null;
                ViewBag.wrk = w;
                var workData = db.Works.Find(w.Value);
                return new InOutRegsRecpt { PropertyParticulars = workData.ContractorName, DepositPurpose = workData.NameOfWork };
            }

        // POST: SD9EMD6/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAquisition([Bind(Include = "IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,RVno,PropertyParticulars,SituatedWhere,DepositPurpose,ValuationDetails,SanctionDateNo,SanctionByWhom,PeriodToSpendYrs,TreasureVoucherDetails")] InOutRegsRecpt inOutRegsRecpt, int? wrk)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                                                
                        //Create a voucher for Form6
                        if (inOutRegsRecpt.RegisterTypeID == 22)
                        {
                            string UserID = User.Identity.GetUserName();
                            var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
                            var item = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsRecpt.Value, ActualAmount = inOutRegsRecpt.Value, For = inOutRegsRecpt.PropertyParticulars, PayDate = DateTime.Today, CBfolio = null, ResNo = null, HeldOn = inOutRegsRecpt.TDate, Meeting = "N/A", LedgerID = 10, SubLedgerID = 69 };
                            db.Vouchers.Add(item);
                            db.SaveChanges();
                            inOutRegsRecpt.RVno = item.VoucherID;
                            
                        }

                        if (inOutRegsRecpt.RegisterTypeID == 21 || inOutRegsRecpt.RegisterTypeID == 23 || inOutRegsRecpt.RegisterTypeID == 24)
                        {
                            var item = new Form4 { Amount = inOutRegsRecpt.Value, LedgerID = 3, PayDate = DateTime.Today, RecvdFrom = "Conditional Grants", SubLedgerID = 5 };
                            if (inOutRegsRecpt.RegisterTypeID == 23 || inOutRegsRecpt.RegisterTypeID == 24) { item.RecvdFrom = inOutRegsRecpt.PropertyParticulars; inOutRegsRecpt.TDate = DateTime.Today; }
                            if (inOutRegsRecpt.RegisterTypeID == 23) { item.SubLedgerID = 26; item.LedgerID = 7; }
                            if (inOutRegsRecpt.RegisterTypeID == 24) { item.SubLedgerID = 25; item.LedgerID = 7; }
                            db.Form4.Add(item);
                            db.SaveChanges();
                            inOutRegsRecpt.RVno = item.RecieptNo;
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
                if (wrk!=null) return RedirectToAction("Edit","Works", new { id = (int)wrk, IOrID = inOutRegsRecpt.IORecptID, rt=inOutRegsRecpt.RegisterTypeID });

                return RedirectToAction("Index", new { rt=inOutRegsRecpt.RegisterTypeID});
            }
            
            return View(inOutRegsRecpt);
        }

        public ActionResult CreateDisposal(int rt, int IORecptID, int? wrk)
        {            
            InOutRegsIssue model = new InOutRegsIssue { IORecptID = IORecptID, RegisterTypeID=rt  };
            ViewBag.wrk = wrk;
            if (rt == 21) return View("CreateDisposal9", model);
            if (rt == 22) return View("CreateDisposal6",model);
            if (rt == 23) return View("CreateDisposalSD", model);
            if (rt == 24) return View("CreateDisposalEMD", model);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: SD9EMD6/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDisposal([Bind(Include = "IOissueID,IORecptID,RegisterTypeID,TDate,Value,IssuedTo,Remarks,RefundReason")] InOutRegsIssue inOutRegsIssue, int? wrk)
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

                        //Create a voucher for Form9 or SD
                        if ((inOutRegsIssue.RegisterTypeID == 21 || inOutRegsIssue.RegisterTypeID == 23 || inOutRegsIssue.RegisterTypeID == 24) && inOutRegsIssue.Value > 0)
                        {
                            var rec = db.InOutRegsRecpts.Find(inOutRegsIssue.IORecptID).Value; //value of grant
                            var spends = db.InOutRegsIssues.Where(i => i.IORecptID== inOutRegsIssue.IORecptID).Sum(s => s.Value);
                            inOutRegsIssue.Balance = (rec??0) - (spends??0) - (inOutRegsIssue.Value??0);

                            string UserID = User.Identity.GetUserName();
                            var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
                            var item = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsIssue.Value, ActualAmount = inOutRegsIssue.Value, For = inOutRegsIssue.Remarks, PayDate = DateTime.Today, CBfolio = null, ResNo = null, HeldOn = inOutRegsIssue.TDate, Meeting = "N/A",LedgerID = 3, SubLedgerID = 5 };
                            if (inOutRegsIssue.RegisterTypeID == 23) { item.SubLedgerID = 121; item.LedgerID = 19; }
                            if (inOutRegsIssue.RegisterTypeID == 24) { item.SubLedgerID = 120; item.LedgerID = 19; }

                            db.Vouchers.Add(item);
                            db.SaveChanges();
                            inOutRegsIssue.RVno = item.VoucherID;
                        }

                        db.SaveChanges(); //to save the receipt or voucher no
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                if (wrk != null) return RedirectToAction("Edit", "Works", new { id = (int)wrk, IOrID = inOutRegsIssue.IORecptID, rt = inOutRegsIssue.RegisterTypeID });
                return RedirectToAction("Index", new { rt = inOutRegsIssue.RegisterTypeID });
            }
                        
            return View(inOutRegsIssue);
        }

        // GET: SD9EMD6/Edit/5
        public ActionResult EditAquisition(int? id, int? wrk)
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

            if (inOutRegsRecpt.RegisterTypeID == 21 || inOutRegsRecpt.RegisterTypeID == 23|| inOutRegsRecpt.RegisterTypeID == 24)            
                ViewBag.isRVtoday = db.Form4.Find(inOutRegsRecpt.RVno).PayDate == DateTime.Today;

            ViewBag.wrk = wrk;
            if (inOutRegsRecpt.RegisterTypeID == 21) return View("EditAquisition9", inOutRegsRecpt);
            if (inOutRegsRecpt.RegisterTypeID == 23) return View("EditAquisitionSD", inOutRegsRecpt);
            if (inOutRegsRecpt.RegisterTypeID == 24) return View("EditAquisitionEMD", inOutRegsRecpt);

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: SD9EMD6/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAquisition([Bind(Include = "IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,RVno,PropertyParticulars,SituatedWhere,DepositPurpose,ValuationDetails,SanctionDateNo,SanctionByWhom,PeriodToSpendYrs,TreasureVoucherDetails")] InOutRegsRecpt inOutRegsRecpt, int? wrk)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inOutRegsRecpt).State = EntityState.Modified;
                db.SaveChanges();
                if (wrk != null) return RedirectToAction("Edit", "Works", new { id = (int)wrk });
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

            if (inOutRegsIssues.RegisterTypeID == 21 || inOutRegsIssues.RegisterTypeID == 23 || inOutRegsIssues.RegisterTypeID == 24)            
                ViewBag.isRVtoday = db.Vouchers.Find(inOutRegsIssues.RVno).PayDate == DateTime.Today;

            if (inOutRegsIssues.RegisterTypeID == 21) return View("EditDisposal9", inOutRegsIssues);
            if (inOutRegsIssues.RegisterTypeID == 23) return View("EditDisposalSD", inOutRegsIssues);
            if (inOutRegsIssues.RegisterTypeID == 24) return View("EditDisposalEMD", inOutRegsIssues);

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
                    //find original value
                //var origspends = db.InOutRegsIssues.Find(inOutRegsIssue.IOissueID).Value;

                //Save Current value
                db.Entry(inOutRegsIssue).State = EntityState.Modified;
                db.SaveChanges();

                if (inOutRegsIssue.RegisterTypeID == 21)
                {

                    var rec = db.InOutRegsRecpts.Find(inOutRegsIssue.IORecptID).Value; //value of grant
                    var spends = db.InOutRegsIssues.Where(i => i.IORecptID == inOutRegsIssue.IORecptID).Sum(s => s.Value);

                   // spends = spends - origspends + inOutRegsIssue.Value;

                    inOutRegsIssue.Balance = (rec ?? 0) - (spends ?? 0);
                }
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
