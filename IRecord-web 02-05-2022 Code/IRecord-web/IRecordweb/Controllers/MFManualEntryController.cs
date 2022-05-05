using BAL;
using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Controllers
{
    public class MFManualEntryController : Controller
    {
        // GET: MFManualEntry
        DAL.Master obj = new DAL.Master();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SaveMFEntry()
            {
            Script _script = new Script();
            ViewBag.Consultant = new SelectList(obj.BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.FundFamily = new SelectList(obj.BindFundFamilyMaster().ToList(), dataValueField: "MutualFundID", dataTextField: "Name");
            ViewBag.DematAC = new SelectList(obj.BindDematMaster().ToList(), dataValueField: "DematID", dataTextField: "Name");
            ViewBag.PaymentMode = new SelectList(obj.BindPaymentType().ToList(), dataValueField: "TypeId", dataTextField: "Name"); 
            ViewBag.Scheme = new SelectList(obj.BindScheme(_script).ToList(), dataValueField: "ScriptID", dataTextField: "Scheme");
            //ViewBag.Bank = new SelectList(obj.BindBankAccount().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            ViewBag.Bank = new SelectList(obj.BindBrokerList().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            obj.BindConsultantMaster();
            return View();
            }
        [HttpPost]
        public ActionResult SaveMFEntry(MutualFundManualEntry _Entry)
            {
            Script _script = new Script();
            ViewBag.Consultant = new SelectList(obj.BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.FundFamily = new SelectList(obj.BindFundFamilyMaster().ToList(), dataValueField: "MutualFundID", dataTextField: "Name");
            ViewBag.DematAC = new SelectList(obj.BindDematMaster().ToList(), dataValueField: "DematID", dataTextField: "Name");
            ViewBag.PaymentMode = new SelectList(obj.BindPaymentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.Scheme = new SelectList(obj.BindScheme(_script).ToList(), dataValueField: "ScriptID", dataTextField: "Scheme");
            ViewBag.Bank = new SelectList(obj.BindBrokerList().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            ModelState["STT"].Errors.Clear();
            if (ModelState.IsValid)
                {
                _Entry.TransactionId = obj.InsertACTransEntry(_Entry);
                obj.InsertMFManualEntry(_Entry);
                obj.InsertBRDematTransEntry(_Entry);
              //  obj.InsertBRDematTransEntry(_Entry);
                obj.InsertBRTransEntry(_Entry);
                ViewBag.Message = "Data Saved Successfully !!";
                }
            return View();
            }

        public JsonResult GetSchemeList(int MutualFundID, Script script)
            {
            //db.Configuration.ProxyCreationEnabled = false;
            // var data = obj.BindScheme(script).Where(x => x.MutualFundID == MutualFundID);
            List<Script> scriptdata = obj.BindScheme(script);
              //  db.States.Where(x => x.CountryId == CountryId).ToList();
            return Json(scriptdata, JsonRequestBehavior.AllowGet);

            }

        }
}