﻿using System;
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
    [Authorize(Roles = "Boss,Type1")]
    public class IllegalConstructionsController : EAController
    {
        // GET: IllegalConstructions
        public ActionResult Index(int? page, DateTime? dc, int? rt,int? WEBstatusID)
        {

            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status");


            var illegalConstructions = db.IllegalConstructions.Where(x => x.RegisterTypeID > 0);
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;
                illegalConstructions = db.IllegalConstructions.Where(x => x.RegisterTypeID == rt);
            }
            if (dc != null)
            {
                illegalConstructions = illegalConstructions.Where(p => p.DateOfComp == dc);
                page = 1;
            }
            if (WEBstatusID != null)
            {
                illegalConstructions = illegalConstructions.Where(p => p.WEBstatusID == WEBstatusID);
                page = 1;
            }
            else
            {
                int FinYr = MyExtensions.GetFinYr();
                illegalConstructions = illegalConstructions.Where(x => ((DateTime)x.DateOfComp).Year == FinYr);
            }
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(illegalConstructions.OrderByDescending(a=>a.IllegalConID).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: IllegalConstructions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IllegalConstruction illegalConstruction = db.IllegalConstructions.Find(id);
            ViewBag.RegisterTypeID = illegalConstruction.RegisterTypeID;
            if (illegalConstruction == null)
            {
                return HttpNotFound();
            }
            return View(illegalConstruction);
        }

        // GET: IllegalConstructions/Create
        public ActionResult Create(int? rt)
        {
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;

            }

            return View();
        }

        // POST: IllegalConstructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IllegalConID,DateOfComp,NameOfPr,AddressOfPr,NatOfCon,OccasOfCons,ActionTaken,Remarks,UserID,RegisterTypeID")] IllegalConstruction illegalConstruction)
        {

            if (ModelState.IsValid)
            {
                illegalConstruction.UserID = User.Identity.GetUserId();
                illegalConstruction.DateOfComp = DateTime.Now;
                db.IllegalConstructions.Add(illegalConstruction);
                db.SaveChanges();

                return RedirectToAction("Index", new { rt = illegalConstruction.RegisterTypeID });


            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", illegalConstruction.RegisterTypeID);
            return View(illegalConstruction);
        }

        // GET: IllegalConstructions/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IllegalConstruction illegalConstruction = db.IllegalConstructions.Find(id);
            ViewBag.RegisterTypeID = illegalConstruction.RegisterTypeID;
        
            ViewBag.WEBstatusID = new SelectList(db.WEBstatus, "WEBstatusID", "Status", illegalConstruction.WEBstatusID);

            if (illegalConstruction == null)
            {
                return HttpNotFound();
            }
            return View(illegalConstruction);
        }

        // POST: IllegalConstructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IllegalConID,DateOfComp,NameOfPr,AddressOfPr,NatOfCon,OccasOfCons,ActionTaken,Remarks,RegisterTypeID,UserID,WEBstatusID")] IllegalConstruction illegalConstruction, int? rt)
        {
            ViewBag.RegisterTypeID = illegalConstruction.RegisterTypeID;
            illegalConstruction.UserID = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {

                db.Entry(illegalConstruction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { rt = illegalConstruction.RegisterTypeID });
            }

            return View(illegalConstruction);
        }

        // GET: IllegalConstructions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IllegalConstruction illegalConstruction = db.IllegalConstructions.Find(id);
            if (illegalConstruction == null)
            {
                return HttpNotFound();
            }
            return View(illegalConstruction);
        }

        // POST: IllegalConstructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IllegalConstruction illegalConstruction = db.IllegalConstructions.Find(id);
            db.IllegalConstructions.Remove(illegalConstruction);
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
