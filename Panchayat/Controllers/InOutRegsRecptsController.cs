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
        public ActionResult Index(int? page, int? rt,DateTime? dr, int? IORecieptID)
        {
            ViewBag.RegisterTypeID =rt;
            ViewBag.IORcpt = IORecieptID ?? null;
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
           
            if (ModelState.IsValid)
            {
           
                inOutRegsRecpt.TDate = DateTime.Now;
                db.SaveChanges();
                int lid = 0, sid = 0;
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

                int vid = 0;
                if (inOutRegsRecpt.RegisterTypeID == 21)
                {
                    lid = 8;
                    sid = 139;
                    var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
                    var item = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsRecpt.Value, ActualAmount = inOutRegsRecpt.Value, For = null, PayDate = inOutRegsRecpt.TDate, CBfolio = null, ResNo = null, HeldOn = inOutRegsRecpt.TDate, Meeting = "N/A", LedgerID = lid, SubLedgerID = sid, Form6 = false };
                    db.Vouchers.Add(item);
                    db.SaveChanges();
                    vid = item.VoucherID;
                }
                else
                {

                    var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
                    var item = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsRecpt.Value * inOutRegsRecpt.Qty, ActualAmount = inOutRegsRecpt.Value * inOutRegsRecpt.Qty, For = null, PayDate = inOutRegsRecpt.TDate, CBfolio = null, ResNo = null, HeldOn = inOutRegsRecpt.TDate, Meeting = "N/A", LedgerID = lid, SubLedgerID = sid, Form6 = false };
                    db.Vouchers.Add(item);
                    db.SaveChanges();
                    vid = item.VoucherID;
                }
                inOutRegsRecpt.RVno = vid;
                db.InOutRegsRecpts.Add(inOutRegsRecpt);           
                db.SaveChanges();
                var item1 = db.Inventories.Where(x => x.ItemID == inOutRegsRecpt.ItemID).FirstOrDefault();
                if (item1 != null)
                {
                    if (inOutRegsRecpt.RegisterTypeID == 21)
                    {
                       
                        item1.Qty = item1.Qty + (int)inOutRegsRecpt.Value;
                        db.Entry(item1).Property(a => a.Qty).IsModified = true;
                        inOutRegsRecpt.Qty = (int)inOutRegsRecpt.Value;

                        db.SaveChanges();
                    }
                    else
                    {
                    
                        item1.Qty = item1.Qty + (int)inOutRegsRecpt.Qty;
                        db.Entry(item1).Property(a => a.Qty).IsModified = true;
                        db.SaveChanges();
                    }
                 
                }
                else
                {
                    if (inOutRegsRecpt.RegisterTypeID != 21)
                    {
                
                        var item2 = new Inventory { ItemID = (int)inOutRegsRecpt.ItemID, Qty = (int)inOutRegsRecpt.Qty };
                        db.Inventories.Add(item2);
                    }
                    else
                    {
                        inOutRegsRecpt.Qty =(int) inOutRegsRecpt.Value;
                        var item2 = new Inventory { ItemID = (int)inOutRegsRecpt.ItemID, Qty = (int)inOutRegsRecpt.Value };
                        db.Inventories.Add(item2);
                    }
                   
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
                if(inOutRegsRecpt.RegisterTypeID==21)
                {
                    db.Entry(inOutRegsRecpt).State = EntityState.Modified;
                    voucher.Amount =  inOutRegsRecpt.Value;
                    db.Entry(voucher).Property(a => a.Amount).IsModified = true;

                    voucher.ActualAmount = inOutRegsRecpt.Value;
                    db.Entry(voucher).Property(a => a.ActualAmount).IsModified = true;
                    db.SaveChanges();
            
                }
                else
                {
                    voucher.Amount = inOutRegsRecpt.Qty * inOutRegsRecpt.Value;
                    db.Entry(voucher).Property(a => a.Amount).IsModified = true;

                    voucher.ActualAmount = inOutRegsRecpt.Qty * inOutRegsRecpt.Value;
                    db.Entry(voucher).Property(a => a.ActualAmount).IsModified = true;

                    var item = db.Inventories.Where(x => x.ItemID == inOutRegsRecpt.ItemID).FirstOrDefault();
                    if (item != null)
                    {
                        db.Entry(inOutRegsRecpt).State = EntityState.Modified;
                        item.Qty = item.Qty - (int)OldQty;
                        item.Qty = item.Qty + (int)inOutRegsRecpt.Qty;

                        db.Entry(item).Property(a => a.Qty).IsModified = true;
                        inOutRegsRecpt.Qty = (int)inOutRegsRecpt.Value;
                        db.SaveChanges();
                    }
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
