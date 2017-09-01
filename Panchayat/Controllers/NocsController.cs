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
    public class NocsController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: Nocs
        public ActionResult Index(int? page,int? rt,DateTime? dor,DateTime? doi,bool? ior,DateTime? dov)
        {
            ViewBag.RegisterTypeID = rt;
            var nocs = db.Nocs.Where(n => n.NocID > 0);
            if (rt!=null)
            {
                nocs = nocs.Where(n => n.RegisterTypeID == rt);
            }
            if(dor!=null)
            {
                nocs = nocs.Where(n => n.DateOfReciept == dor);
            }
            if (doi != null)
            {
                nocs = nocs.Where(n => n.DateOfComm == doi);
            }
            if (dov != null)
            {
                nocs = nocs.Where(n => n.DateOfReciept == dov);
            }
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(nocs.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Nocs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noc noc = db.Nocs.Find(id);
            if (noc == null)
            {
                return HttpNotFound();
            }
            return View(noc);
        }

        // GET: Nocs/Create
        public ActionResult Create(int? rt)
        {
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;
            }
            return View();
        }

        // POST: Nocs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NocID,DateOfReciept,NameAndAdressAppl,NatOfNoc,DateOfVp,NoOfResolution,IssueOrReject,RejectedReason,DateOfComm,Remarks,RegisterTypeID")] Noc noc)
        {
            ViewBag.RegisterTypeID = noc.RegisterTypeID;
            if (ModelState.IsValid)
            {
                db.Nocs.Add(noc);
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=noc.RegisterTypeID });
            }

            return View(noc);
        }

        // GET: Nocs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noc noc = db.Nocs.Find(id);
            ViewBag.RegisterTypeID = noc.RegisterTypeID;
            if (noc == null)
            {
                return HttpNotFound();
            }
            return View(noc);
        }

        // POST: Nocs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NocID,DateOfReciept,NameAndAdressAppl,NatOfNoc,DateOfVp,NoOfResolution,IssueOrReject,RejectedReason,DateOfComm,Remarks,RegisterTypeID")] Noc noc)
        {
            ViewBag.RegisterTypeID = noc.RegisterTypeID;
            if (ModelState.IsValid)
            {
                db.Entry(noc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { rt=noc.RegisterTypeID});
            }
            return View(noc);
        }

        // GET: Nocs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noc noc = db.Nocs.Find(id);
            if (noc == null)
            {
                return HttpNotFound();
            }
            return View(noc);
        }

        // POST: Nocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Noc noc = db.Nocs.Find(id);
            db.Nocs.Remove(noc);
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
