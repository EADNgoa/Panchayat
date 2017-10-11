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
    public class VPRentsController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: VPRents
        public ActionResult Index(int? page,  string PropName, int? yr)
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(5, 1, DateTime.Now.Year);
            yr = MyExtensions.GetFinYr(DateTime.Parse("1 April" + yr ?? DateTime.Now.Year.ToString()));
            ViewBag.yr = yr;
            var vpRents = db.VPRents.Where(i => i.RentYear == yr);

            if (PropName?.Length > 0 )
            {
                vpRents = vpRents.Where(r => r.RentPayerName.Contains(PropName));
                page = 1;
            }

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View("Index", vpRents.OrderBy(r => r.RentPayerName).ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult IndexDet(int yr)
        {
            return View(db.VPRentDetails.ToList());
        }

        // GET: VPRents/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VPRent vPRent = db.VPRents.Find(id);
            if (vPRent == null)
            {
                return HttpNotFound();
            }
            ViewBag.vpRent = vPRent;

            ViewBag.rd = db.VPRentDetails.Where(r => r.VPRentID == id).OrderBy(r=>r.Month);
            ViewBag.MonthBox = MyExtensions.MonthList();
            return View();
        }

        // GET: VPRents/Create
        public ActionResult Create()
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(8, 1, DateTime.Today.Year);
            return View();
        }

        // POST: VPRents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VPRentID,RentYear,RentPayerName,RentedPropertyName")] VPRent vPRent)
        {
            if (ModelState.IsValid)
            {
                db.VPRents.Add(vPRent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vPRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDet([Bind(Include = "VPRentID,Month,Arrears,Current,RecoveryAmt,RecoveryDate,BalanceArrears,BalanceCurrent")] VPRentDetail vPRentDetail)
        {
            if (ModelState.IsValid)
            {
                db.VPRentDetails.Add(vPRentDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            VPRent vPRent = db.VPRents.Find(vPRentDetail.VPRentID);
            if (vPRent == null)
            {
                return HttpNotFound();
            }
            ViewBag.vpRent = vPRent;

            ViewBag.rd = db.VPRentDetails.Where(r => r.VPRentID == vPRentDetail.VPRentID).OrderBy(r => r.Month);
            ViewBag.MonthBox = MyExtensions.MonthList();
            return View("Details", vPRentDetail);
        }


        // GET: VPRents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VPRent vPRent = db.VPRents.Find(id);
            if (vPRent == null)
            {
                return HttpNotFound();
            }
            ViewBag.YearBox = MyExtensions.MakeYrRq(8, 1, DateTime.Today.Year);
            return View(vPRent);
        }

        // POST: VPRents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VPRentID,RentYear,RentPayerName,RentedPropertyName")] VPRent vPRent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vPRent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.YearBox = MyExtensions.MakeYrRq(8, 1, DateTime.Today.Year);
            return View(vPRent);
        }


        public ActionResult EditDet(int VPRentID, int dMonth)
        {
            var vPRentDet = db.VPRentDetails.FirstOrDefault(rd => rd.VPRentID == VPRentID && rd.Month == dMonth);
            if (vPRentDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.MonthBox = MyExtensions.MonthList();
            return View("EditDet",vPRentDet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDet([Bind(Include = "VPRentID,Month,Arrears,Current,RecoveryAmt,RecoveryDate,BalanceArrears,BalanceCurrent")] VPRentDetail vPRentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vPRentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        
            return View(vPRentDetail);
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
