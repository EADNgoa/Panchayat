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
    public class SubLedgersController : EAController
    {
        

        // GET: SubLedgers
        public ActionResult Index(int? page ,string SearchString, string subSearchString,int? SubType)
        {

            var SubLedger = db.SubLedgers.Where(s => s.Ledger.Length > 2);
          
         
            ViewBag.SubType = new SelectList(db.SubLedgerTypes, "SubLedgerTypeID", "SubLedgerType1");


            if (!string.IsNullOrEmpty(SearchString))
            {
                SubLedger = SubLedger.Where(l => l.Ledger1.Ledger1.Contains(SearchString) );

                page = 1;
            }
            if (!string.IsNullOrEmpty(subSearchString))
            {
                SubLedger = SubLedger.Where(p => p.Ledger.Contains(subSearchString));

                page = 1;
            }
            if (SubType!= null)
            {
               SubLedger = SubLedger.Where(e => e.SubLedgerTypeID == SubType);
                ViewBag.ID = SubType;
            }


            SubLedger = SubLedger.OrderBy(e => e.Ledger);

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
           return View(SubLedger.ToPagedList(pageNumber, pageSize));
        }

        // GET: SubLedgers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Sl = db.SubLedgers.Find(id);
            if (Sl == null)
            {
                return HttpNotFound();
            }
            ViewBag.ForSL = $"For: {Sl.Ledger} >> {Sl.Ledger}";

            ViewBag.LedDets = db.LedgerDetails.Where(ld => ld.SubLedgerID == Sl.SubLedgerID);
                        
            return View(new LedgerDetail { SubLedgerID = Sl.SubLedgerID });
        }


        // POST: LedgerDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLedDet([Bind(Include = "LedgerDetailID,SubLedgerID,LedgerDetail1")] LedgerDetail ledgerDetail)
        {
            if (ModelState.IsValid)
            {
                db.LedgerDetails.Add(ledgerDetail);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = ledgerDetail.SubLedgerID });
            }
                return RedirectToAction("Index");

        }

        public ActionResult EditLedDet(int id)
        {
            var ld = db.LedgerDetails.Find(id);
            if (ld == null)
            {
                return HttpNotFound();
            }

            var Sl = db.SubLedgers.Find(ld.SubLedgerID);
            
            ViewBag.ForSL = $"For: {Sl.Ledger} >> {Sl.Ledger}";
            
            return View("DetailsEditLedg" ,ld);

        }

        // POST: LedgerDetails/EditLedDet
        // Edit LedgerDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLedDet([Bind(Include = "LedgerDetailID,SubLedgerID,LedgerDetail1")] LedgerDetail ledgerDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ledgerDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = ledgerDetail.SubLedgerID });
            }
            
            return RedirectToAction("Index");

        }

        // GET: SubLedgers/Create
        public ActionResult Create()
        {
            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1");
            ViewBag.SubLedgerTypeID = new SelectList(db.SubLedgerTypes, "SubLedgerTypeID", "SubLedgerType1");
            return View();
        }

        // POST: SubLedgers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubLedgerID,LedgerID,SubLedgerTypeID,Ledger,HasDemand")] SubLedger subLedger)
        {
            if (ModelState.IsValid)
            {
                db.SubLedgers.Add(subLedger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1", subLedger.LedgerID);
            ViewBag.SubLedgerTypeID = new SelectList(db.SubLedgerTypes, "SubLedgerTypeID", "SubLedgerType1", subLedger.SubLedgerTypeID);
            return View(subLedger);
        }

        // GET: SubLedgers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubLedger subLedger = db.SubLedgers.Find(id);
            if (subLedger == null)
            {
                return HttpNotFound();
            }
            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1", subLedger.LedgerID);
            ViewBag.SubLedgerTypeID = new SelectList(db.SubLedgerTypes, "SubLedgerTypeID", "SubLedgerType1", subLedger.SubLedgerTypeID);
            return View(subLedger);
        }

        // POST: SubLedgers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubLedgerID,LedgerID,SubLedgerTypeID,Ledger,HasDemand")] SubLedger subLedger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subLedger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LedgerID = new SelectList(db.Ledgers, "LedgerID", "Ledger1", subLedger.LedgerID);
            ViewBag.SubLedgerTypeID = new SelectList(db.SubLedgerTypes, "SubLedgerTypeID", "SubLedgerType1", subLedger.SubLedgerTypeID);
            return View(subLedger);
        }

        // GET: SubLedgers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubLedger subLedger = db.SubLedgers.Find(id);
            if (subLedger == null)
            {
                return HttpNotFound();
            }
            return View(subLedger);
        }

        // POST: SubLedgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubLedger subLedger = db.SubLedgers.Find(id);
            db.SubLedgers.Remove(subLedger);
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
