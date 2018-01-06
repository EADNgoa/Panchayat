using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panchayat.Controllers
{
    [Authorize(Roles = "Boss,Type1,Type2")]
    public class HomeController : EAController
    {
        public ActionResult Index()
        {
            int days = (int)db.Configs.First().MeetingAlert;
            var currdate = DateTime.Now;
            var md = currdate.AddDays(days);
            ViewBag.meets = db.Meetings.Where(a => a.MeetingDate > currdate && a.MeetingDate <= md);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult pli()
        {
            return View();
        }
    }
}