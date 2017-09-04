using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Panchayat;
using System.Collections;
using System.Data.SqlClient;

/// <summary>
/// Flags for generating the report reqest
///     ViewBag.PeriodID = new SelectList(db.PeriodSLs, "PeriodID", "PeriodSL1");
///     ViewBag.YearBox = MyExtensions.MakeYrRq(3, 1, DateTime.Today.Year);
///     ViewBag.MonthBox = MyExtensions.MonthList();
///     ViewBag.ForDate = 1;
///     ViewBag.SubLedger = 1;
///     ViewBag.IsIncome = new List<SelectListItem>() { new SelectListItem() { Value = "True", Text = "Receipts" }, new SelectListItem() { Value = "False", Text = "Expenditure" } };
///     
/// </summary>

namespace Panchayat.Controllers
{
    [Authorize(Roles = "Boss,Type1")]
    public class ReportsController : EAController
    {
        // GET: Form10
        public ActionResult Form10RptRq()
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(8, 1, DateTime.Today.Year);
            ViewBag.MonthBox = MyExtensions.MonthList();
            
            ViewBag.ReturnAction = "Form10Report";
            ViewBag.Title = "Fetch Form 10";
            
            return View("ReportRq");
        }

        [HttpPost]
        public ActionResult Form10Report(FormCollection dt)
        {
            if (dt["RptYear"] == null || dt["RptMonth"] == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int RptYr = int.Parse(dt["RptYear"]);
            int RptMon = int.Parse(dt["RptMonth"]);

            string queryString = @"SELECT l.Ledger, sl.Ledger as SubLedger, l.IsIncome, COALESCE(SUM(Amount),0)  as Amount  FROM Ledgers l INNER JOIN SubLedgers sl ON l.LedgerID=sl.LedgerID
	            LEFT JOIN Form3 f3 ON (sl.SubLedgerID=f3.SubLedgerID AND MONTH(f3.PayDate)=@RptMon AND YEAR(f3.PayDate)=@RptYr)
	            GROUP BY l.IsIncome, l.Ledger, sl.Ledger";

            var parameters = new List<SqlParameter> {
                new SqlParameter("@RptMon", RptMon),
                new SqlParameter("@RptYr", RptYr) 
            };


            var Form10 = db.Database.SqlQuery<Form10rpt>(queryString, parameters.ToArray());
            ViewBag.vpname = db.Configs.Find(1).VP;
            ViewBag.MonYr = MyExtensions.MonthFromInt(RptMon) + " " + RptYr;

            return View("Form10Report", Form10.ToList());
        }

        // GET: Form1
        public ActionResult Form1RptRq()
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(8, 1, DateTime.Today.Year);
            ViewBag.MonthBox = MyExtensions.MonthList();
            ViewBag.SubLedger = 1;

            ViewBag.ReturnAction = "Form1Report";
            ViewBag.Title = "Fetch Form 1";

            return View("ReportRq");
        }

        [HttpPost]
        public ActionResult Form1Report(FormCollection dt)
        {
            if (dt["RptYear"] == null || dt["RptMonth"] == null || dt["SubLedgerID"] == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int RptYr = int.Parse(dt["RptYear"]);            
            int RptMon = int.Parse(dt["RptMonth"]);
            int SLid = int.Parse(dt["SubLedgerID"]);

            var sl = db.SubLedgers.FirstOrDefault(l => l.SubLedgerID == SLid);
            ViewBag.RptFor = sl.Ledger1.Ledger1 + " >> " + sl.Ledger;
            ViewBag.IsIncome = (sl.Ledger1.IsIncome) ? "Receipt" : "Expenditure";

            var form1 = db.Form1.Where(r=>r.SubLedgerID==SLid && r.EntryDate.Value.Month==RptMon && r.EntryDate.Value.Year==RptYr) ;
            return View("Form1Report", form1.ToList());
        }

        // GET: Form2
        public ActionResult Form2RptRq()
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(8, 1, DateTime.Today.Year);            
            ViewBag.SubLedger = 1;

            ViewBag.Title = "Fetch Form 2";
            ViewBag.ReturnAction = "Form2Report";

            return View("ReportRq");
        }

