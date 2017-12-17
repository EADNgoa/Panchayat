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
    public class PovertyCertificatesController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: PovertyCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var povertyCertificates = db.PovertyCertificates.Where(a=>a.PovertyCertificateID >0);
            if(!string.IsNullOrEmpty(PersonName))
            {
                povertyCertificates = povertyCertificates.Where(a => a.PersonName == PersonName);
            }
            if(rt!=null)
            {
                ViewBag.RegisterTypeID = rt;
                povertyCertificates = povertyCertificates.Where(a => a.RegisterTypeID == rt);

            }
            if(WEBstatusID!= null)
            {
                povertyCertificates = povertyCertificates.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(povertyCertificates.OrderByDescending(a => a.PovertyCertificateID).ToList().ToPagedList(pageNumber, pageSize));
        }


        // GET: PovertyCertificates/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;
            return View();
        }

        // POST: PovertyCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PovertyCertificateID,RegisterTypeID,PersonName,PersonAddress,RequestedBy,AddOfPerReqBy,UserID,WEBstatusID")] PovertyCertificate povertyCertificate)
        {
            if (ModelState.IsValid)
            {
                povertyCertificate.UserID = User.Identity.GetUserId();
                povertyCertificate.WEBstatusID = 1;
                db.PovertyCertificates.Add(povertyCertificate);
                db.SaveChanges();
                return RedirectToAction("Index",new { rt = povertyCertificate.RegisterTypeID });
            }

            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", povertyCertificate.WEBstatusID);
            return View(povertyCertificate);
        }
        // GET: PovertyCertificates1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PovertyCertificate povertyCertificate = db.PovertyCertificates.Find(id);
            ViewBag.ph = db.Configs.Select(a=>a.PanchHead).FirstOrDefault();
            if (povertyCertificate == null)
            {
                return HttpNotFound();
            }
            return View(povertyCertificate);
        }

        // GET: PovertyCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PovertyCertificate povertyCertificate = db.PovertyCertificates.Find(id);
            ViewBag.RegisterTypeID = povertyCertificate.RegisterTypeID;
            if (povertyCertificate == null)
            {
                return HttpNotFound();
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", povertyCertificate.WEBstatusID);
            return View(povertyCertificate);
        }

        // POST: PovertyCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PovertyCertificateID,RegisterTypeID,PersonName,AddOfPerReqBy,PersonAddress,RequestedBy,UserID,WEBstatusID")] PovertyCertificate povertyCertificate)
        {

            ViewBag.RegisterTypeID = povertyCertificate.RegisterTypeID;

            if (ModelState.IsValid)
            {
                db.Entry(povertyCertificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", povertyCertificate.WEBstatusID);
            return View(povertyCertificate);
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
