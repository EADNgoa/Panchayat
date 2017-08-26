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
    public class SubLedgerTypesController : EAController
    {
        

        // GET: SubLedgerTypes
        public ActionResult Index()
        {
            return View(db.SubLedgerTypes.ToList());
        }

        // GET: SubLedgerTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubLedgerType subLedgerType = db.SubLedgerTypes.Find(id);
            if (subLedgerType == null)
            {
                return HttpNotFound();
            }
            return View(subLedgerType);
        }

        // GET: SubLedgerTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubLedgerTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubLedgerTypeID,SubLedgerType1")] SubLedgerType subLedgerType)
        {
            if (ModelState.IsValid)
            {
                db.SubLedgerTypes.Add(subLedgerType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subLedgerType);
        }

        // GET: SubLedgerTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubLedgerType subLedgerType = db.SubLedgerTypes.Find(id);
            if (subLedgerType == null)
            {
                return HttpNotFound();
            }
            return View(subLedgerType);
        }

        // POST: SubLedgerTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubLedgerTypeID,SubLedgerType1")] SubLedgerType subLedgerType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subLedgerType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subLedgerType);
        }

        // GET: SubLedgerTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubLedgerType subLedgerType = db.SubLedgerTypes.Find(id);
            if (subLedgerType == null)
            {
                return HttpNotFound();
            }
            return View(subLedgerType);
        }

        // POST: SubLedgerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubLedgerType subLedgerType = db.SubLedgerTypes.Find(id);
            db.SubLedgerTypes.Remove(subLedgerType);
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
