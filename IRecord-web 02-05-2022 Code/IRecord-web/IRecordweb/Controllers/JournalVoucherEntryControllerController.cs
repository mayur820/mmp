using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Controllers
{
    public class JournalVoucherEntryControllerController : Controller
    {
        DAL.JournalVoucherDAL obj = new DAL.JournalVoucherDAL();
        // GET: JournalVoucherEntryController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SaveJournalVoucherEntry()
        {
            ViewBag.UGroupName = new SelectList(obj.GroupList().ToList(), dataValueField: "UGroupID", dataTextField: "UGroupName");
            ViewBag.Narration = new SelectList(obj.GetNarrationList().ToList(), dataValueField: "Code", dataTextField: "Name");
            return View();
        }
        [HttpPost]
        public ActionResult SaveJournalVoucherEntry(JournalVoucherEntry _Journal)
        {
            DateTime Selected_Date = _Journal.Date;
            DateTime currentdate = DateTime.Now;
            ViewBag.UGroupName = new SelectList(obj.GroupList().ToList(), dataValueField: "UGroupID", dataTextField: "UGroupName");
            ViewBag.Narration = new SelectList(obj.GetNarrationList().ToList(), dataValueField: "Narrationid", dataTextField: "Name");
            if (ModelState.IsValid)
            {
                if (Selected_Date > currentdate)
                {
                    ViewBag.Message = BAL.Common.DateMessage;
                }
                else
                {
                    obj.InsertJournalvoucher(_Journal);
                    ViewBag.Message = BAL.Common.SaveMessage;
                }
            }
            return View();
        }

        public JsonResult GetAccountList(int UGroupID, Group _Group)
        {
            List<AccountMaster> Accountdata = obj.JournalAccountList(_Group).ToList();
            return Json(Accountdata, JsonRequestBehavior.AllowGet);
        }


    }
}