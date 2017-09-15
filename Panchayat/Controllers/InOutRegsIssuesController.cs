using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Panchayat;
using Microsoft.AspNet.Identity;

namespace Panchayat.Controllers
{
    public class InOutRegsIssuesController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: InOutRegsIssues
        public ActionResult Index(int? page,int? rt)
        {
            
            var inOutRegsIssue = db.InOutRegsIssues.Where(x => x.IOissueID > 0);
            ViewBag.RegisterTypeID = rt;
            if (rt != null)
            {
               
                inOutRegsIssue = inOutRegsIssue.Where(x => x.RegisterTypeID == rt);
            }
            return View(inOutRegsIssue.ToList());
        }

        // GET: InOutRegsIssues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsIssue inOutRegsIssue = db.InOutRegsIssues.Find(id);
            if (inOutRegsIssue == null)
            {
                return HttpNotFound();
            }
            return View(inOutRegsIssue);
        }

        // GET: InOutRegsIssues/Create
        public ActionResult Create(int rt)
        {
            ViewBag.RegisterTypeID = rt;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item");
            return View();
        }

        // POST: InOutRegsIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IOissueID,IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,IssuedTo,Balance,Remarks,RVno")] InOutRegsIssue inOutRegsIssue)
        {
            int lid = 0, sid = 0;
            string UserID = User.Identity.GetUserName();

            if (inOutRegsIssue.RegisterTypeID == 17)
            {
                lid = 8;
                sid = 42;
            }
            if (inOutRegsIssue.RegisterTypeID == 18)
            {
                lid = 8;
                sid = 137;
            }
            if (inOutRegsIssue.RegisterTypeID == 19)
            {
                lid = 8;
                sid = 138;
            }

            var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
            var item = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsIssue.Value * inOutRegsIssue.Qty, ActualAmount = inOutRegsIssue.Value * inOutRegsIssue.Qty, For = null, PayDate = inOutRegsIssue.TDate, CBfolio = null, ResNo = null, HeldOn = inOutRegsIssue.TDate, Meeting = "N/A", LedgerID = lid, SubLedgerID = sid, Form6 = false };
            db.Vouchers.Add(item);
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                inOutRegsIssue.RVno = item.VoucherID;
                db.InOutRegsIssues.Add(inOutRegsIssue);
                db.SaveChanges();
                var itemID = db.Inventories.Where(x => x.ItemID == inOutRegsIssue.ItemID).Select(x => x.ItemID).FirstOrDefault();
                if (itemID != 0)
                {
                    var ExQty = db.Inventories.Find(itemID);
                    ExQty.Qty = ExQty.Qty - (int)inOutRegsIssue.Qty;
                    inOutRegsIssue.Balance = ExQty.Qty;
                    inOutRegsIssue.IORecptID = null;
                    db.Entry(ExQty).Property(a => a.Qty).IsModified = true;
                    db.SaveChanges();
                }
               
                return RedirectToAction("Index", new { rt = inOutRegsIssue.RegisterTypeID });
               
            }

            ViewBag.RegisterTypeID = inOutRegsIssue.RegisterTypeID;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item", inOutRegsIssue.ItemID);
            return View(inOutRegsIssue);
        }

        // GET: InOutRegsIssues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsIssue inOutRegsIssue = db.InOutRegsIssues.Find(id);

            if (inOutRegsIssue == null)
            {
                return HttpNotFound();
            }
            ViewBag.OldQty = (int)inOutRegsIssue.Qty;
            ViewBag.RegisterTypeID = inOutRegsIssue.RegisterTypeID;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item", inOutRegsIssue.ItemID);
            return View(inOutRegsIssue);
        }

        // POST: InOutRegsIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IOissueID,IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,IssuedTo,Balance,Remarks,RVno")] InOutRegsIssue inOutRegsIssue,int? OldQty)
        {
            if (ModelState.IsValid)
            {
                int id = (int)inOutRegsIssue.RVno;
                var voucher = db.Vouchers.Find(id);
                voucher.Amount = inOutRegsIssue.Qty * inOutRegsIssue.Value;
                db.Entry(voucher).Property(a => a.Amount).IsModified = true;

                voucher.ActualAmount = inOutRegsIssue.Qty * inOutRegsIssue.Value;
                db.Entry(voucher).Property(a => a.ActualAmount).IsModified = true;



                var itemID = db.Inventories.Where(x => x.ItemID == inOutRegsIssue.ItemID).Select(x => x.ItemID).FirstOrDefault();
                if (itemID != 0)
                {
                    db.Entry(inOutRegsIssue).State = EntityState.Modified;
                    db.SaveChanges();
                    var ExQty = db.Inventories.Find(itemID);
                    ExQty.Qty = ExQty.Qty + (int)OldQty;
                    ExQty.Qty = ExQty.Qty - (int)inOutRegsIssue.Qty;
                    inOutRegsIssue.Balance = ExQty.Qty;
                    inOutRegsIssue.IORecptID = null;
                    db.Entry(ExQty).Property(a => a.Qty).IsModified = true;
                    db.SaveChanges();
                }


                return RedirectToAction("Index", new { rt = inOutRegsIssue.RegisterTypeID });
              
            }
            ViewBag.RegisterTypeID = inOutRegsIssue.RegisterTypeID;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item", inOutRegsIssue.ItemID);
            return View(inOutRegsIssue);
        }

        // GET: InOutRegsIssues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsIssue inOutRegsIssue = db.InOutRegsIssues.Find(id);
            if (inOutRegsIssue == null)
            {
                return HttpNotFound();
            }
            return View(inOutRegsIssue);
        }

        // POST: InOutRegsIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InOutRegsIssue inOutRegsIssue = db.InOutRegsIssues.Find(id);
            db.InOutRegsIssues.Remove(inOutRegsIssue);
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
