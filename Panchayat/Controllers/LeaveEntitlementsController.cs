using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Panchayat;
using Panchayat.Models;

namespace Panchayat.Controllers
{
    public class LeaveEntitlementsController : EAAController
    {

        // GET: LeaveEntitlements
        public ActionResult Index()
        {
            var leaveEntitlements = db.LeaveEntitlements.Include(l => l.LeaveType);
            return View(leaveEntitlements.ToList());
        }

        // GET: LeaveEntitlements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveEntitlement leaveEntitlement = db.LeaveEntitlements.Find(id);
            if (leaveEntitlement == null)
            {
                return HttpNotFound();
            }
            return View(leaveEntitlement);
        }

        // GET: LeaveEntitlements/Create
        public ActionResult Create()
        {
            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName");
            return View();
        }

        // POST: LeaveEntitlements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeaveEntitlementID,LeaveYear,LeaveTypeID,LeaveDays,Attendance")] LeaveEntitlement leaveEntitlement)
        {
            if (ModelState.IsValid)
            {
                db.LeaveEntitlements.Add(leaveEntitlement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName", leaveEntitlement.LeaveTypeID);
            return View(leaveEntitlement);
        }

        // GET: LeaveEntitlements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveEntitlement leaveEntitlement = db.LeaveEntitlements.Find(id);
            if (leaveEntitlement == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName", leaveEntitlement.LeaveTypeID);
            return View(leaveEntitlement);
        }

        // POST: LeaveEntitlements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeaveEntitlementID,LeaveYear,LeaveTypeID,LeaveDays,Attendance")] LeaveEntitlement leaveEntitlement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaveEntitlement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName", leaveEntitlement.LeaveTypeID);
            return View(leaveEntitlement);
        }

        // GET: LeaveEntitlements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveEntitlement leaveEntitlement = db.LeaveEntitlements.Find(id);
            if (leaveEntitlement == null)
            {
                return HttpNotFound();
            }
            return View(leaveEntitlement);
        }

        // POST: LeaveEntitlements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveEntitlement leaveEntitlement = db.LeaveEntitlements.Find(id);
            db.LeaveEntitlements.Remove(leaveEntitlement);
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
