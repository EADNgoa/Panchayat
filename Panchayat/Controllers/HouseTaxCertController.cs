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
    public class HouseTaxCertsController : EAController
    {

        // GET: ResidenceCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var HouseTaxCert = db.HouseTaxCerts.Where(a => a.HouseTaxCertID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                HouseTaxCert = HouseTaxCert.Where(a => a.PersonName == PersonName);
            }
            if (rt != null)
            {
                HouseTaxCert = HouseTaxCert.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                HouseTaxCert = HouseTaxCert.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(HouseTaxCert.OrderByDescending(a => a.HouseTaxCertID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: ResidenceCertificates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseTaxCert HouseTaxCert = db.HouseTaxCerts.Find(id);
            ViewBag.P = db.Configs.FirstOrDefault() ;
            if (HouseTaxCert == null)
            {
                return HttpNotFound();
            }
            return View(HouseTaxCert);
        }

        // GET: ResidenceCertificates/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;
            if(rt==28)
            {
                ViewBag.st = "N/A";
            }
            return View();
        }

        // POST: ResidenceCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HouseTaxCertID,PersonName,WardNo,PersonAddress,Tdate,MeetingDate,PrevPersonName,Fees,DeveloperName,DeveloperAddress,UserID,RegisterTypeID,WebStatusID")] HouseTaxCert house)
        {
        
            if (ModelState.IsValid)
            {
                house.Tdate = DateTime.Now;
                house.UserID = User.Identity.GetUserId();
                house.WEBstatusID = 1;
                db.HouseTaxCerts.Add(house);
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=house.RegisterTypeID });
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", house.RegisterTypeID);
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", house.WEBstatusID);
            return View(house);
        }

        // GET: ResidenceCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseTaxCert house = db.HouseTaxCerts.Find(id);
            ViewBag.RegisterTypeID = house.RegisterTypeID;
            if (house == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", house.WEBstatusID);
            return View(house);
        }

        // POST: ResidenceCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HouseTaxCertID,PersonName,WardNo,PersonAddress,Tdate,MeetingDate,PrevPersonName,Fees,DeveloperName,DeveloperAddress,UserID,RegisterTypeID,WebStatusID")] HouseTaxCert house)
        {
            ViewBag.RegisterTypeID = house.RegisterTypeID;
          
            if (ModelState.IsValid)
            {
                db.Entry(house).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=house.RegisterTypeID });
            }           
            return View(house);
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
