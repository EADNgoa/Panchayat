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
using Microsoft.AspNet.Identity;

namespace Panchayat.Controllers
{
    public class ConstLicenseCertsController : EAController
    {

        // GET: ConstLicenseCerts
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var construction = db.ConstLicenseCerts.Where(a => a.ConstLicenseID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                construction = construction.Where(a => a.OwnersOfHouse == PersonName);
            }
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;
                construction = construction.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                construction = construction.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(construction.OrderByDescending(a => a.ConstLicenseID).ToList().ToPagedList(pageNumber, pageSize));

        }

        // GET: ConstLicenseCerts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstLicenseCert constLicenseCert = db.ConstLicenseCerts.Find(id);
            ViewBag.ph = db.Configs.Select(a => a.PanchHead).FirstOrDefault();

            if (constLicenseCert == null)
            {
                return HttpNotFound();
            }
            return View(constLicenseCert);
        }

        // GET: ConstLicenseCerts/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;

            return View();
        }

        // POST: ConstLicenseCerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConstLicenseID,OwnersOfHouse,MeetingDated,BuildingType,PropertyZone,SurveyNo,SubDivision,OwnwersAddress,DeveloperAddress,DeveloperName,SubDivision,OrderNo,Tdate,RefNo,RefDate,ValidUpTo,RecieptNo,RecieptDate,ConstFees,SanitationFees,RegisterTypeID,WEBstatusID,UserID")] ConstLicenseCert constLicenseCert)
        {
            if (ModelState.IsValid)
            {
                constLicenseCert.Tdate = DateTime.Now;
                constLicenseCert.UserID = User.Identity.GetUserId();
                constLicenseCert.WEBstatusID = 1;
                db.ConstLicenseCerts.Add(constLicenseCert);
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=constLicenseCert.RegisterTypeID });
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", constLicenseCert.RegisterTypeID);
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", constLicenseCert.WEBstatusID);
            return View(constLicenseCert);
        }

        // GET: ConstLicenseCerts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstLicenseCert constLicenseCert = db.ConstLicenseCerts.Find(id);
            if (constLicenseCert == null)
            {
                return HttpNotFound();
            }
 
            return View(constLicenseCert);
        }

        // POST: ConstLicenseCerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConstLicenseID, OwnersOfHouse, MeetingDated, BuildingType, PropertyZone, SurveyNo, SubDivision, OwnwersAddress, DeveloperAddress, DeveloperName, SubDivision, OrderNo, Tdate, RefNo, RefDate, ValidUpTo, RecieptNo, RecieptDate, ConstFees, SanitationFees, RegisterTypeID, WEBstatusID, UserID")] ConstLicenseCert constLicenseCert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(constLicenseCert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = constLicenseCert.RegisterTypeID });
            }

            return View(constLicenseCert);
        }

        // GET: ConstLicenseCerts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstLicenseCert constLicenseCert = db.ConstLicenseCerts.Find(id);
            if (constLicenseCert == null)
            {
                return HttpNotFound();
            }
            return View(constLicenseCert);
        }

        // POST: ConstLicenseCerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConstLicenseCert constLicenseCert = db.ConstLicenseCerts.Find(id);
            db.ConstLicenseCerts.Remove(constLicenseCert);
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
