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
    [Authorize(Roles = "Boss,Type1")]
    public class BudgetsController : EAController
    {
        // GET: Budgets                
        public ActionResult Index(int? BudFYr)
        {
            BudFYr = BudFYr ?? DateTime.Now.Year;
            ViewBag.YearBox = MyExtensions.MakeYrRq(5, 1, DateTime.Today.Year);
            var bud = db.Budgets.Where(b => b.BudgtFY == BudFYr).ToList();
            if (bud.Count == 0)
            {
                var sl = db.SubLedgers.ToList();

                foreach (var item in sl)
                {
                    db.Budgets.Add(new Budget { BudgtFY = BudFYr, BudgetAmount = 0, SubLedgerID = item.SubLedgerID });
                }
                db.SaveChanges();
                bud = db.Budgets.Where(b => b.BudgtFY == BudFYr).ToList();
                //bud = db.SubLedgers.Select(a => new { BudgtFY = BudFYr, BudgetAmount = 0, SubLedgerID = a.SubLedgerID }).ToList()
                //    .Select(x => new Budget() { BudgtFY = x.BudgtFY, BudgetAmount = x.BudgetAmount, SubLedgerID = x.SubLedgerID }).ToList();
            }
            return View(bud);
        }


       
        

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", budget.SubLedgerID);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BudgetID,BudgtFY,SubLedgerID,BudgetAmount,ActualAmount,OpeningBalance")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubLedgerID = new SelectList(db.SubLedgers, "SubLedgerID", "Ledger", budget.SubLedgerID);
            return View(budget);
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
