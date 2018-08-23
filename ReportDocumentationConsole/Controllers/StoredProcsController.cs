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
       
        public ActionResult Index(int id, string name)
        {
            int selectedReport = id;
            string selectedReportName = name;

            
           //int selectedReport = Convert.ToInt32(Request.Form["selectedReportId"]);
           
            List<DB.ReportSP> reportSP = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == selectedReport).OrderByDescending(sp => sp.RowCreateDate).ToList();
            StoredProcsViewModel reportsViewModel = new StoredProcsViewModel(reportSP);
            ViewData["selectedReportId"] = selectedReport;
            ViewData["buttonName"] = "SP";
            ViewData["selectedReportName"] = selectedReportName;
            return View(reportsViewModel.reportSPs);
        }


        


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

            return RedirectToAction("Index", "StoredProcs", new { id = SSRSReportId, name = Request.Form["selectedReportName"] });
        }

        public class SPToDelete
        {
            public int ID { get; set; }

        }

        public int DeleteSP(SPToDelete sp)
        {
            var de = DB_MSBDW.ReportSPs.FirstOrDefault(s => s.ID == sp.ID);
            DB_MSBDW.ReportSPs.Remove(de);
            var deCH = DB_MSBDW.ReportChangeLogs.Where(s => s.ReportSPId == sp.ID);
            DB_MSBDW.ReportChangeLogs.RemoveRange(deCH);
            var dePA = DB_MSBDW.ReportSPParameters.Where(s => s.ReportSPId == sp.ID);
            DB_MSBDW.ReportSPParameters.RemoveRange(dePA);
            DB_MSBDW.SaveChanges();
            return 1;
        }
    }
}