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
    public class InOutRegsIssuesController : EAController
    {
        // GET: InOutRegsIssues
        public ActionResult Index(int? page,int? rt,DateTime? di,int? IORecieptID)
        {
            
            var inOutRegsIssue = db.InOutRegsIssues.Where(x => x.IOissueID > 0);
            ViewBag.RegisterTypeID = rt;
            ViewBag.IORcpt = IORecieptID ?? 0;
            if (rt != null)
            {               
                inOutRegsIssue = inOutRegsIssue.Where(x => x.RegisterTypeID == rt);
            }
            if (di != null)
            {
                inOutRegsIssue = inOutRegsIssue.Where(x => x.TDate == di);
            }
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(inOutRegsIssue.OrderByDescending(a =>a.TDate).ToList().ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(int rt, int? IORecieptID)
        {
            ViewBag.RegisterTypeID = rt;
            ViewBag.IORcpt = IORecieptID ?? null;
            ViewBag.ItemID = new SelectList(db.InvItems, "ItemID", "Item");
            return View();
        }

        // POST: InOutRegsIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IOissueID,IORecptID,RegisterTypeID,TDate,ItemID,Qty,Value,IssuedTo,Balance,Remarks,RVno")] InOutRegsIssue inOutRegsIssue,int? IORecptID)
        {
          
       
            if (ModelState.IsValid)
            {
                inOutRegsIssue.TDate = DateTime.Now;
                //inOutRegsIssue.Qty = (int)inOutRegsIssue.Value;
           
                db.InOutRegsIssues.Add(inOutRegsIssue);

                db.SaveChanges();

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

                if (inOutRegsIssue.RegisterTypeID == 20)
                {
                    lid = 8;
                    sid = 139;
                }
                int vid = 0;
                if (inOutRegsIssue.RegisterTypeID == 21)
                {
                    lid = 1;
                    sid = 3;
                    var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
                    var item1 = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsIssue.Value, ActualAmount = inOutRegsIssue.Value, For = null, PayDate = inOutRegsIssue.TDate, CBfolio = null, ResNo = null, HeldOn = inOutRegsIssue.TDate, Meeting = "N/A", LedgerID = lid, SubLedgerID = sid};

                    db.Vouchers.Add(item1);
                    db.SaveChanges();
                    vid = item1.VoucherID;
                }
                else
                {
                    //var pn = db.Configs.Select(x => x.VP).FirstOrDefault();
                    //var item1 = new Voucher { PassedBy = UserID, of = pn, Amount = inOutRegsIssue.Value * inOutRegsIssue.Qty, ActualAmount = inOutRegsIssue.Value * inOutRegsIssue.Qty, For = null, PayDate = inOutRegsIssue.TDate, CBfolio = null, ResNo = null, HeldOn = inOutRegsIssue.TDate, Meeting = "N/A", LedgerID = lid, SubLedgerID = sid};

                    //db.Vouchers.Add(item1);
                    db.SaveChanges();
                    //vid = item1.VoucherID;
                }
                //inOutRegsIssue.RVno =vid;
             
             
                //Update the Inventory table Qtys
                var item = db.Inventories.Where(x => x.ItemID == inOutRegsIssue.ItemID).FirstOrDefault();
                if (item != null)
                {
                    
                    var rcp = inOutRegsIssue.IORecptID;
                    var rcptBal = db.InOutRegsRecpts.Find(rcp);
                    
                    if (inOutRegsIssue.RegisterTypeID == 21)
                    {
                       
                        db.Entry(item).Property(a => a.Qty).IsModified = true;
                        inOutRegsIssue.Balance =rcptBal.Qty - inOutRegsIssue.Value;
                        rcptBal.Qty =(int) inOutRegsIssue.Balance;
                        db.Entry(rcptBal).Property(a => a.Qty).IsModified = true;

                        db.SaveChanges();

                    }
                    else
                    {
                        item.Qty = item.Qty - (int)inOutRegsIssue.Qty;
                        inOutRegsIssue.Balance = item.Qty;
                        db.Entry(item).Property(a => a.Qty).IsModified = true;
                        db.SaveChanges();
                    }
             
                    
                  
                }
                if (inOutRegsIssue.RegisterTypeID == 21)
                {
                    return RedirectToAction("Index", "InOutRegsRecpts", new { rt = inOutRegsIssue.RegisterTypeID });
                }
                else
                {
                    return RedirectToAction( "Index", new { rt = inOutRegsIssue.RegisterTypeID });

                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
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
                //int id = (int)inOutRegsIssue.RVno;
                //var voucher = db.Vouchers.Find(id);
                if (inOutRegsIssue.RegisterTypeID ==21)
                {
                    db.Entry(inOutRegsIssue).State = EntityState.Modified;
                  
                    //voucher.Amount =  inOutRegsIssue.Value;
                    //db.Entry(voucher).Property(a => a.Amount).IsModified = true;

                    //voucher.ActualAmount =  inOutRegsIssue.Value;
                    //db.Entry(voucher).Property(a => a.ActualAmount).IsModified = true;
                    db.SaveChanges();
                }
                else
                {

                    //voucher.Amount = inOutRegsIssue.Qty * inOutRegsIssue.Value;
                    //db.Entry(voucher).Property(a => a.Amount).IsModified = true;

                    //voucher.ActualAmount = inOutRegsIssue.Qty * inOutRegsIssue.Value;
                    //db.Entry(voucher).Property(a => a.ActualAmount).IsModified = true;

                    var item = db.Inventories.Where(x => x.ItemID == inOutRegsIssue.ItemID).FirstOrDefault();
                    if (item != null)
                    {
                        db.Entry(inOutRegsIssue).State = EntityState.Modified;
                      
                        item.Qty = item.Qty + (int)OldQty;
                        item.Qty = item.Qty - (int)inOutRegsIssue.Qty;
                        inOutRegsIssue.Balance = item.Qty;
                        inOutRegsIssue.IORecptID = null;
                        db.Entry(item).Property(a => a.Qty).IsModified = true;
                        db.SaveChanges();
                    }


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
