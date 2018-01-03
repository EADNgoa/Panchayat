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
    public class BNDsController : EAController
    {

        // GET: BNDs
        public ActionResult Index(int? page, string PersonName, int? rt, int? WEBstatusID)
        {
            ViewBag.RegisterTypeID = rt;
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");
            var BNDCertificates = db.BNDs.Where(a => a.BNDID > 0);
            if (!string.IsNullOrEmpty(PersonName))
            {
                BNDCertificates = BNDCertificates.Where(a => a.InformantsName == PersonName);
            }
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;
                BNDCertificates = BNDCertificates.Where(a => a.RegisterTypeID == rt);

            }
            if (WEBstatusID != null)
            {
                BNDCertificates = BNDCertificates.Where(a => a.WEBstatusID == WEBstatusID);
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(BNDCertificates.OrderByDescending(a => a.BNDID).ToList().ToPagedList(pageNumber, pageSize)); 
        }

        // GET: BNDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BND bND = db.BNDs.Find(id);
            if (bND == null)
            {
                return HttpNotFound();
            }
            return View(bND);
        }

        // GET: BNDs/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;
            return View();
        }

        // POST: BNDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BNDID,ChildName,Gender,DateOfBirth,TDate,PlaceOfBirth,PlaceOfBirthHouse,NameOfMother,UIDmother,NameOfFather,UIDfather,GrandFather,GrandMother,AddressParentsBirth,PermAddress,InformantsName,InformantAddress,NameOfTown,TownOrVillage,NameOfDistrict,NameOfState,ReligionOfFamily,AnyOtherReligion,FEduc,MEduc,FOccup,MOccup,AgeOfMotherMarraige,AgeOfMotherBirth,NoOfChild,AttentionType,DeliveryMethod,BirthWeight,DurationOfPregnancy,UserID,WEBstatusID,RegisterTypeID")] BND bND)
        {
            if (ModelState.IsValid)
            {
                bND.UserID = User.Identity.GetUserId();
                bND.WEBstatusID = 1;
                db.BNDs.Add(bND);
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = bND.RegisterTypeID });
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", bND.RegisterTypeID);
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", bND.WEBstatusID);
            return View(bND);
        }

        // GET: BNDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BND bND = db.BNDs.Find(id);
            ViewBag.RegisterTypeID = bND.RegisterTypeID;

            if (bND == null)
            {
                return HttpNotFound();
            }
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", bND.WEBstatusID);
            return View(bND);
        }

        // POST: BNDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BNDID,ChildName,Gender,DateOfBirth,TDate,PlaceOfBirth,PlaceOfBirthHouse,NameOfMother,UIDmother,NameOfFather,UIDfather,GrandFather,GrandMother,AddressParentsBirth,PermAddress,InformantsName,InformantAddress,NameOfTown,TownOrVillage,NameOfDistrict,NameOfState,ReligionOfFamily,AnyOtherReligion,FEduc,MEduc,FOccup,MOccup,AgeOfMotherMarraige,AgeOfMotherBirth,NoOfChild,AttentionType,DeliveryMethod,BirthWeight,DurationOfPregnancy,UserID,WEBstatusID,RegisterTypeID")] BND bND)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bND).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = bND.RegisterTypeID });
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", bND.RegisterTypeID);
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WebStatusID", "Status", bND.WEBstatusID);
            return View(bND);
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
