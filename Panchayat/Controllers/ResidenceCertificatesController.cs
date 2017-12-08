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
    public class ResidenceCertificatesController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: ResidenceCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
           
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var residenceCertificates = db.ResidenceCertificates.Where(a => a.ResidenceCertificateID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                residenceCertificates = residenceCertificates.Where(a => a.PersonName == PersonName);
            }
            if (rt != null)
            {
                ViewBag.Rt = rt;
                residenceCertificates = residenceCertificates.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                residenceCertificates = residenceCertificates.Where(a => a.WebStatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(residenceCertificates.OrderByDescending(a => a.ResidenceCertificateID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: ResidenceCertificates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceCertificate residenceCertificate = db.ResidenceCertificates.Find(id);
            ViewBag.ph = db.Configs.Select(a => a.PanchHead).FirstOrDefault();
            if (residenceCertificate == null)
            {
                return HttpNotFound();
            }
            return View(residenceCertificate);
        }

        // GET: ResidenceCertificates/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;
            return View();
        }

        // POST: ResidenceCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResidenceCertificateID,PersonName,BirthDate,BirthPlace,NameOfMother,NameOfFather,Address,FromDate,TillDate,IsDead,UserID,RegisterTypeID,WebStatusID")] ResidenceCertificate residenceCertificate)
        {
            if (ModelState.IsValid)
            {
                residenceCertificate.UserID = User.Identity.GetUserId();
                residenceCertificate.WebStatusID = 1;
                db.ResidenceCertificates.Add(residenceCertificate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", residenceCertificate.RegisterTypeID);
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", residenceCertificate.WebStatusID);
            return View(residenceCertificate);
        }

        // GET: ResidenceCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceCertificate residenceCertificate = db.ResidenceCertificates.Find(id);
            if (residenceCertificate == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", residenceCertificate.RegisterTypeID);
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", residenceCertificate.WebStatusID);
            return View(residenceCertificate);
        }

        // POST: ResidenceCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResidenceCertificateID,PersonName,BirthDate,BirthPlace,NameOfMother,NameOfFather,Address,FromDate,TillDate,TDate,IsDead,UserID,RegisterTypeID,WebStatusID")] ResidenceCertificate residenceCertificate)
        {
            ViewBag.RegisterTypeID = residenceCertificate.RegisterTypeID;
          
            if (ModelState.IsValid)
            {
                db.Entry(residenceCertificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }           
            return View(residenceCertificate);
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
