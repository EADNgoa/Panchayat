using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Panchayat.Controllers
{
    [Authorize(Roles = "Boss,Type1")]
    public class RentController : EAController
    {

        // GET: Form4
        public ActionResult Index(int? page, int? SearchRecieptNo, string SearchCitizen, DateTime? tdate, DateTime? fdate, decimal? fAmount, decimal? tAmount)
        {


            var form4 = db.Form4.Where(f => f.SubLedgerID ==30);

            if (SearchRecieptNo != null)
            {
                form4 = form4.Where(f => f.RecieptNo == SearchRecieptNo);
            }
            if (SearchCitizen?.Length > 0)
            {
                form4 = form4.Where(f => f.RecvdFrom.Contains(SearchCitizen) || f.Citizen.Name.Contains(SearchCitizen));
            }
            if (fdate != null && tdate != null)
            {
                form4 = form4.Where(t => t.PayDate >= fdate && t.PayDate <= tdate);
            }
            if (fAmount != null && tAmount != null)
            {
                form4 = form4.Where(t => t.Amount >= fAmount && t.Amount <= tAmount);
            }

            form4 = form4.OrderByDescending(e => e.PayDate);
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(form4.ToPagedList(pageNumber, pageSize));
        }

        // GET: Form4/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form4 form4 = db.Form4.Find(id);
            if (form4 == null)
            {
                return HttpNotFound();
            }

            ViewBag.vpname = db.Configs.Find(1).VP;

            var cnts = new ChangeNumbersToWords();
            ViewBag.AmtInWds = cnts.changeToWords(form4.Amount.ToString());

            return View(form4);
        }

        // GET: Form4/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Form4/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecieptNo,CitizenID,Amount,SubLedgerID,RecvdFrom,HouseNo")] Form4 form4, FormCollection fm)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        form4.SubLedgerID = 30;

                        form4.LedgerID = db.SubLedgers.Find(form4.SubLedgerID).LedgerID;
                                               

                        
                        form4.PayDate = DateTime.Today;
                        db.Form4.Add(form4);
                        db.SaveChanges();
                        decimal currentAmt = Decimal.Parse(fm["data-90"]);

                        var Pdate = form4.PayDate.Value.AddMonths(-1).Month ;
                        var PrevArre = db.Form4.FirstOrDefault(a => a.CitizenID == form4.CitizenID && a.PayDate.Value.Month == Pdate );
                        decimal PrevArreVal = 0;

                        if (PrevArre != null)
                            Decimal.TryParse(db.RVdetails.FirstOrDefault(r => r.ReceiptNo == PrevArre.RecieptNo && r.LedgerDetailID == 89).RVDetail1, out PrevArreVal );
                        
                        decimal ThisMonthsbal = PrevArreVal + currentAmt - (decimal)form4.Amount;

                        db.RVdetails.Add(new RVdetail { ReceiptNo = form4.RecieptNo, LedgerDetailID = 89, RVDetail1 = ThisMonthsbal.ToString() });

                        //Update our Budget
                        int budYr = ((DateTime)form4.PayDate).Month > 3 ? ((DateTime)form4.PayDate).Year : ((DateTime)form4.PayDate).Year - 1;
                        var bud = db.Budgets.FirstOrDefault(b => b.SubLedgerID == form4.SubLedgerID && b.BudgtFY == budYr);
                        bud.ActualAmount += form4.Amount ?? 0;
                        db.Entry(bud).State = EntityState.Modified;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                      throw ex;
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.CitizenID = new SelectList(db.Citizens, "CitizenID", "Name", form4.CitizenID);

            return View(form4);
        }

        // GET: Form4/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form4 form4 = db.Form4.Find(id);
            if (form4 == null)
            {
                return HttpNotFound();
            }

            return View(form4);
        }

        /// <summary>
        /// Using a Partial view so that the CLD can be in the same entry form as the Demands
        /// </summary>
        /// <param name="id">Chosen Subledger ID from Autocomplete box</param>        
        /// <returns></returns>        
        public ActionResult EditRVLedgDetsPartial(int id)
        {
            var CLedgDets = db.RVdetails.Where(rv => rv.ReceiptNo == id).ToList();
            return PartialView("_EditRVLedgDetsPartial", CLedgDets);

        }

        public ActionResult AutoCompleteHouseNo(string term, int? Cit)
        {
            var h = (Cit.HasValue) ? db.Demands.Where(d => d.CitizenID == Cit && d.HouseNo.Contains(term)).Select(c => new { id = c.HouseNo, value = c.HouseNo }) : db.Demands.Where(d => d.HouseNo.Contains(term)).Select(c => new { id = c.HouseNo, value = c.HouseNo });

            return Json(h, JsonRequestBehavior.AllowGet);
        }

        // POST: Form4/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecieptNo,CitizenID,Amount,PayDate,LedgerID, SubLedgerID,RecvdFrom,HouseNo")] Form4 form4, FormCollection fm)
        {
            if (ModelState.IsValid && form4.PayDate == DateTime.Today)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        form4.HouseNo = form4.HouseNo?.ToUpper().Trim();
                        db.Entry(form4).State = EntityState.Modified;


                        foreach (var item in fm)
                        {
                            if (item.ToString().Length > 5 && item.ToString().Substring(0, 5) == "data-")
                            {
                                int iLedDetID = int.Parse(item.ToString().Substring(item.ToString().LastIndexOf('-') + 1));

                                var cld = db.RVdetails.Find(iLedDetID);
                                cld.RVDetail1 = fm[item.ToString()];

                                db.Entry(cld).State = EntityState.Modified;
                            }
                        }

                        //Update the Budget
                        int budYr = ((DateTime)form4.PayDate).Month > 3 ? ((DateTime)form4.PayDate).Year : ((DateTime)form4.PayDate).Year - 1;
                        var bud = db.Budgets.FirstOrDefault(b => b.SubLedgerID == form4.SubLedgerID && b.BudgtFY == budYr);

                        var CurrAmt = db.Entry(form4).CurrentValues.Clone();
                        db.Entry(form4).Reload();
                        bud.ActualAmount -= form4.Amount ?? 0; //minus the old value
                        bud.ActualAmount += (decimal)CurrAmt["Amount"]; //add the new value
                        db.Entry(form4).CurrentValues.SetValues(CurrAmt);
                        db.Entry(form4).State = EntityState.Modified;
                        db.Entry(bud).State = EntityState.Modified;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(form4);
        }

        //// GET: Form4/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Form4 form4 = db.Form4.Find(id);
        //    if (form4 == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(form4);
        //}

        //// POST: Form4/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Form4 form4 = db.Form4.Find(id);
        //    db.Form4.Remove(form4);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


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
