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
using Microsoft.AspNet.Identity;

namespace Panchayat.Controllers
{
    public class LeaveApplicationsController : EAAController
    {
      

        // GET: LeaveApplications
        public ActionResult Index()
        {
            var leaveApplications = db.LeaveApplications.Include(l => l.LeaveType).Include(l => l.Status);
            return View(leaveApplications.ToList());
        }

  

        // GET: LeaveApplications/Create
        public ActionResult Create()
        {
            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "Status1");
            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeaveApplicationID,ApplicationDate,UserID,LeaveTypeID,LeaveStartDate,NoOfDays,StatusID,StatusBy,StatusDate")] LeaveApplication leaveApplication)
        {
            leaveApplication.UserID = User.Identity.GetUserId();

            var leaveEnt = db.LeaveEntitlements.Where(a=>a.LeaveTypeID==leaveApplication.LeaveTypeID && a.LeaveYear==leaveApplication.LeaveStartDate.Value.Year).FirstOrDefault().LeaveDays;
            var LeavBal = db.LeaveBalances.Where(a => a.UserID == leaveApplication.UserID && a.LeaveTypeID == leaveApplication.LeaveTypeID && a.LeaveYear == leaveApplication.LeaveStartDate.Value.Year).FirstOrDefault();


            decimal bal = 0 ;
            
            if (LeavBal != null)
            {
                bal = (decimal)LeavBal.LeaveDays;
                bal = (decimal)leaveApplication.NoOfDays + bal;
            }
            if (bal == 0)
            {
                bal = 0;
            }
            if (ModelState.IsValid && bal <= leaveEnt)
            {
                leaveApplication.ApplicationDate = DateTime.Now;
                leaveApplication.StatusID = 3;
                db.LeaveApplications.Add(leaveApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName", leaveApplication.LeaveTypeID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "Status1", leaveApplication.StatusID);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveApplication = db.LeaveApplications.Find(id);
            if (leaveApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName", leaveApplication.LeaveTypeID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "Status1", leaveApplication.StatusID);
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeaveApplicationID,ApplicationDate,UserID,LeaveTypeID,LeaveStartDate,NoOfDays,StatusID,StatusBy,StatusDate")] LeaveApplication leaveApplication)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(leaveApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LeaveTypeID = new SelectList(db.LeaveTypes, "LeaveTypeID", "LeaveTypeName", leaveApplication.LeaveTypeID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "Status1", leaveApplication.StatusID);
            return View(leaveApplication);
        }

        public ActionResult LeaveApproval(int? ap, int? rp)
        {
            var ApprovalList = db.LeaveApplications.Where(a => a.StatusID == 3).ToList();
          
                    if (ap != null)
                    {
                        var AppRec = db.LeaveApplications.Find(ap);
                        AppRec.StatusID = 1;
                        AppRec.StatusDate = DateTime.Now;
                        AppRec.StatusBy = User.Identity.GetUserId();
                        db.Entry(AppRec).State = EntityState.Modified;

                        var LeavBal = db.LeaveBalances.Where(a=>a.UserID == AppRec.UserID && a.LeaveTypeID ==AppRec.LeaveTypeID && a.LeaveYear == AppRec.LeaveStartDate.Value.Year ).FirstOrDefault();
                        if (LeavBal == null)
                        {
                            var item = new LeaveBalance
                            {
                                LeaveDays = AppRec.NoOfDays,
                                LeaveTypeID= (int)AppRec.LeaveTypeID,
                                LeaveYear= AppRec.ApplicationDate.Value.Year,
                                UserID=AppRec.UserID,                                
                            };
                    db.LeaveBalances.Add(item);
                        }
                        else
                        {
                            LeavBal.LeaveDays += AppRec.NoOfDays;
                        }


                    }
                    if (rp != null)
                    {
                        var AppRec = db.LeaveApplications.Find(rp);
                        AppRec.StatusID = 2;
                        AppRec.StatusDate = DateTime.Now;
                        AppRec.StatusBy = User.Identity.GetUserId();
                        db.Entry(AppRec).State = EntityState.Modified;

                    }
           
                    db.SaveChanges();

           return View(ApprovalList);

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
