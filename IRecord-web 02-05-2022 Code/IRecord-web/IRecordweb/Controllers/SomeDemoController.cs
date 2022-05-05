using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Controllers
{
    // [OutputCache(Duration = 10)]
    [RoutePrefix("C1")]
    public class SomeDemoController : Controller
    {
        // GET: SomeDemo
       [Route("AM1")]
        public ActionResult Index()
        {
            ViewBag.datetime = DateTime.Now;
            return View();
        }
        [Route("AM2")]
        public ActionResult Index2()
        {
            ViewBag.datetime = DateTime.Now;
            return View("Index");
        }
    }
}