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
    public class PropertyBookingsController : EAController
    {        
        // GET: PropertyBookings
        public ActionResult Index(int? page, DateTime? hbd,DateTime? dor)
        {
            var propertyBookings = db.PropertyBookings.Where(a=>a.PropertyBookingID>0);
            if (hbd != null)
            {
                propertyBookings = db.PropertyBookings.Where(a => a.HDate ==hbd);
            }
            if (dor != null)
            {
                propertyBookings = db.PropertyBookings.Where(a => a.HDate == dor);
            }
            if(hbd==null && dor==null)
            {
                int FinYr = MyExtensions.GetFinYr();
                propertyBookings = propertyBookings.Where(p => ((DateTime)p.TDate).Year == FinYr);
            }
            int pageSize = db.Configs.FirstOrDefault().RowsPerPage ?? 5;
            int pageNumber = (page ?? 1);
            return View(propertyBookings.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: PropertyBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyBooking propertyBooking = db.PropertyBookings.Find(id);
            if (propertyBooking == null)
            {
                return HttpNotFound();
            }
            return View(propertyBooking);
        }

        // GET: PropertyBookings/Create
        public ActionResult Create()
        {
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "PropertyName");
            return View();
        }

        // POST: PropertyBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PropertyBookingID,PropertyID,Inward_No,TDate,NameOfApplicant,PhoneNo,HDate,AdvReceiptNo,AdvAmount,SecurityDepositAmt,ApplForSDRefund,FullPayReceiptNo,FullPayAmount,LuxTaxReceiptNo,RefundSDVoucherNo,PaymentOfLuxTax,Remarks")] PropertyBooking propertyBooking)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var AdvRcpt = new Form4 { CitizenID = null, Amount = propertyBooking.AdvAmount, PayDate = DateTime.Now, LedgerID = 7, SubLedgerID = 3, RecvdFrom = propertyBooking.NameOfApplicant, HouseNo = null };
                        db.Form4.Add(AdvRcpt);
                        db.SaveChanges();
                        propertyBooking.AdvReceiptNo = AdvRcpt.RecieptNo;
                        propertyBooking.ApplForSDRefund = null;
                        propertyBooking.FullPayReceiptNo = null;
                        propertyBooking.FullPayAmount = null;
                        propertyBooking.LuxTaxReceiptNo = null;
                        propertyBooking.RefundSDVoucherNo = null;
                        propertyBooking.PaymentOfLuxTax = null;
                        propertyBooking.TDate = DateTime.Now;
                        db.PropertyBookings.Add(propertyBooking);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    return RedirectToAction("Index");
                }
            }

            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "PropertyName", propertyBooking.PropertyID);
            return View(propertyBooking);
        }

        // GET: PropertyBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyBooking propertyBooking = db.PropertyBookings.Find(id);
            if (propertyBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "PropertyName", propertyBooking.PropertyID);
            return View(propertyBooking);
        }

        // POST: PropertyBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyBookingID,PropertyID,Inward_No,TDate,NameOfApplicant,PhoneNo,HDate,AdvReceiptNo,AdvAmount,SecurityDepositAmt,ApplForSDRefund,FullPayReceiptNo,FullPayAmount,LuxTaxReceiptNo,RefundSDVoucherNo,PaymentOfLuxTax,Remarks")] PropertyBooking propertyBooking)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(propertyBooking).State = EntityState.Modified;
                        if (propertyBooking.FullPayAmount != null)
                        {
                            if (!propertyBooking.FullPayReceiptNo.HasValue)//Create a receipt only if it is the initial edit
                            {
                                var fullPayRcpt = new Form4 { CitizenID = null, Amount = propertyBooking.FullPayAmount, PayDate = DateTime.Now, LedgerID = 7, SubLedgerID = 3, RecvdFrom = propertyBooking.NameOfApplicant, HouseNo = null };
                                db.Form4.Add(fullPayRcpt);
                                db.SaveChanges();
                                propertyBooking.FullPayReceiptNo = fullPayRcpt.RecieptNo;
                            }
                        }
                        if (propertyBooking.PaymentOfLuxTax != null)
                        {
                            if (!propertyBooking.LuxTaxReceiptNo.HasValue)
                            {
                                var LuxTax = new Form4 { CitizenID = null, Amount = propertyBooking.PaymentOfLuxTax, PayDate = DateTime.Now, LedgerID = 7, SubLedgerID = 3, RecvdFrom = propertyBooking.NameOfApplicant, HouseNo = null };
                                db.Form4.Add(LuxTax);
                                db.SaveChanges();
                                propertyBooking.LuxTaxReceiptNo = LuxTax.RecieptNo;
                            }
                        }
                        if (propertyBooking.ApplForSDRefund != null)
                        {
                            if (!propertyBooking.RefundSDVoucherNo.HasValue)
                            {
                                string UserID = User.Identity.GetUserName();
                                var pn = db.Configs.Select(x => x.VP).FirstOrDefault();


                                var RefundSD = new Voucher { PassedBy = UserID, of = pn, Amount = propertyBooking.SecurityDepositAmt, ActualAmount = propertyBooking.SecurityDepositAmt, For = "Security Deposit refund of property booking", PayDate = propertyBooking.TDate, CBfolio = null, ResNo = null, HeldOn = propertyBooking.HDate, Meeting = "N/A", LedgerID = 7, SubLedgerID = 3};
                                db.Vouchers.Add(RefundSD);
                                db.SaveChanges();
                                propertyBooking.RefundSDVoucherNo = RefundSD.VoucherID;
                            }
                        }
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
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "PropertyName", propertyBooking.PropertyID);
            return View(propertyBooking);
        }

        // GET: PropertyBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyBooking propertyBooking = db.PropertyBookings.Find(id);
            if (propertyBooking == null)
            {
                return HttpNotFound();
            }
            return View(propertyBooking);
        }

        // POST: PropertyBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PropertyBooking propertyBooking = db.PropertyBookings.Find(id);
            db.PropertyBookings.Remove(propertyBooking);
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
