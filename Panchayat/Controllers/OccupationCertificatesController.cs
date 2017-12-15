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
    public class OccupationCertificatesController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: OccupationCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var Occupation=db.OccupationCertificates.Where(a => a.OccupationCertificateID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                Occupation = Occupation.Where(a => a.PersonName == PersonName);
            }
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;
                Occupation = Occupation.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                Occupation = Occupation.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(Occupation.OrderByDescending(a => a.OccupationCertificateID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: OccupationCertificates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OccupationCertificate occupationCertificate = db.OccupationCertificates.Find(id);
            ViewBag.Dets = db.OccupationCertDetails.Where(a => a.OccupationCertificateID == occupationCertificate.OccupationCertificateID);
            if (occupationCertificate == null)
            {
                return HttpNotFound();
            }
            return View(occupationCertificate);
        }

        // GET: OccupationCertificates/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status");
            return View();
        }

        // POST: OccupationCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OccupationCertificateID,PersonName,PersonAddress,MeetingDated,ConstLicNo,ConstLicDate,BuildingDetails,Tdate,SurveyNo,PlotNumber,RefNo,RefDate,HSref,HSrefdate,RegisterTypeID,WEBstatusID,UserID")] OccupationCertificate occupationCertificate)
        {
            if (ModelState.IsValid)
            {
                occupationCertificate.UserID = User.Identity.GetUserId();
                occupationCertificate.WEBstatusID = 1;
                db.OccupationCertificates.Add(occupationCertificate);
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=occupationCertificate.RegisterTypeID });
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status");

            return View(occupationCertificate);
        }
        public ActionResult AddDetails(int? oc)
        {
            ViewBag.oc = oc;
            ViewBag.Dets = db.OccupationCertDetails.Where(a => a.OccupationCertificateID == oc);

            return View();
        }

        // POST: OccupationCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDetails([Bind(Include = "OccupationCertDetailsID,OccupationCertificateID,NameOfTheOwner,FlatNo,HouseNo,HouseTax,GarbageTax")] OccupationCertDetail occupation)
        {
            if (ModelState.IsValid)
            {
                
                db.OccupationCertDetails.Add(occupation);
                db.SaveChanges();
                return RedirectToAction("AddDetails",new {oc=occupation.OccupationCertificateID});
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status");
            return View(occupation);
        }
        public ActionResult ManageDetails(int? id)

        {

            OccupationCertDetail occupation = db.OccupationCertDetails.Find(id);
            if (occupation == null)
            {
                return HttpNotFound();
            }

            return View(occupation);
        }

        // POST: OccupationCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageDetails([Bind(Include = "OccupationCertDetailsID,OccupationCertificateID,NameOfTheOwner,FlatNo,HouseNo,HouseTax,GarbageTax")] OccupationCertDetail occupation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(occupation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddDetails", new { oc = occupation.OccupationCertificateID });
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status");

            return View(occupation);
        }
        // GET: OccupationCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OccupationCertificate occupationCertificate = db.OccupationCertificates.Find(id);
            if (occupationCertificate == null)
            {
                return HttpNotFound();
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status");

            return View(occupationCertificate);
        }

        // POST: OccupationCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OccupationCertificateID,PersonName,PersonAddress,MeetingDated,ConstLicNo,ConstLicDate,BuildingDetails,Tdate,SurveyNo,PlotNumber,RefNo,RefDate,HSref,HSrefdate,RegisterTypeID,WEBstatusID,UserID")] OccupationCertificate occupationCertificate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(occupationCertificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = occupationCertificate.RegisterTypeID });
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status");

            return View(occupationCertificate);
        }

        // GET: OccupationCertificates/Delete/5
   

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
