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

namespace Panchayat.Controllers
{
    public class InwardsController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: Inwards
        public ActionResult Index(int? page, DateTime? dr,DateTime? dl)
        {
           
            var inwards = db.Inwards.Where(x=>x.InwardID>0);
            if (dr != null)
            {
                inwards = db.Inwards.Where(x => x.DateOfReciept == dr);
            }
            if (dl != null)
            {
                inwards = db.Inwards.Where(x => x.DateOfLett == dl);
            }
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(inwards.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Inwards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inward inward = db.Inwards.Find(id);
            if (inward == null)
            {
                return HttpNotFound();
            }
            return View(inward);
        }

        // GET: Inwards/Create
        public ActionResult Create()
        {
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1");
            return View();
        }

        // POST: Inwards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InwardID,DateOfReciept,FromWhereRec,NoOfLett,DateOfLett,FileNo,SubjectMatter,ActionTaken,Remark,RegisterTypeID")] Inward inward)
        {
            inward.RegisterTypeID = 2;
            if (ModelState.IsValid)
            {
                
                db.Inwards.Add(inward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", inward.RegisterTypeID);
            return View(inward);
        }

        // GET: Inwards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inward inward = db.Inwards.Find(id);
            if (inward == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", inward.RegisterTypeID);
            return View(inward);
        }

        // POST: Inwards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InwardID,DateOfReciept,FromWhereRec,NoOfLett,DateOfLett,FileNo,SubjectMatter,ActionTaken,Remark,RegisterTypeID")] Inward inward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", inward.RegisterTypeID);
            return View(inward);
        }

        // GET: Inwards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inward inward = db.Inwards.Find(id);
            if (inward == null)
            {
                return HttpNotFound();
            }
            return View(inward);
        }

        // POST: Inwards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inward inward = db.Inwards.Find(id);
            db.Inwards.Remove(inward);
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
