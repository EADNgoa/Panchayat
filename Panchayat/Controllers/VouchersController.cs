using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Panchayat;
using PagedList;

namespace Panchayat.Controllers
{
    [Authorize(Roles = "Boss,Type1")]
    public class VouchersController : EAController
    {
        

        // GET: Vouchers
        public ActionResult Index(int? page, int? SearchRecieptNo, string PassedBy, DateTime? tdate, DateTime? fdate, decimal? fAmount, decimal? tAmount)
        {
            var Vouc = db.Vouchers.Where(f => f.VoucherID> 0);

            if (SearchRecieptNo != null)
            {
                Vouc = Vouc.Where(f => f.VoucherID == SearchRecieptNo);
            }
            if (PassedBy?.Length > 0)
            {
                Vouc = Vouc.Where(f => f.PassedBy.Contains(PassedBy));
            }
            if (fdate != null && tdate != null)
            {
                Vouc = Vouc.Where(t => t.PayDate >= fdate && t.PayDate <= tdate);
            }
            if (fAmount != null && tAmount != null)
            {
                Vouc = Vouc.Where(t => t.Amount >= fAmount && t.Amount <= tAmount);
            }


            Vouc = Vouc.OrderByDescending(e => e.PayDate);
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(Vouc.ToPagedList(pageNumber, pageSize));
        }

        // GET: Vouchers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }

            ViewBag.vpname = db.Configs.Find(1).VP;

            var cnts = new ChangeNumbersToWords();
            ViewBag.AmtInWds = cnts.changeToWords(voucher.ActualAmount.ToString());

            return View(voucher);
        }

        // GET: Vouchers/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: Vouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VoucherID,PassedBy,of,Amount,ActualAmount,For,CBfolio,ResNo,HeldOn,Meeting, SubLedgerID")] Voucher voucher, FormCollection fm)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        voucher.LedgerID = db.SubLedgers.Find(voucher.SubLedgerID).LedgerID;
                voucher.PayDate = DateTime.Today;
                db.Vouchers.Add(voucher);

                //For receipts for subledgers that do not have demand records we need to store the LedgerDetails at this point
                foreach (var item in fm)
                {
                    if (item.ToString().Length > 5 && item.ToString().Substring(0, 5) == "data-")
                    {
                        int iLedDetID = int.Parse(item.ToString().Substring(item.ToString().LastIndexOf('-') + 1));

                        //Prevent overposting attack by checking that only valid LedgerDetails for the given subledger are saved
                        if (db.LedgerDetails.Any(l => l.LedgerDetailID == iLedDetID && l.SubLedgerID == voucher.SubLedgerID))
                            db.RVdetails.Add(new RVdetail { LedgerDetailID = iLedDetID, RVDetail1 = fm[item.ToString()], VoucherID = voucher.VoucherID });
                    }
                }

                        //Update our Budget
                        int budYr = MyExtensions.GetFinYr((DateTime)voucher.PayDate);
                        var bud = db.Budgets.FirstOrDefault(b => b.SubLedgerID == voucher.SubLedgerID && b.BudgtFY == budYr);
                        bud.ActualAmount += voucher.ActualAmount?? 0;
                        db.Entry(bud).State = EntityState.Modified;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(voucher);
        }


        // GET: Vouchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            
            return View(voucher);
        }

        /// <summary>
        /// Using a Partial view so that the CLD can be in the same entry form as the Demands
        /// </summary>
        /// <param name="id">Chosen Subledger ID from Autocomplete box</param>        
        /// <returns></returns>        
        public ActionResult EditRVLedgDetsPartial(int id)
        {
            var CLedgDets = db.RVdetails.Where(rv => rv.VoucherID== id).ToList();
            return PartialView("_EditRVLedgDetsPartial", CLedgDets);

        }

        // POST: Vouchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VoucherID,PassedBy,of,Amount,ActualAmount,For,PayDate,CBfolio,ResNo,HeldOn,Meeting,SubLedgerID,LedgerID")] Voucher voucher, FormCollection fm)
        {
            if (ModelState.IsValid && voucher.PayDate==DateTime.Today)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(voucher).State = EntityState.Modified;

                        foreach (var item in fm)
                        {
                            if (item.ToString().Length > 5 && item.ToString().Substring(0, 5) == "data-")
                            {
                                int iLedDetID = int.Parse(item.ToString().Substring(item.ToString().LastIndexOf('-') + 1));

                                var cld = db.RVdetails.Find(iLedDetID);
                                cld.RVDetail1 = fm[item.ToString()];

                                db.Entry(cld).State = EntityState.Modified;
                            }
                        }

                        //Update the Budget
                        int budYr = MyExtensions.GetFinYr((DateTime)voucher.PayDate);
                        var bud = db.Budgets.FirstOrDefault(b => b.SubLedgerID == voucher.SubLedgerID && b.BudgtFY == budYr);

                        var CurrAmt = db.Entry(voucher).CurrentValues.Clone();
                        db.Entry(voucher).Reload();
                        bud.ActualAmount -= voucher.ActualAmount?? 0; //minus the old value
                        bud.ActualAmount += (decimal)CurrAmt["ActualAmount"]; //add the new value
                        db.Entry(voucher).CurrentValues.SetValues(CurrAmt);
                        db.Entry(voucher).State = EntityState.Modified;
                        db.Entry(bud).State = EntityState.Modified;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(voucher);
        }

        // GET: Vouchers/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Voucher voucher = db.Vouchers.Find(id);
        //    if (voucher == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(voucher);
        //}

        //// POST: Vouchers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Voucher voucher = db.Vouchers.Find(id);
        //    db.Vouchers.Remove(voucher);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
