using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportDocumentationConsole.ViewModels;

namespace ReportDocumentationConsole.Controllers
{
    public class StoredProcsController : Controller
    {
        private DB.MSBDWEntities DB_MSBDW = new DB.MSBDWEntities();
        // GET: StoredProcs
        [HttpPost]
        public ActionResult Index()
        {
            
            //int selectedReport = Convert.ToInt32(collection["selectedReportId"]);
            int selectedReport = Convert.ToInt32(Request.Form["selectedReportId"]);
            //string buttonName = Request.Form["buttonName"];
            List<DB.ReportSP> reportSP = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == selectedReport).OrderByDescending(sp => sp.RowCreateDate).ToList();
            StoredProcsViewModel reportsViewModel = new StoredProcsViewModel(reportSP);
            ViewData["selectedReportId"] = selectedReport;
            ViewData["buttonName"] = "SP";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            return View(reportsViewModel.reportSPs);
        }


        //public ActionResult AddStoredProcs()
        //{
        //    ViewData["selectedReportId"] = Request.Form["selectedReportId"];
        //    ViewData["buttonName"] = "SP";
        //    ViewData["selectedReportName"] = Request.Form["selectedReportName"];
        //    var endUserNames = DB_MSBDW.endusers.Where(eu => eu.ad_active_flag == "Y" && eu.primary_ad_role_id == 2031).Select(eu => eu.full_name).ToList();
        //    ViewModels.ReportsViewModel EndUserNames = new ReportsViewModel() { names = endUserNames };
        //    return View(EndUserNames);
        //}


        [HttpPost]
        public ActionResult AddStoredProcsResult()
        {

            int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            string selectedEuName = Request.Form["CreatedEndUser"];
            DB.ReportSP temp = new DB.ReportSP();
            temp.SSRSReportId = SSRSReportId;
            temp.SPName = Request.Form["SPName"];
            temp.PermissionsNotes = Request.Form["PermissionNotes"];
            temp.CreateEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;
            temp.RowCreateDate = DateTime.Now;
            temp.RowModifyDate = DateTime.Now;
            temp.ModifyEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;
            temp.IsRDLDropdown = false;

            DB_MSBDW.ReportSPs.Add(temp);
            DB_MSBDW.SaveChanges();


            List<DB.ReportSP> reportSP = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == SSRSReportId).OrderByDescending(sp => sp.RowCreateDate).ToList();
            StoredProcsViewModel reportsViewModel = new StoredProcsViewModel(reportSP);
            //ViewData["selectedReportId"] = selectedReport;
            ViewData["selectedReportId"] = SSRSReportId;
            ViewData["buttonName"] = "SP";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            //return View("Index", reportsViewModel.reportSPs);

            return View();
        }

        public class SPToDelete
        {
            public int ID { get; set; }

        }

        public int DeleteSP(SPToDelete sp)
        {
            var de = DB_MSBDW.ReportSPs.FirstOrDefault(s => s.ID == sp.ID);
            DB_MSBDW.ReportSPs.Remove(de);
            DB_MSBDW.SaveChanges();
            return 1;
        }
    }
}