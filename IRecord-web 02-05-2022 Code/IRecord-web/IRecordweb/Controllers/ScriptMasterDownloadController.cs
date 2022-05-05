using BAL;
using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace IRecordweb.Controllers
    {
    public class ScriptMasterDownloadController : Controller
        {
        DAL.Master obj = new DAL.Master();
        MType mtype = new MType();
        // GET: ScriptMasterDownload
        public ActionResult Index()
            {
            return View();

            }
       
        [HttpGet]
        public ActionResult ScriptUpload(string Upload, string Download, HttpPostedFileBase FilePath , int? Page)
            {
            ScriptUploadDownload _Script = new ScriptUploadDownload();      
           
            _Script.scripdata = obj.SelectallScriptdata();
           // data.ToList().ToPagedList(10, 10);

             ViewBag.InvestmentType = new SelectList(obj.BindInvenstmentType(mtype).ToList(), dataValueField: "TypeId", dataTextField: "Name");
            return View(_Script);
          //  return View(data.ToPagedList(1, 10));
            }

        [HttpPost]
        public ActionResult ScriptUpload(ScriptUploadDownload _Script, string Download, string Upload, string Save, HttpPostedFileBase FilePath, IRecordweb.Models.ImportExcel import)
            {

            if (!string.IsNullOrEmpty(Download))
                {
                // _Script.scripdata = obj.SelectallScriptdata();
                _Script.scripdata = obj.InsertDownloadScriptMaster(_Script);
                //obj.DownloadScriptData(_Script, FilePath);
                }
            else if (!string.IsNullOrEmpty(Upload))
                {
                if (_Script.FilePath.ContentLength > 0)
                    {                   
                        _Script.scripdata = obj.UploadData(_Script, FilePath);
                    }
                }
            else if (!string.IsNullOrEmpty(Save))
                {
                _Script.scripdata = obj.InsertBulkScriptMaster(_Script, FilePath);
                ViewBag.Message = "Data Upload Successfully";
                }

            ViewBag.InvestmentType = new SelectList(obj.BindInvenstmentType(mtype).ToList(), dataValueField: "TypeId", dataTextField: "Name");
            return View(_Script);
            }
        [HttpGet]
        public ActionResult DownloadScript(string Upload, HttpPostedFileBase FilePath)
            {
            ScriptUploadDownload _Script = new ScriptUploadDownload();
           
            _Script.scripdata = obj.UploadData(_Script, FilePath);
            return View(_Script);
            }


       [HttpPost]
        public ActionResult DownloadScript(ScriptUploadDownload _Script, string Upload, HttpPostedFileBase FilePath)
            {
            if (!string.IsNullOrEmpty(Upload))
                {
                if (_Script.FilePath.ContentLength > 0)
                    {
                    _Script.scripdata = obj.UploadData(_Script, FilePath);
                    ViewBag.Message = "Saved Successfully";
                    }
                }
            return View(_Script);
            }

        }
    }