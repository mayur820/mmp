
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRecordweb.Models;
using BAL;
using DAL;

namespace IRecordweb.Controllers
    {
    public class CompanyMasterController : Controller
        {
        DAL.Master obj = new Master();
       
        // GET: CompanyMaster
        public ActionResult Index()
            {
            
            return View();
            }
        [HttpGet]
        public ActionResult ShowData(Subscriber __subscriber)
            {
            obj.GetSubscriberMaster(__subscriber);
            return View();
            }
        //[HttpPost]
        //public ActionResult ShowData(Company _Company)
        //    {
        //    var data = obj.GetCompanyMaster(_Company);
        //    //string message = "Records display !!";
        //    //return Json(new { message }, JsonRequestBehavior.AllowGet);
        //    return View(data);
        //    }

        //[ActionName("Insert")]
        [HttpGet]
        public ActionResult CreateSubscriber()
            {
            return View();
            }
        [HttpPost]
        public ActionResult CreateSubscriber(Subscriber _subscriber)
            {
            try
                {
                if (ModelState.IsValid)
                    {
                    // TODO: Add insert logic here
                    obj.InsertSubscriberMaster(_subscriber);
                    ViewBag.Message = "Records Save Sucessfully !!";
                    }               
                return View();
                }
            catch (Exception ex)
                {

                return View(ex);
                }
            }
        }
    }