using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Controllers
    {
    public class UserDetailsController : Controller
        {
        DAL.Master obj = new DAL.Master();
        // GET: UserDetails
        public ActionResult Index()
            {
            return View();
            }
        [HttpGet]
        public ActionResult SaveUserDetails()
            {          
            return View();
            }
        [HttpPost]
        public ActionResult SaveUserDetails(UserDetails _UserDetails)
            {
            if (ModelState.IsValid)
                {
                obj.InsertUserDetailsMaster(_UserDetails);
                ViewBag.Message = "Records Save Sucessfully !!";
                }
            return View();
            }
        }
    }