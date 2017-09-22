using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Panchayat;

namespace Panchayat.Controllers
{
    [Authorize(Roles = "Boss,Type1")]
    public class CorrectionsController : EAController
    {
        

        // GET: Corrections
        public ActionResult Index(int? ReceiptID, int? VoucherID)
        {
            var cor = Enumerable.Empty<Correction>();
            if (ReceiptID.HasValue)
            {
                cor = db.Corrections.Where(c => c.RecieptID == (int)ReceiptID);
                var FaultyRec = db.Form4.Find(ReceiptID);
                ViewBag.Heading = String.Format("Receipt of {0} created on {1:dd-MMM-yyyy} against subledger: {2}", FaultyRec.Citizen.Name + FaultyRec.RecvdFrom, FaultyRec.PayDate, FaultyRec.Ledger.Ledger1 + " >> " + FaultyRec.SubLedger.Ledger);
            }

            if (VoucherID.HasValue)
            {
                cor = db.Corrections.Where(c => c.VoucherID == (int)VoucherID);
                var FaultyRec = db.Vouchers.Find(VoucherID);
                ViewBag.Heading = String.Format("Voucher of {0} created on {1:dd-MMM-yyyy} against subledger: {2}", FaultyRec.PassedBy, FaultyRec.PayDate, FaultyRec.Ledger.Ledger1 + " >> " + FaultyRec.SubLedger.Ledger);
            }
            if (cor == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



            var corrections = db.Corrections.Include(c => c.Form4).Include(c => c.Voucher);
            return View(cor.ToList());
        }

        // GET: Corrections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Correction correction = db.Corrections.Find(id);
            if (correction == null)
            {
                return HttpNotFound();
            }
            return View(correction);
        }

        // GET: Corrections/Create
        public ActionResult Create(int? ReceiptID, int? VoucherID)
        {
            Correction cor=null;
            if (ReceiptID.HasValue)
            {
                cor = new Correction { RecieptID = (int)ReceiptID };
                var FaultyRec = db.Form4.Find(ReceiptID);
                ViewBag.Heading = String.Format("Receipt of {0} created on {1:dd-MMM-yyyy} against subledger: {2}", FaultyRec.Citizen?.Name + FaultyRec.RecvdFrom, FaultyRec.PayDate, FaultyRec.Ledger.Ledger1 + " >> " + FaultyRec.SubLedger.Ledger);
            }

            if (cor == null)
            {
                cor = (VoucherID.HasValue) ? new Correction { VoucherID = (int)VoucherID } : null;
                var FaultyRec = db.Vouchers.Find(VoucherID);
                ViewBag.Heading = String.Format("Voucher of {0} created on {1:dd-MMM-yyyy} against subledger: {2}", FaultyRec.PassedBy, FaultyRec.PayDate, FaultyRec.Ledger.Ledger1 + " >> " + FaultyRec.SubLedger.Ledger);
            }
            if (cor == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            


            return View(cor);
        }

        // POST: Corrections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CorrectionID,RecieptID,VoucherID,Amount,Remark")] Correction correction)
        {
            if (ModelState.IsValid)
            {
                correction.CorrectionDate = DateTime.Today;
                db.Corrections.Add(correction);
                db.SaveChanges();
                return RedirectToAction("Index", new { ReceiptID = correction.RecieptID, VoucherID = correction.VoucherID });
            }
                        
            return View(correction);
        }

        // GET: Corrections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Correction correction = db.Corrections.Find(id);
            if (correction == null)
            {
                return HttpNotFound();
            }
            
            return View(correction);
        }

        // POST: Corrections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CorrectionID,RecieptID,VoucherID,Amount,Remark")] Correction correction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(correction).State = EntityState.Modified;
                correction.CorrectionDate = DateTime.Today;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(correction);
        }

        // GET: Corrections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Correction correction = db.Corrections.Find(id);
            if (correction == null)
            {
                return HttpNotFound();
            }
            return View(correction);
        }

        // POST: Corrections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Correction correction = db.Corrections.Find(id);
            db.Corrections.Remove(correction);
            db.SaveChanges();
            return RedirectToAction("Index");
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
