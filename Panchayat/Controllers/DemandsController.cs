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
    public class DemandsController : EAController
    {
       
        // GET: Demands/Create
        public ActionResult Create(int? id)
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(1, 1, DateTime.Today.Year);

            //Depending on where this is called from, set the Citizen
            int CitID = id ?? 0;

            ViewBag.CitName = (CitID > 0) ? db.Citizens.Find(CitID).Name : "";
            DemandInitLoad dil = new DemandInitLoad { CitizenID = CitID };
            ViewBag.PeriodID = new SelectList(db.PeriodSLs, "PeriodID", "PeriodSL1");
            return View(dil);
        }

        // POST: Demands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DemandID,CitizenID,PeriodID, HouseNo, SubLedgerID, Amt, Remarks")] DemandInitLoad demand, FormCollection fm)
        {
                if (ModelState.IsValid)
            {
                Demand dmdID; //If the Demand record doesnt exist create it.
                    demand.HouseNo = demand.HouseNo.Trim().ToUpper(); //it is alphanumeric. To Upper avoids duplications due to lowercase

                if (db.DemandDetails.Any(dde => dde.Demand.CitizenID == demand.CitizenID && dde.Demand.HouseNo == demand.HouseNo && dde.SubLedgerID == demand.SubLedgerID))
                    throw new EAshowstopper("You are attempting to enter a duplicate record. Save aborted.");

                using (var dbt = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (db.Demands.Any(d => d.CitizenID == demand.CitizenID && d.HouseNo == demand.HouseNo))
                        {
                            dmdID = db.Demands.FirstOrDefault(d => d.CitizenID == demand.CitizenID && d.HouseNo == demand.HouseNo);

                            //If remarks are updated save it, else leave existing remarks
                            if (demand.Remarks?.Length>0)
                            {
                                dmdID.Remarks = demand.Remarks;
                                db.Entry(dmdID).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            dmdID = new Demand { CitizenID = demand.CitizenID, HouseNo = demand.HouseNo, Remarks = demand.Remarks };
                            dmdID.StopDate = DateTime.Parse("31-Mar-3999");
                            db.Demands.Add(dmdID);
                        }
                        db.SaveChanges();

                        //Add the Demand Details Record
                        var dd = db.DemandDetails.Add(new DemandDetail { DemandID = dmdID.DemandID, SubLedgerID = (int)demand.SubLedgerID });

                        //Add the Demand Period record. Our PK will scream if duplicate. since this is a new record initialise the SysAmt too                        
                        var demP = db.DemandPeriods.Add(new DemandPeriod { DDID = dd.DDID, Amount = demand.Amt, SysAmt=demand.Amt, PeriodID = GetCurrentPeriod() });

                        db.DemandYears.Add(new DemandYear
                        {
                            DDID = dd.DDID,
                            DemandYear1 = (System.DateTime.Today.Month > 3) ? System.DateTime.Today.Year : System.DateTime.Today.Year - 1,
                            PeriodID = GetCurrentPeriod()
                        });

                        foreach (var item in fm)
                        {
                            if (item.ToString().Length > 5 && item.ToString().Substring(0, 5) == "data-")
                            {
                                int iLedDetID = int.Parse(item.ToString().Substring(item.ToString().LastIndexOf('-') + 1));

                                //Prevent overposting attack by checking that only valid LedgerDetails for the given subledger are saved
                                if (db.LedgerDetails.Any(l => l.LedgerDetailID == iLedDetID && l.SubLedgerID == demand.SubLedgerID))
                                    db.DemandLedgerDetails.Add(new DemandLedgerDetail {  DDID=dd.DDID, LedgerDetailID = iLedDetID, DemandLedgerDetail1 = fm[item.ToString()] });
                            }
                        }


                        db.SaveChanges();
                        dbt.Commit();
                        return RedirectToAction("Create", new { id = dmdID.CitizenID });

                    }
                    catch (Exception)
                    {
                        dbt.Rollback();
                    }
                }
            }


            return RedirectToAction("Create");
        }

        //If one day we decide to auto fetch the existing remarks onblur of HouseNo in Create Screen
        [HttpPost]
        public string getHouseRemark(int Cit, string Hou)
        {
            Hou = Hou.Trim().ToUpper();
            var re = db.Demands.FirstOrDefault(d => d.CitizenID == Cit && d.HouseNo == Hou);
            return (re != null) ? re.Remarks : "";
            
        }




        /// <summary>
        /// Using a Partial view so that the CLD can be in the same entry form as the Demands
        /// </summary>
        /// <param name="id">Chosen Subledger ID from Autocomplete box</param>        
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDmdLedgDetsPartial(int id)
        {
            var LedgDets = db.LedgerDetails.Where(ld => ld.SubLedgerID == id && ld.SubLedger.HasDemand == true);
            return PartialView("_CreateLedgDetsPartial", LedgDets);

        }

        /// <summary>
        /// Using a Partial view so that the CLD can be in the same entry form as the Demands
        /// </summary>
        /// <param name="id">Chosen Subledger ID from Autocomplete box</param>        
        /// <returns></returns>        
        public ActionResult EditLedgDetsPartial(int DDID)
        {
            var CLedgDets = db.DemandLedgerDetails.Where(cld => cld.DDID == DDID).ToList();
            return PartialView("_EditLedgDetsPartial", CLedgDets);

        }


        // GET: Demands/Edit/5
        public ActionResult Edit(int id, int SLid)
        {
            if (id == null || SLid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demand demand = db.Demands.Find(id);
            if (demand == null)
            {
                return HttpNotFound();
            }

            var Dmds = db.DemandDetails.FirstOrDefault(d => d.DemandID == id && d.SubLedgerID == SLid);
            int pd = GetCurrentPeriod();
            var dp = db.DemandPeriods.FirstOrDefault(p => p.DDID == Dmds.DDID && p.PeriodID == pd);

            var DME = new DemandInitEdit { DemandID = demand.DemandID, DDID = Dmds.DDID, HouseNo = demand.HouseNo, Remarks = demand.Remarks, StopDate = demand.StopDate.Year, Amt = dp.Amount, DemandPeriodID = dp.PeriodID };

            ViewBag.Citizen = demand.Citizen.Name;
            ViewBag.CitizenID = demand.CitizenID;
            ViewBag.SubLedger = $"For: {Dmds.SubLedger.Ledger1.Ledger1 } >> {Dmds.SubLedger.Ledger}";
            ViewBag.YearBox = MyExtensions.MakeYrRq(1, 1, DateTime.Today.Year);

            return View(DME);
        }

        private int GetCurrentPeriod()
        {
            int y = (System.DateTime.Today.Month > 3) ? System.DateTime.Today.Year : System.DateTime.Today.Year - 1;
            return db.Period.FirstOrDefault(p => p.FromYr <= y && p.ToYr >= y).PeriodID;            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DemandID, DDID, HouseNo, Amt, Remarks, StopDate, DemandPeriodID")] DemandInitEdit dmd, FormCollection fm)
        {
            if (ModelState.IsValid)
            {
                //Save Demand record House No
                var edm = db.Demands.Find(dmd.DemandID);
                edm.HouseNo = dmd.HouseNo;
                edm.StopDate = DateTime.Parse("31/March/" + dmd.StopDate);
                edm.Remarks = dmd.Remarks;
                db.Entry(edm).State = EntityState.Modified;

                //Save demand period Details record
                var edmd = db.DemandPeriods.Find(dmd.DDID, dmd.DemandPeriodID);
                edmd.Amount = dmd.Amt;
                db.Entry(edmd).State = EntityState.Modified;

                
                foreach (var item in fm)
                {
                    if (item.ToString().Length > 5 && item.ToString().Substring(0, 5) == "data-")
                    {
                        int iLedDetID = int.Parse(item.ToString().Substring(item.ToString().LastIndexOf('-') + 1));

                        var cld = db.DemandLedgerDetails.Find(iLedDetID);
                        cld.DemandLedgerDetail1 = fm[item.ToString()];

                        db.Entry(cld).State = EntityState.Modified;

                    }
                }

                
                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    db.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                
                return RedirectToAction("Details", "Citizens", new { id = edm.CitizenID });
            }


            return RedirectToAction("Create");
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
