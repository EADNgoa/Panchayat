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
    public class Form1Controller : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: Form1
        public ActionResult Index()
        {
            var form1 = db.Form1.Include(f => f.Ledger).Include(f => f.SubLedger);
            return View(form1.ToList());
        }

        // GET: Form1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form1 form1 = db.Form1.Find(id);
            if (form1 == null)
            {
                return HttpNotFound();
            }
            return View(form1);
        }

        // GET: Form1/Create
        public ActionResult Create()
        {
            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1");
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger");
            return View();
        }

        // POST: Form1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Form1ID,LedgerID,SubLedgerID,EntryDate,Particulars,Amount,Progressive,Total")] Form1 form1)
        {
            if (ModelState.IsValid)
            {
                db.Form1.Add(form1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1", form1.LedgerID);
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", form1.SubLedgerID);
            return View(form1);
        }

        // GET: Form1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form1 form1 = db.Form1.Find(id);
            if (form1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1", form1.LedgerID);
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", form1.SubLedgerID);
            return View(form1);
        }

        // POST: Form1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Form1ID,LedgerID,SubLedgerID,EntryDate,Particulars,Amount,Progressive,Total")] Form1 form1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(form1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1", form1.LedgerID);
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", form1.SubLedgerID);
            return View(form1);
        }

        // GET: Form1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form1 form1 = db.Form1.Find(id);
            if (form1 == null)
            {
                return HttpNotFound();
            }
            return View(form1);
        }

        // POST: Form1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Form1 form1 = db.Form1.Find(id);
            db.Form1.Remove(form1);
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
