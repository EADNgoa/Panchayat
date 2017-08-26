using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Panchayat.Controllers
{
    [Authorize(Roles = "Boss,Type1")]
    public class CitizensController : EAController
    {
        

        // GET: Citizens
        
        public ActionResult Index( string searchString, int? page)
        {


            var citizs = db.Citizens.Where(c => c.Name.Length > 2);
           
            if (!string.IsNullOrEmpty(searchString))
            {
                citizs = citizs.Where(p => p.Name.Contains(searchString));

                page = 1;
            }

        

            citizs = citizs.OrderBy(e => e.Name);

            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(citizs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Citizens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citizen citizen = db.Citizens.Find(id);
            if (citizen == null)
            {
                return HttpNotFound();
            }

            
            return View(citizen);
        }


        public ActionResult FetchCitDmd(int id)
        {
            return PartialView(db.Demands.Where(d => d.CitizenID == id ));            
        }

        /// <summary>
        /// Partial View for dynamic fields of CLD
        /// </summary>
        /// <param name="DemandID"></param>
        /// <param name="SLid"></param>
        /// <returns></returns>
        public ActionResult FetchCitLedDets(int DDID, int SLid)
        {
            var cld = db.DemandLedgerDetails.Where(c => c.DDID == DDID);
            ViewBag.ld = db.LedgerDetails.Where(l => l.SubLedgerID == SLid);
            return PartialView("_FetchCitLedDets", cld.ToList());           

        }

        // GET: Citizens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Citizens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CitizenID,Name,Mobile,Email,ResAddress")] Citizen citizen)
        {
            if (ModelState.IsValid)
            {
                db.Citizens.Add(citizen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(citizen);
        }

        // GET: Citizens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citizen citizen = db.Citizens.Find(id);
            if (citizen == null)
            {
                return HttpNotFound();
            }
            return View(citizen);
        }

        // POST: Citizens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CitizenID,Name,Mobile,Email,ResAddress")] Citizen citizen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citizen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(citizen);
        }

        // GET: Citizens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citizen citizen = db.Citizens.Find(id);
            if (citizen == null)
            {
                return HttpNotFound();
            }
            return View(citizen);
        }

        // POST: Citizens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Citizen citizen = db.Citizens.Find(id);
            db.Citizens.Remove(citizen);
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
