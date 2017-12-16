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
    public class CharacterCertificatesController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: ResidenceCertificates
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.Rt = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var characterCertificates = db.CharacterCertificates.Where(a => a.CharacterID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                characterCertificates = characterCertificates.Where(a => a.PersonName == PersonName);
            }
            if (rt != null)
            {
                ViewBag.Rt = rt;
                characterCertificates = characterCertificates.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                characterCertificates = characterCertificates.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(characterCertificates.OrderByDescending(a => a.CharacterID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: ResidenceCertificates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CharacterCertificate characterCertificates = db.CharacterCertificates.Find(id);
            ViewBag.Vp = db.Configs.Select(a => a.VP).FirstOrDefault();
            if (characterCertificates == null)
            {
                return HttpNotFound();
            }
            return View(characterCertificates);
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
        public ActionResult Create([Bind(Include = "CharacterID,PersonName,Age,FatherName,PurposeOf,MotherName,Tdate,Address,WardOf,KnownYears,UserID,RegisterTypeID,WebStatusID")] CharacterCertificate character)
        {
            if (ModelState.IsValid)
            {
                character.UserID = User.Identity.GetUserId();
                character.WEBstatusID = 1;
                db.CharacterCertificates.Add(character);
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=character.RegisterTypeID });
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", character.RegisterTypeID);
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", character.WEBstatusID);
            return View(character);
        }

        // GET: ResidenceCertificates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CharacterCertificate character = db.CharacterCertificates.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", character.RegisterTypeID);
            ViewBag.WebStatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", character.WEBstatusID);
            return View(character);
        }

        // POST: ResidenceCertificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CharacterID,PersonName,Age,PurposeOf,FatherName,MotherName,Tdate,Address,WardOf,KnownYears,UserID,RegisterTypeID,WebStatusID")] CharacterCertificate character)
        {
            ViewBag.RegisterTypeID = character.RegisterTypeID;
          
            if (ModelState.IsValid)
            {
                db.Entry(character).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = character.RegisterTypeID });
            }           
            return View(character);
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
