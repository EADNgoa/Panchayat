using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Panchayat;

namespace Panchayat.Controllers
{
    public class CertificateRequirementsController : EAController
    {
        

        // GET: CertificateRequirements
        public ActionResult Index(int? RegisterTypeID)
        {
            var certificateRequirements = db.CertificateRequirements.Where(c => c.CertificateRequirementID>0);
            if (RegisterTypeID != null)
            {
                certificateRequirements = certificateRequirements.Where(r=>r.RegisterTypeID == RegisterTypeID);
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes.Where(a => a.RegisterTypeID > 24), "RegisterTypeID", "RegisterType1");

            return View(certificateRequirements.ToList());
        }

        // GET: CertificateRequirements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificateRequirement certificateRequirement = db.CertificateRequirements.Find(id);
            if (certificateRequirement == null)
            {
                return HttpNotFound();
            }
            return View(certificateRequirement);
        }

        // GET: CertificateRequirements/Create
        public ActionResult Create()
        {
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes.Where(a=>a.RegisterTypeID > 24), "RegisterTypeID", "RegisterType1");
            return View();
        }

        // POST: CertificateRequirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CertificateRequirementID,RegisterTypeID,CertificateName")] CertificateRequirement certificateRequirement)
        {
            if (ModelState.IsValid)
            {
                db.CertificateRequirements.Add(certificateRequirement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes.Where(a => a.RegisterTypeID > 24), "RegisterTypeID", "RegisterType1", certificateRequirement.RegisterTypeID);
            return View(certificateRequirement);
        }

        // GET: CertificateRequirements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificateRequirement certificateRequirement = db.CertificateRequirements.Find(id);
            if (certificateRequirement == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", certificateRequirement.RegisterTypeID);
            return View(certificateRequirement);
        }

        // POST: CertificateRequirements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CertificateRequirementID,RegisterTypeID,CertificateName")] CertificateRequirement certificateRequirement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(certificateRequirement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes.Where(a => a.RegisterTypeID > 24), "RegisterTypeID", "RegisterType1", certificateRequirement.RegisterTypeID);
            return View(certificateRequirement);
        }

        // GET: CertificateRequirements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificateRequirement certificateRequirement = db.CertificateRequirements.Find(id);
            if (certificateRequirement == null)
            {
                return HttpNotFound();
            }
            return View(certificateRequirement);
        }

        // POST: CertificateRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CertificateRequirement certificateRequirement = db.CertificateRequirements.Find(id);
            db.CertificateRequirements.Remove(certificateRequirement);
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
