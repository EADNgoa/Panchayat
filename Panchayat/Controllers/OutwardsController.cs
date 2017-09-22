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
    [Authorize(Roles = "Boss,Type1")]
    public class OutwardsController : EAController
    {
        // GET: Outwards
        public ActionResult Index(int? page,DateTime? dod,DateTime? frd,string frn,int? rt)
        {

            var outwards = db.Outwards.Where(o => o.OutwardID>0);
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;

                outwards = db.Outwards.Where(x => x.RegisterTypeID == rt);

            }
            if (dod!=null)
            {
                outwards = outwards.Where(x=>x.DateOfDisp==dod);
            }
            if (frd != null)
            {
                outwards = outwards.Where(x => x.FileReferenceDate == frd);
            }
            if (dod == null && frd == null)
            {
                int FinYr = MyExtensions.GetFinYr();
                outwards = outwards.Where(x => ((DateTime)x.FileReferenceDate).Year == FinYr);
            }

            if (frn?.Length>0)
            {
                outwards = outwards.Where(x => x.FileReferenceNo == frn);
            }
            


            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(outwards.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Outwards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outward outward = db.Outwards.Find(id);
            if (outward == null)
            {
                return HttpNotFound();
            }
            return View(outward);
        }

        // GET: Outwards/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID =rt;
            return View();
        }

        // POST: Outwards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OutwardID,DateOfDisp,ToWhom,FileReferenceNo,FileReferenceDate,SubjectMatter,PostageDrawn,PostageExtended,Remark,RegisterTypeID")] Outward outward)
        {
             ViewBag.RegisterTypeID = outward.RegisterTypeID;
            if (ModelState.IsValid)
            {
                db.Outwards.Add(outward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(outward);
        }

        // GET: Outwards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outward outward = db.Outwards.Find(id);
            ViewBag.RegisterTypeID = outward.RegisterTypeID;
            if (outward == null)
            {
                return HttpNotFound();
            }
            return View(outward);
        }

        // POST: Outwards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OutwardID,DateOfDisp,ToWhom,FileReferenceNo,FileReferenceDate,SubjectMatter,PostageDrawn,PostageExtended,Remark,RegisterTypeID")] Outward outward)
        {
            ViewBag.RegisterTypeID = outward.RegisterTypeID;
            if (ModelState.IsValid)
            {
                db.Entry(outward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", outward.RegisterTypeID);
            return View(outward);
        }

        // GET: Outwards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outward outward = db.Outwards.Find(id);
            if (outward == null)
            {
                return HttpNotFound();
            }
            return View(outward);
        }

        // POST: Outwards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Outward outward = db.Outwards.Find(id);
            db.Outwards.Remove(outward);
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