        [HttpPost]
        public ActionResult Form2Report(FormCollection fm)
        {
            if (fm["RptYear"] == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int RptYr = int.Parse(fm["RptYear"]);            
            int SLid = int.Parse(fm["SubLedgerID"]);

            var sl = db.SubLedgers.FirstOrDefault(l => l.SubLedgerID == SLid);
            ViewBag.RptFor = sl.Ledger1.Ledger1 + " >> " + sl.Ledger;
            ViewBag.IsIncome = (sl.Ledger1.IsIncome) ? "Receipt" : "Expenditure";

            var form2 = db.Form2.Where(r => r.Year == RptYr && r.SubLedgerID == SLid);
            return View("Form2Report", form2.ToList());
        }

        public ActionResult Form3RptRq()
        {
            ViewBag.ForDate = 1;
            ViewBag.IsIncome = new List<SelectListItem>() { new SelectListItem() { Value = "True", Text = "Receipts" }, new SelectListItem() { Value = "False", Text = "Expenditure" } };
            ViewBag.Title = "Fetch Form 3 ";
            ViewBag.ReturnAction = "Form3Report";
            return View("ReportRq");
        }

        [HttpPost]
        public ActionResult Form3Report(FormCollection fm)
        {
            if (fm["ForDate"] == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var y = DateTime.Parse(fm["ForDate"]);
            DateTime x = y.AddDays(-1);
            var IsIncome = bool.Parse(fm["IsIncome"]);

            ViewBag.IsIncome = (IsIncome) ? "Receipt" : "Expenditure";
            ViewBag.OB = db.CBRunnings.FirstOrDefault(c => c.EntryDate == x).CBTotal;
            ViewBag.OB = ViewBag.OB ?? 0;
            ViewBag.fy = y.Year;
            var ff = db.Form3.Where(c => c.PayDate == y).ToList();
            return View("Form3Report", ff);
        }



        public ActionResult Form7RptRq()
        {
            ViewBag.PeriodID = new SelectList(db.PeriodSLs, "PeriodID", "PeriodSL1");
            ViewBag.Title = "Fetch Form 7 Report";
            ViewBag.ReturnAction = "Form7Report";
            return View("ReportRq");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Form7Report(int PeriodID)
        {
            if (ModelState.IsValid)
            {
                var PeriodStart = db.Period.FirstOrDefault(p => p.PeriodID == PeriodID);
                DateTime Pstart = DateTime.Parse("1/April/" + PeriodStart.FromYr);
                DateTime Pend = DateTime.Parse("31/March/" + PeriodStart.ToYr);
                
                List<Form7Rpt> fSevn = db.Demands.Where(d => d.StopDate >= Pstart && d.CreatedOn <= Pend)
                    .Select(g => new Form7Rpt { form7ID = g.DemandID,
                        CitID = (int)g.CitizenID,
                        CitName = g.Citizen.Name,
                        House = g.HouseNo,
                        Remarks = g.Remarks
                        }).ToList<Form7Rpt>();

                foreach (var item in fSevn)
                {
                    item.TaxDmd = db.DemandPeriods
                        .Where(c => c.DemandDetail.DemandID == item.form7ID && c.PeriodID== PeriodID)
                        .Select(c => new TaxDmd { SLid = (int)c.DemandDetail.SubLedgerID, DmdAmt = (decimal)(c.Amount ?? c.SysAmt), SLname = c.DemandDetail.SubLedger.Ledger})
                        .OrderBy(c => c.SLid)
                        .ToList<TaxDmd>();
                }

                return View(fSevn);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        public ActionResult Form8RptRq()
        {
            ViewBag.PeriodID = new SelectList(db.PeriodSLs, "PeriodID", "PeriodSL1");
            ViewBag.Title = "Fetch Form 8 Report";
            ViewBag.ReturnAction = "Form8Report";
            return View("ReportRq");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Form8Report(int PeriodID)
        {
            if (ModelState.IsValid)
            {
                var PeriodStart = db.Period.FirstOrDefault(p => p.PeriodID == PeriodID);
                DateTime Pstart = DateTime.Parse("1/April/" + PeriodStart.FromYr);
                DateTime Pend = DateTime.Parse("31/March/" + PeriodStart.ToYr);
                ViewBag.SelPeriod = PeriodStart;

                List<Form7Rpt> fSevn = db.Demands.Where(d => d.StopDate >= Pstart && d.CreatedOn <= Pend)
                    .Select(g => new Form7Rpt
                    {
                        form7ID = g.DemandID,
                        CitID = (int)g.CitizenID,
                        CitName = g.Citizen.Name,
                        House = g.HouseNo,
                        Remarks = g.Remarks
                    }).ToList<Form7Rpt>();

                foreach (var item in fSevn)
                {
                    item.TaxDmd = db.DemandPeriods
                        .Where(c => c.DemandDetail.DemandID == item.form7ID && c.PeriodID == PeriodID)
                        .Select(c => new TaxDmd { SLid = (int)c.DemandDetail.SubLedgerID, DDID=c.DDID, DmdAmt = (decimal)(c.Amount ?? c.SysAmt), SLname = c.DemandDetail.SubLedger.Ledger })
                        .OrderBy(c => c.SLid)
                        .ToList<TaxDmd>();


                    foreach (var td in item.TaxDmd)
                    {
                        td.TaxActuals = db.DemandYears.Where(y => y.PeriodID == PeriodID && y.DDID == td.DDID && y.DemandDetail.SubLedgerID==td.SLid)
                            .Select(y => new TaxActuals { Arrears = (decimal)(y.Arrears??0), f8yr = y.DemandYear1, PaidAmt = (decimal)(y.RecptAmt??0), PayDate = (DateTime) y.RecptDate, ReceiptNo = (int)(y.RecieptNo??0) })
                            .ToDictionary(r => r.f8yr);
                    }
                }

                return View(fSevn);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Form7Report(int RptYear)
        //{
        //    if (ModelState.IsValid && RptYear > 2000 && RptYear < 3000)
        //    {
        //        //Make a nice ordered list of the Citizens with an ordered child list of his tax demands
        //       List<Form7Rpt> fSevn = db.Form7.Where(f => f.Year == RptYear && f.HouseNo.Length > 0)
        //        .Select(g => new Form7Rpt { form7ID = g.Form7ID, CitID = (int)g.CitizenID, CitName = g.Citizen.Name, House = g.HouseNo }).ToList<Form7Rpt>();



        //        //List<Form7Rpt> fSevn = db.Form7.Where(f => f.Year == RptYear && f.HouseNo.Length > 0)
        //        //    .Join(db.Citizens, 
        //        //        c  => c.CitizenID,
        //        //        s =>s.CitizenID,
        //        //        (Form7,Citizen) => new
        //        //        Form7Rpt { CitID = (int) Form7.CitizenID, CitName = Citizen.Name, House = Form7.HouseNo })
        //        //    .OrderBy(c =>c.CitName)
        //        //    .ToList<Form7Rpt>();

        //        string remks = "";
        //        foreach (var item in fSevn)
        //        {
        //            item.TaxDmd = db.Form7
        //                .Where(c => c.CitizenID == item.CitID && c.Year == RptYear)
        //                .Select(c => new TaxDmd { SLid = (int)c.SubLedgerID, DmdAmt = (decimal)c.DemandAmount, SLname = c.SubLedger.Ledger, rems = c.Remark })
        //                .OrderBy(c => c.SLid)
        //                .ToList<TaxDmd>();

        //            //Compile the remarks
        //            foreach (var r in item.TaxDmd)
        //                remks += r.rems + "; ";

        //            item.Remarks = remks;
        //            remks = "";


        //        }



        //        //List<Form7Rpt> fSevn = db.Form7.Where(f => f.Year == RptYear && f.HouseNo.Length > 0)
        //        //    .Select(g => new Form7Rpt { CitID = (int)g.CitizenID, House = g.HouseNo }).ToList<Form7Rpt>();

        //        //List<Form7Rpt> fSevn = db.Form7.Where(f => f.Year == RptYear && f.HouseNo.Length>0)
        //        //    .GroupBy(g => new { g.CitizenID, g.HouseNo })
        //        //    .Select(g => new Form7Rpt { CitID = (int)g.Key.CitizenID, House = g.Key.HouseNo }).ToList<Form7Rpt>();

        //        //f7 = f7.Where(h => h.House.Length > 0);

        //        return View(fSevn);
        //    }
        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}



        public ActionResult GenericRptRq()
        {
            ViewBag.ForDate = 1;
            ViewBag.SubLedger = 1;
            ViewBag.Title = "Generic Report ";
            ViewBag.ReturnAction = "GenericReport";
            return View("ReportRq");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenericReport(DateTime ForDate, int SubLedgerID)
        {
            if (ModelState.IsValid && SubLedgerID !=null && ForDate !=null)
            {
                var sl = db.SubLedgers.Find(SubLedgerID);
                ViewBag.Title = "Register For " + sl.Ledger;
                ViewBag.RVtype = sl.Ledger1.IsIncome;
                 
                IEnumerable <GenericReport> res = (sl.Ledger1.IsIncome) ? db.Form4.Where(f => f.SubLedgerID == SubLedgerID && f.PayDate==ForDate).Select(f => new GenericReport { SLid = (int)f.SubLedgerID, Amount = (decimal)f.Amount, RVid = f.RecieptNo, Tdate = (DateTime) f.PayDate })  : db.Vouchers.Where(f => f.SubLedgerID == SubLedgerID && f.PayDate == ForDate).Select(f => new GenericReport { SLid = (int)f.SubLedgerID, Amount = (decimal)f.Amount, RVid = f.VoucherID, Tdate = (DateTime) f.PayDate });

                ViewBag.DyHeads = db.LedgerDetails.Where(l => l.SubLedgerID == SubLedgerID).Select(l=>l.LedgerDetail1);
                

                return View("GenericReport", res);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GenericReportDetails(bool RVtype, int RVid)
        {
            var res = (RVtype) ? db.RVdetails.Where(rv => rv.ReceiptNo == RVid) : db.RVdetails.Where(rv => rv.VoucherID == RVid);
            return PartialView("_GenericReportDetails", res);
        }

        public ActionResult RVRptRq(int Id)
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(5, 1, DateTime.Today.Year);
            ViewBag.SubLedger = Id;
            ViewBag.Title = db.SubLedgers.Find(Id).Ledger;
            ViewBag.ReturnAction = "RVReport";

            return View("ReportRq");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RVReport(int RptYear, int SubLedgerID)
        {
            if (ModelState.IsValid )
            {
                var sl = db.SubLedgers.Find(SubLedgerID);
                string RptName = sl.Ledger;
                IEnumerable<RVReport> res = (sl.Ledger1.IsIncome) ?
                    db.Form4.Where(f => f.SubLedgerID == SubLedgerID && ((DateTime)f.PayDate).Year == RptYear).Select(f => new RVReport { Amount = (decimal)f.Amount, RVid = f.RecieptNo, Tdate = (DateTime)f.PayDate }).ToList()
                    : db.Vouchers.Where(f => f.SubLedgerID == SubLedgerID && ((DateTime)f.PayDate).Year == RptYear).Select(f => new RVReport { Amount = (decimal)f.Amount, RVid = f.VoucherID, Tdate = (DateTime)f.PayDate }).ToList();

                //Fetch Detail Data
                foreach (var r in res)
                {
                    r.DetailData = db.LedgerDetails.Where(l => l.SubLedgerID == SubLedgerID).ToDictionary(l => l.LedgerDetail1, l => "");

                    List<RVdetail> detvals = (sl.Ledger1.IsIncome) ? db.RVdetails.Where(rv => rv.ReceiptNo == r.RVid).ToList() : db.RVdetails.Where(rv => rv.VoucherID == r.RVid).ToList();
                    ViewBag.date =db.Form4.Where(x => x.RecieptNo == r.RVid).Select(x=>x.PayDate).FirstOrDefault();
                    int i = 0;
                    foreach (var item in r.DetailData.Keys.ToList())
                    {
                        r.DetailData[item] = detvals[i].RVDetail1;
                        i++;
                    }
                }
                return View(RptName, res);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
