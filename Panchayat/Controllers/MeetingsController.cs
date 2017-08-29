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
    public class MeetingsController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: Meetings
        public ActionResult Index(bool? poa,string pn,DateTime? md,int? page,int? rt)
        {
            var meetings = db.Meetings.Where(x=>x.MeetingID>0);

            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;

                meetings = db.Meetings.Where(x => x.RegisterTypeID == rt);

            }
            if (!string.IsNullOrEmpty(pn))
            {
                meetings = meetings.Where(p => p.ProposerName.Contains(pn));

                page = 1;
            }
            if(md !=null)
            {
                meetings = meetings.Where(p => p.MeetingDate ==md);
                page = 1;
            }
         
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(meetings.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Meetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // GET: Meetings/Create
        public ActionResult Create(int? rt)
        {
            if (rt != null)
            {
                ViewBag.RegisterTypeID = rt;

            }
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MeetingID,Subject,ProposerName,PropOrAmend,For,Against,MeetingDate,Resolution,Remark,RegisterTypeID")] Meeting meeting)
        {
          ViewBag.RegisterTypeID = meeting.RegisterTypeID;
          if (ModelState.IsValid)
            {               
                db.Meetings.Add(meeting);
                db.SaveChanges();
                return RedirectToAction("Index",new { rt=meeting.RegisterTypeID});
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", meeting.RegisterTypeID);
            return View(meeting);
        }

        // GET: Meetings/Edit/5
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            ViewBag.RegisterTypeID = meeting.RegisterTypeID;
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MeetingID,Subject,ProposerName,PropOrAmend,For,Against,MeetingDate,Resolution,Remark,RegisterTypeID")] Meeting meeting)
        {
            ViewBag.RegisterTypeID = meeting.RegisterTypeID;
            if (ModelState.IsValid)
            {
                db.Entry(meeting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new {rt=meeting.RegisterTypeID });
            }
          
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meeting meeting = db.Meetings.Find(id);
            db.Meetings.Remove(meeting);
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
