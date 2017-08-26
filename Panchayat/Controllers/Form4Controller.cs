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
    public class Form4Controller : EAController
    {
       
        // GET: Form4
        public ActionResult Index(int? page ,int? SearchRecieptNo,string SearchCitizen ,DateTime? tdate , DateTime? fdate,decimal? fAmount, decimal? tAmount)
        {

           
            var form4 = db.Form4.Where(f=>f.RecieptNo>0);
          
            if (SearchRecieptNo != null)
            {
                form4 = form4.Where(f => f.RecieptNo == SearchRecieptNo);
            }
            if (SearchCitizen?.Length>0)
            {
                form4 = form4.Where(f => f.RecvdFrom.Contains(SearchCitizen) || f.Citizen.Name.Contains(SearchCitizen));
            }
            if(fdate!= null && tdate!=null)
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
                form4.LedgerID = db.SubLedgers.Find(form4.SubLedgerID).LedgerID;

                //if a citizen was entered by mistake and then a non citizen was entered ignore the citizen
                if (form4.RecvdFrom != null)
                    form4.CitizenID = null;

                form4.HouseNo = form4.HouseNo?.ToUpper().Trim();
                form4.PayDate = DateTime.Today;
                db.Form4.Add(form4);


                //For receipts for subledgers that do not have demand records we need to store the LedgerDetails at this point
                foreach (var item in fm)
                {
                    if (item.ToString().Length > 5 && item.ToString().Substring(0, 5) == "data-")
                    {
                        int iLedDetID = int.Parse(item.ToString().Substring(item.ToString().LastIndexOf('-') + 1));

                        //Prevent overposting attack by checking that only valid LedgerDetails for the given subledger are saved
                        if (db.LedgerDetails.Any(l => l.LedgerDetailID == iLedDetID && l.SubLedgerID == form4.SubLedgerID))
                            db.RVdetails.Add(new RVdetail { LedgerDetailID = iLedDetID, RVDetail1 = fm[item.ToString()], ReceiptNo = form4.RecieptNo });
                    }
                }

                db.SaveChanges();
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
            if (ModelState.IsValid && form4.PayDate==DateTime.Today)
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



                        db.SaveChanges();
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
