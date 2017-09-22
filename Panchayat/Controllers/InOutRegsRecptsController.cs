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
using PagedList;

namespace Panchayat.Controllers
{
    [Authorize(Roles = "Boss,Type1")]
    public class InOutRegsRecptsController : EAController
    {
        // GET: InOutRegsRecpts
        public ActionResult Index(int? page, int? rt,DateTime? dr)
        {
            ViewBag.RegisterTypeID =rt;
            var inOutRegsRecpts = db.InOutRegsRecpts.Where(x=>x.IORecptID>0);
            if (rt != null)
            {
                inOutRegsRecpts = inOutRegsRecpts.Where(x => x.RegisterTypeID == rt);
            }
            if (dr != null)
            {

                inOutRegsRecpts = inOutRegsRecpts.Where(x => x.TDate == dr);
            }
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(inOutRegsRecpts.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: InOutRegsRecpts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsRecpt inOutRegsRecpt = db.InOutRegsRecpts.Find(id);
            if (inOutRegsRecpt == null)
            {
                return HttpNotFound();
            }
            return View(inOutRegsRecpt);
        }

        // GET: InOutRegsRecpts/Create
        public ActionResult Create(int? rt)
        {
            ViewBag.RegisterTypeID = rt;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item");

            return View();
        }

        // POST: InOutRegsRecpts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,RVno")] InOutRegsRecpt inOutRegsRecpt)
        {
            int lid=0, sid=0;
            string UserID = User.Identity.GetUserName();

            if (inOutRegsRecpt.RegisterTypeID == 17)
            {
                lid = 8;
                sid = 42;
            }
            if (inOutRegsRecpt.RegisterTypeID == 18)
            {
                 lid = 8;
                 sid = 137;
            }
            if (inOutRegsRecpt.RegisterTypeID == 19)
            {
                 lid = 8;
                 sid = 138;
            }
            if (inOutRegsRecpt.RegisterTypeID == 20)
            {
                lid = 8;
                sid = 139;
            }
            var pn = db.Configs.Select(x=>x.VP).FirstOrDefault();
            var item = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsRecpt.Value * inOutRegsRecpt.Qty, ActualAmount = inOutRegsRecpt.Value * inOutRegsRecpt.Qty, For =null,PayDate=inOutRegsRecpt.TDate,CBfolio = null,ResNo= null,HeldOn= inOutRegsRecpt.TDate,Meeting="N/A",LedgerID = lid,SubLedgerID=sid,Form6=false};
            db.Vouchers.Add(item);
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                inOutRegsRecpt.RVno = item.VoucherID;
                db.InOutRegsRecpts.Add(inOutRegsRecpt);           
                db.SaveChanges();
                var itemID = db.Inventories.Where(x => x.ItemID == inOutRegsRecpt.ItemID).Select(x=>x.ItemID).FirstOrDefault();
                if (itemID != 0)
                {
                    var ExQty = db.Inventories.Find(itemID);
                    ExQty.Qty = ExQty.Qty+(int)inOutRegsRecpt.Qty;
                    db.Entry(ExQty).Property(a => a.Qty).IsModified = true;
                    db.SaveChanges();
                }
                else
                {
                    var item2 = new Inventory {ItemID = (int)inOutRegsRecpt.ItemID ,Qty =(int)inOutRegsRecpt.Qty };
                    db.Inventories.Add(item2);
                    db.SaveChanges();
                }

                return RedirectToAction("Index",new {rt= inOutRegsRecpt.RegisterTypeID });
            }

         
            ViewBag.RegisterTypeID = inOutRegsRecpt.RegisterTypeID;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item", inOutRegsRecpt.ItemID);
            return View(inOutRegsRecpt);
        }

        // GET: InOutRegsRecpts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsRecpt inOutRegsRecpt = db.InOutRegsRecpts.Find(id);
           
           
            if (inOutRegsRecpt == null)
            {
                return HttpNotFound();
            }
            ViewBag.OldQty = (int)inOutRegsRecpt.Qty;
            ViewBag.RegisterTypeID = inOutRegsRecpt.RegisterTypeID;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item", inOutRegsRecpt.ItemID);
         
            return View(inOutRegsRecpt);
        }

        // POST: InOutRegsRecpts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,RVno")] InOutRegsRecpt inOutRegsRecpt,int? OldQty )
        {
           

            if (ModelState.IsValid)
            {
                int id = (int)inOutRegsRecpt.RVno;
                var voucher = db.Vouchers.Find(id);
                voucher.Amount = inOutRegsRecpt.Qty * inOutRegsRecpt.Value;
                db.Entry(voucher).Property(a => a.Amount).IsModified = true;

                voucher.ActualAmount = inOutRegsRecpt.Qty * inOutRegsRecpt.Value;
                db.Entry(voucher).Property(a => a.ActualAmount).IsModified = true;



                var itemID = db.Inventories.Where(x => x.ItemID == inOutRegsRecpt.ItemID).Select(x => x.ItemID).FirstOrDefault();
                if (itemID != 0)
                {
                    db.Entry(inOutRegsRecpt).State = EntityState.Modified;
                    db.SaveChanges();
                    var ExQty = db.Inventories.Find(itemID);
                    ExQty.Qty = ExQty.Qty - (int)OldQty;
                    ExQty.Qty = ExQty.Qty +(int) inOutRegsRecpt.Qty;

                    db.Entry(ExQty).Property(a => a.Qty).IsModified = true;
                    db.SaveChanges();
                }
              

                return RedirectToAction("Index", new { rt = inOutRegsRecpt.RegisterTypeID });
            }
            ViewBag.RegisterTypeID = inOutRegsRecpt.RegisterTypeID;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item", inOutRegsRecpt.ItemID);
            return View(inOutRegsRecpt);
        }

        // GET: InOutRegsRecpts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOutRegsRecpt inOutRegsRecpt = db.InOutRegsRecpts.Find(id);
            if (inOutRegsRecpt == null)
            {
                return HttpNotFound();
            }
            return View(inOutRegsRecpt);
        }

        // POST: InOutRegsRecpts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InOutRegsRecpt inOutRegsRecpt = db.InOutRegsRecpts.Find(id);
            db.InOutRegsRecpts.Remove(inOutRegsRecpt);
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
