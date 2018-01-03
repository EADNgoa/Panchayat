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
    public class IncomeCertificatesController : EAController
    {

        // GET: IncomeCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {

            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var incomeCertificates = db.IncomeCertificates.Where(a => a.IncomeCertificateID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                incomeCertificates = incomeCertificates.Where(a => a.PersonName == PersonName);
            }
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;
                incomeCertificates = incomeCertificates.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                incomeCertificates = incomeCertificates.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(incomeCertificates.OrderByDescending(a => a.IncomeCertificateID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: IncomeCertificates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomeCertificate incomeCertificate = db.IncomeCertificates.Find(id);
            ViewBag.vp = db.Configs.Select(a => a.VP).FirstOrDefault();

            if (incomeCertificate == null)
            {
                return HttpNotFound();
            }
            return View(incomeCertificate);
        }

        // GET: IncomeCertificates/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;
            return View();
        }
        // POST: IncomeCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncomeCertificateID,PersonName,RelationName,Address,IncomeAmt,YearOf,OfficeName,PurposeOf,Inquiry,ReportNo,InquiryDate,Place,PrintDate,UserID,RegisterTypeID,WEBstatusID")] IncomeCertificate incomeCertificate)
        {
            if (ModelState.IsValid)
            {
                incomeCertificate.UserID = User.Identity.GetUserId();
                incomeCertificate.WEBstatusID = 1;
                db.IncomeCertificates.Add(incomeCertificate);
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=incomeCertificate.RegisterTypeID });
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", incomeCertificate.RegisterTypeID);
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", incomeCertificate.WEBstatusID);
            return View(incomeCertificate);
        }

        // GET: IncomeCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomeCertificate incomeCertificate = db.IncomeCertificates.Find(id);
            ViewBag.RegisterTypeID = incomeCertificate.RegisterTypeID;

            if (incomeCertificate == null)
            {
                return HttpNotFound();
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", incomeCertificate.WEBstatusID);
            return View(incomeCertificate);
        }

        // POST: IncomeCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncomeCertificateID,PersonName,RelationName,Address,IncomeAmt,YearOf,OfficeName,PurposeOf,Inquiry,ReportNo,InquiryDate,Place,PrintDate,UserID,RegisterTypeID,WEBstatusID")] IncomeCertificate incomeCertificate)
        {
            ViewBag.RegisterTypeID = incomeCertificate.RegisterTypeID;

            if (ModelState.IsValid)
            {
                db.Entry(incomeCertificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=incomeCertificate.RegisterTypeID });
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", incomeCertificate.WEBstatusID);
            return View(incomeCertificate);
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
