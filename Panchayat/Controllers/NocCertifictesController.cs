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
    public class NocCertifictesController : EAController
    {

        // GET: ResidenceCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var NocCert = db.NocCertifictes.Where(a => a.NocID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                NocCert = NocCert.Where(a => a.PersonName == PersonName);
            }
            if (rt != null)
            {
                NocCert = NocCert.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                NocCert = NocCert.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(NocCert.OrderByDescending(a => a.NocID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: ResidenceCertificates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NocCertificte NocCert = db.NocCertifictes.Find(id);
            ViewBag.Vp = db.Configs.Select(a => a.VP).FirstOrDefault();
            if (NocCert == null)
            {
                return HttpNotFound();
            }
            return View(NocCert);
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
        public ActionResult Create([Bind(Include = "NocID,Hno,No,PersonName,AprovedDate,PrintDate,Address,ElectDeptAdd,UserID,RegisterTypeID,WebStatusID")] NocCertificte noc)
        {

            if (ModelState.IsValid)
            {

                noc.UserID = User.Identity.GetUserId();
                noc.WEBstatusID = 1;
                db.NocCertifictes.Add(noc);
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = noc.RegisterTypeID });
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", noc.RegisterTypeID);
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", noc.WEBstatusID);
            return View(noc);
        }

        // GET: ResidenceCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NocCertificte noc = db.NocCertifictes.Find(id);
            ViewBag.RegisterTypeID = noc.RegisterTypeID;
            if (noc == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", noc.WEBstatusID);
            return View(noc);
        }

        // POST: ResidenceCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NocID,Hno,No,PersonName,AprovedDate,PrintDate,Address,ElectDeptAdd,UserID,RegisterTypeID,WebStatusID")] NocCertificte noc)
        {
            ViewBag.RegisterTypeID = noc.RegisterTypeID;

            if (ModelState.IsValid)
            {
                db.Entry(noc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = noc.RegisterTypeID });
            }
            return View(noc);
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
