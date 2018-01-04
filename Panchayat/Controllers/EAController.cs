using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panchayat.Controllers
{
    [HandleError]
    public class EAController : Controller
    {
        protected Entities db;
        public EAController()
        {
            this.db = new Entities();
        }

        // GET: EA
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (DateTime.Now.Date > DateTime.Parse("15 Aug 2017"))
            //{                
            //    filterContext.Result = new RedirectResult("~/Home/pli");
            //    return;
            //}
            
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controller = filterContext.RouteData.Values["controller"].ToString();
                string action = filterContext.RouteData.Values["action"].ToString();
                Exception ex = filterContext.Exception;

                filterContext.Result = new ViewResult {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary(new HandleErrorInfo(ex, controller, action))
                };
                filterContext.ExceptionHandled = true;
            }
        }

        public ActionResult AutoCompleteCitz(string term)
        {
            var filteredItems = db.Citizens.Where(c => c.Name.Contains(term)).Select(c => new { id = c.CitizenID, value = c.Name });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Used only for Subledger autocomplete
        /// </summary>
        protected internal class SLautoComplete
        {
            public int id { get; set; }
            public string value { get; set; }
        }


        public ActionResult AutoCompleteSubLedgs(string term, bool? IsIncome, bool? HasDemand)
        {
            var fi = db.SubLedgers.Where(c => c.Ledger.Contains(term));

            if (IsIncome != null)
                fi = fi.Where(f => f.Ledger1.IsIncome == IsIncome);

            if (HasDemand.HasValue)
                fi = fi.Where(f => f.HasDemand == HasDemand);

            IEnumerable<SLautoComplete> filteredItems = fi.Select(c => new SLautoComplete { id = c.SubLedgerID, value = c.Ledger1.Ledger1 + " >> " + c.Ledger });
            
            return Json((IEnumerable<SLautoComplete>) filteredItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Using a Partial view so that the CLD can be in the same entry form as the Receipts and Vouchers
        /// </summary>
        /// <param name="id">Chosen Subledger ID from Autocomplete box</param>        
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateLedgDetsPartial(int id)
        {
            var LedgDets = db.LedgerDetails.Where(ld => ld.SubLedgerID == id && ld.SubLedger.HasDemand ==false);
            return PartialView("_CreateLedgDetsPartial", LedgDets);

        }
        

    }
}