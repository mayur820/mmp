using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IRecordweb.Controllers
{
    public class OnboardingController : Controller
    {
        // GET: Onboarding
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Offer()
            {
            return View();
            }
        public ActionResult Investor(string id)
            {
            Session["Sucb_RoleId"] = id;
            return View();
            }
        [HttpPost]
        public ActionResult Investor(int id)
            {
            return View();
            }

        [HttpGet]
        public ActionResult Advisor(string id)
            {
            Session["Sucb_RoleId"] = id;
            return View();
            }
        [HttpPost]
        public ActionResult Advisor(int id)
            {
            return View();
            }

        [HttpGet]
        public ActionResult Accountant(string id)
            {
            Session["Sucb_RoleId"] = id;
            return View();
            }
        [HttpPost]
        public ActionResult Accountant(int id)
            {
           // USER _User = new USER();
           // _User.UserName=Session["ONborUserName"].ToString();
           // _User.Password = Session["ONborPassword"].ToString();
           // LoginController ng = new LoginController();

           //ng.UserLogin(_User);


           // FormsAuthentication.SetAuthCookie(_User.UserName, false);

           // ViewBag.Message = "Login Successful !!";

           // return Redirect("~/Home/Index");
            return View();
            }
        }
}