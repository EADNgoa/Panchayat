using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Panchayat;

namespace Panchayat.Controllers
{
    public class WorksController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: Works
        public ActionResult Index(int? page, string PropName, int? yr)
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(5, 1, DateTime.Now.Year);
            yr = MyExtensions.GetFinYr(DateTime.Parse("1 April" + yr ?? DateTime.Now.Year.ToString()));
            var wrks = db.Works.Where(r => ((DateTime)r.CommenceDate).Year == yr).ToList();

            if (PropName?.Length > 0)
            {//Contractor Name
                wrks = wrks.Where(r => r.ContractorName.Contains(PropName)).ToList();
                page = 1;
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View("Index", wrks.OrderBy(r => r.ContractorName).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Works/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // GET: Works/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Works/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorksID,NameOfWork,TechSanctionNo,ContractorName,PercentageAccepted,EMDIOrecptID,SDIOrecptID,ITrecptNo,RoyaltyDeposit,TimeLimit,ExtentionTime,EstimatedCost,TenderedCost,FinalValue,NetPaymentToContractor,CommenceDate,CompletionDate,GrantsRecieved,GrantsUtilized,MBNo")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Works.Add(work);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                        
            return View(work);
        }

        // GET: Works/Edit/5
        public ActionResult Edit(int? id, int? IOrID, int? rt)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            if (IOrID != null)
            {
                if (rt == 24) work.EMDIOrecptID = IOrID;
                if (rt == 23) work.SDIOrecptID=IOrID;
                if (rt == 27) work.ITrecptNo = IOrID;
                if (rt == 28) work.RoyaltyDeposit = IOrID;
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.EMDrefund = (work.EMDIOrecptID !=null)? db.InOutRegsRecpts.Find(work.EMDIOrecptID).InOutRegsIssues.FirstOrDefault()?.Value.ToString():" Pending";
            ViewBag.SDrefund = (work.SDIOrecptID != null) ? db.InOutRegsRecpts.Find(work.SDIOrecptID).InOutRegsIssues.FirstOrDefault()?.Value.ToString():" Pending";
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorksID,NameOfWork,TechSanctionNo,ContractorName,PercentageAccepted,EMDIOrecptID,SDIOrecptID,ITrecptNo,RoyaltyDeposit,TimeLimit,ExtentionTime,EstimatedCost,TenderedCost,FinalValue,NetPaymentToContractor,CommenceDate,CompletionDate,GrantsRecieved,GrantsUtilized,MBNo")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMDrefund = (work.EMDIOrecptID != null) ? db.InOutRegsRecpts.Find(work.EMDIOrecptID).InOutRegsIssues.FirstOrDefault()?.Value.ToString():"";
            ViewBag.SDrefund = (work.SDIOrecptID != null) ? db.InOutRegsRecpts.Find(work.SDIOrecptID).InOutRegsIssues.FirstOrDefault()?.Value.ToString():"";
            return View(work);
        }

        // GET: Works/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Works.Find(id);
            db.Works.Remove(work);
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
