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
    public class LedgerDetailsController : EAController
    {
        

        // GET: LedgerDetails
        public ActionResult Index(int? page)
        {
            var ledgerDetails = db.LedgerDetails.Include(l => l.SubLedger);
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(db.LedgerDetails.OrderBy(l=>l.SubLedger.Ledger).ThenBy(l=>l.LedgerDetailID).ToPagedList(pageNumber, pageSize));
        }

        // GET: LedgerDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LedgerDetail ledgerDetail = db.LedgerDetails.Find(id);
            if (ledgerDetail == null)
            {
                return HttpNotFound();
            }
            return View(ledgerDetail);
        }

        // GET: LedgerDetails/Create
        public ActionResult Create()
        {
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger");
            return View();
        }

        // POST: LedgerDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LedgerDetailID,SubLedgerID,LedgerDetail1")] LedgerDetail ledgerDetail)
        {
            if (ModelState.IsValid)
            {
                db.LedgerDetails.Add(ledgerDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", ledgerDetail.SubLedgerID);
            return View(ledgerDetail);
        }

        // GET: LedgerDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LedgerDetail ledgerDetail = db.LedgerDetails.Find(id);
            if (ledgerDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", ledgerDetail.SubLedgerID);
            return View(ledgerDetail);
        }

        // POST: LedgerDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LedgerDetailID,SubLedgerID,LedgerDetail1")] LedgerDetail ledgerDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ledgerDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", ledgerDetail.SubLedgerID);
            return View(ledgerDetail);
        }

        // GET: LedgerDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LedgerDetail ledgerDetail = db.LedgerDetails.Find(id);
            if (ledgerDetail == null)
            {
                return HttpNotFound();
            }
            return View(ledgerDetail);
        }

        // POST: LedgerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LedgerDetail ledgerDetail = db.LedgerDetails.Find(id);
            db.LedgerDetails.Remove(ledgerDetail);
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
