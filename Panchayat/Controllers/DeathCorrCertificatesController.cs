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
    public class DeathCorrCertificatesController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: DeathCorrCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var DeathCertificates = db.DeathCorrCertificates.Where(a => a.DeathCorrCertificateID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                DeathCertificates = DeathCertificates.Where(a => a.FromName == PersonName);
            }
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;
                DeathCertificates = DeathCertificates.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                DeathCertificates = DeathCertificates.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(DeathCertificates.OrderByDescending(a => a.DeathCorrCertificateID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: DeathCorrCertificates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeathCorrCertificate deathCorrCertificate = db.DeathCorrCertificates.Find(id);
            ViewBag.vp = db.Configs.Select(a => a.VP).FirstOrDefault();

            if (deathCorrCertificate == null)
            {
                return HttpNotFound();
            }
            return View(deathCorrCertificate);
        }

        // GET: DeathCorrCertificates/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;

            return View();
        }

        // POST: DeathCorrCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeathCorrCertificateID,FromName,FromAddress,TDate,BirthOf,BornOn,BirthPlace,FromWrongName,InsteadFWN,NameOfFather,InsteadNF,NameOfMother,InsteadNM,NameOfGrandMother,InsteadNGM,NameOfGrandFather,InsteadNGF,BirthDeathName,UserID,RegisterTypeID,WEBstatusID")] DeathCorrCertificate deathCorrCertificate)
        {
            if (ModelState.IsValid)
            {
                deathCorrCertificate.UserID = User.Identity.GetUserId();
                deathCorrCertificate.WEBstatusID = 1;
                deathCorrCertificate.TDate = DateTime.Now;
                db.DeathCorrCertificates.Add(deathCorrCertificate);
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = deathCorrCertificate.RegisterTypeID } );
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", deathCorrCertificate.RegisterTypeID);
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", deathCorrCertificate.WEBstatusID);
            return View(deathCorrCertificate);
        }

        // GET: DeathCorrCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeathCorrCertificate deathCorrCertificate = db.DeathCorrCertificates.Find(id);
            ViewBag.RegisterTypeID = deathCorrCertificate.RegisterTypeID;

            if (deathCorrCertificate == null)
            {
                return HttpNotFound();
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", deathCorrCertificate.WEBstatusID);
            return View(deathCorrCertificate);
        }

        // POST: DeathCorrCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeathCorrCertificateID,FromName,FromAddress,TDate,BirthOf,BornOn,BirthPlace,FromWrongName,InsteadFWN,NameOfFather,InsteadNF,NameOfMother,InsteadNM,NameOfGrandMother,InsteadNGM,NameOfGrandFather,InsteadNGF,BirthDeathName,UserID,RegisterTypeID,WEBstatusID")] DeathCorrCertificate deathCorrCertificate)
        {
            ViewBag.RegisterTypeID = deathCorrCertificate.RegisterTypeID;

            if (ModelState.IsValid)
            {
                db.Entry(deathCorrCertificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", deathCorrCertificate.RegisterTypeID);
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", deathCorrCertificate.WEBstatusID);
            return View(deathCorrCertificate);
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
