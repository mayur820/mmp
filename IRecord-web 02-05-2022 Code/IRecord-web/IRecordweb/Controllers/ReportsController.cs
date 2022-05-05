using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports 
        public ActionResult HoldingReports()
        {
            return View();
        }
        public ActionResult AccountLedger()
        {
            return View();
        }
        public ActionResult StockHolding()
        {
            return View();
        }
        public ActionResult Gain_Loss()
        {
            return View();
        }
    }
}