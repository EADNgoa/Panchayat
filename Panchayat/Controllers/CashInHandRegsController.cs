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
    public class CashInHandRegsController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: CashInHandRegs
        public ActionResult Index(int? page,DateTime? md)
        {
            var cih = db.CashInHandRegs.Where(a=>a.CashInHandRegID >0);
            if (md!=null)
            {
                cih = cih.Where(a => a.Tdate == md);
            }
            cih = cih.OrderByDescending(a => a.CashInHandRegID);
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(cih.ToPagedList(pageNumber, pageSize));
        }


        // GET: CashInHandRegs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CashInHandRegs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CashInHandRegID,Tdate,NameAndDesg,CashToDeclareStart,DetailsOfCashExp,CashToDeclareEnd,Remarks")] CashInHandReg cashInHandReg)
        {
          

            if (ModelState.IsValid)
            {
                cashInHandReg.Tdate = DateTime.Now;
                db.CashInHandRegs.Add(cashInHandReg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cashInHandReg);
        }

        // GET: CashInHandRegs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashInHandReg cashInHandReg = db.CashInHandRegs.Find(id);
            if (cashInHandReg == null)
            {
                return HttpNotFound();
            }
            return View(cashInHandReg);
        }

        // POST: CashInHandRegs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CashInHandRegID,Tdate,NameAndDesg,CashToDeclareStart,DetailsOfCashExp,CashToDeclareEnd,Remarks")] CashInHandReg cashInHandReg)
        {
            if (ModelState.IsValid)
            {
                cashInHandReg.Tdate = DateTime.Now;
                db.Entry(cashInHandReg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cashInHandReg);
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
