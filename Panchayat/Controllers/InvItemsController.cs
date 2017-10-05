using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Panchayat;

namespace Panchayat.Controllers
{
    public class InvItemsController : Controller
    {
        private PanchayatEntities db = new PanchayatEntities();

        // GET: InvItems
        public ActionResult Index()
        {
            var invItems = db.InvItems.Include(i => i.RegisterType).Include(i => i.Inventory);
            return View(invItems.ToList());
        }

        // GET: InvItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItem invItem = db.InvItems.Find(id);
            if (invItem == null)
            {
                return HttpNotFound();
            }
            return View(invItem);
        }

        // GET: InvItems/Create
        public ActionResult Create()
        {
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1");
            ViewBag.ItemID = new SelectList(db.Inventories, "ItemID", "ItemID");
            return View();
        }

        // POST: InvItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,Item,RegisterTypeID")] InvItem invItem)
        {
            if (ModelState.IsValid)
            {
                db.InvItems.Add(invItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", invItem.RegisterTypeID);
            ViewBag.ItemID = new SelectList(db.Inventories, "ItemID", "ItemID", invItem.ItemID);
            return View(invItem);
        }

        // GET: InvItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItem invItem = db.InvItems.Find(id);
            if (invItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", invItem.RegisterTypeID);
            ViewBag.ItemID = new SelectList(db.Inventories, "ItemID", "ItemID", invItem.ItemID);
            return View(invItem);
        }

        // POST: InvItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,Item,RegisterTypeID")] InvItem invItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegisterTypeID = new SelectList(db.RegisterTypes, "RegisterTypeID", "RegisterType1", invItem.RegisterTypeID);
            ViewBag.ItemID = new SelectList(db.Inventories, "ItemID", "ItemID", invItem.ItemID);
            return View(invItem);
        }

        // GET: InvItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItem invItem = db.InvItems.Find(id);
            if (invItem == null)
            {
                return HttpNotFound();
            }
            return View(invItem);
        }

        // POST: InvItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvItem invItem = db.InvItems.Find(id);
            db.InvItems.Remove(invItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AutoCompleteInvItems(string term, int rt)
        {
            var h = db.InvItems.Where(d => d.RegisterTypeID == rt && d.Item.Contains(term)).Select(c => new { id = c.ItemID, value = c.Item });
            return Json(h, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _CurrInv(int id)
        {
            var ci = db.Inventories.FirstOrDefault(i => i.ItemID == id).Qty;
            return PartialView("_CurrInv", "The current inventory quantity is: " + ci.ToString());
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
