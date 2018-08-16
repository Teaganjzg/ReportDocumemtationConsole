using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportDocumentationConsole.ViewModels;

namespace ReportDocumentationConsole.Controllers
{
    public class ChangeHistoryController : Controller
    {

        // GET: ChangeHistory
        private DB.MSBDWEntities DB_MSBDW = new DB.MSBDWEntities();
        [HttpPost]
        public ActionResult Index()
        {
            int selectedReport;
            //if (Request.Form["isAdd"] == "true")
            //{
            //    int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            //    string selectedSPName = Request.Form["ReportSPName"];

            //    DB.ReportChangeLog temp = new DB.ReportChangeLog();
            //    temp.SSRSReportId = SSRSReportId;
            //    temp.ReportSPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == selectedSPName).ID;
            //    temp.ITComment = Request.Form["ITComment"];
            //    temp.PublicComment = Request.Form["PublicComment"];
            //    temp.RowCreateDate = DateTime.Now;
            //    temp.IsRDLChange = true;

            //    DB_MSBDW.ReportChangeLogs.Add(temp);
            //    DB_MSBDW.SaveChanges();


            //    //List<DB.ReportSP> reportSP = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == selectedReport).ToList();
                
            //}
            
            
            if (Request.Form["selectedReportName"] != null && Request.Form["selectedReportName"] != "select report" && Request.Form["selectedReportName"] != "")
            {
                string SelectedReportName = Request.Form["selectedReportName"];
                selectedReport = DB_MSBDW.SsrsReports.FirstOrDefault(r => r.rpt_name == SelectedReportName).id;

            }
            else
            {
                //selectedReport = Convert.ToInt32(Request.Form["selectedReportId"]);
                return RedirectToAction("Index", "Home");
            }

            if (Request.Form["ReportDescription"] != null && Request.Form["ReportDescription"] != "")
            {
                DB.SsrsReport report = DB_MSBDW.SsrsReports.FirstOrDefault(re => re.id == selectedReport);
                report.rpt_desc = Request.Form["ReportDescription"];
                DB_MSBDW.SaveChanges();

            }



            //buttonName = Request.Form["buttonName"];
            List<DB.ReportChangeLog> reportChange;
            reportChange = DB_MSBDW.ReportChangeLogs.Where(sp => sp.SSRSReportId == selectedReport).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ChangeHistoryViewModel changeHistoryViewModel = new ChangeHistoryViewModel(reportChange);

            ViewData["selectedReportId"] = selectedReport;
            ViewData["buttonName"] = "CH";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            return View(changeHistoryViewModel.reportChangeLogs);
            
        }

        //public ActionResult AddChangeHistory()
        //{
        //    int selectedReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
        //    ViewData["selectedReportId"] = Request.Form["selectedReportId"];
        //    ViewData["buttonName"] = "CH";
        //    ViewData["selectedReportName"] = Request.Form["selectedReportName"];
        //    //List<string> reportSPs = new List<string>();
        //    var spNames = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == selectedReportId).Select(sp => sp.SPName).ToList();
        //    ViewModels.ReportsViewModel spnames = new ReportsViewModel() { names = spNames };
        //    return View(spnames);
        //}
      

        [HttpPost]
        
        public ActionResult AddChangeHistoryResult()
        {

            int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            string selectedSPName = Request.Form["ReportSPName"];

            DB.ReportChangeLog temp = new DB.ReportChangeLog();
            temp.SSRSReportId = SSRSReportId;
            temp.ReportSPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == selectedSPName).ID;
            temp.ITComment = Request.Unvalidated.Form["ITComment"];
            temp.PublicComment = Request.Form["PublicComment"];
            temp.RowCreateDate = DateTime.Now;
            temp.IsRDLChange = true;

            DB_MSBDW.ReportChangeLogs.Add(temp);
            DB_MSBDW.SaveChanges();


            //List<DB.ReportSP> reportSP = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == selectedReport).ToList();
            List<DB.ReportChangeLog> reportChange = DB_MSBDW.ReportChangeLogs.Where(sp => sp.SSRSReportId == SSRSReportId).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ChangeHistoryViewModel changeHistoryViewModel = new ChangeHistoryViewModel(reportChange);

            //ViewData["selectedReportId"] = selectedReport;
            ViewData["selectedReportId"] = SSRSReportId;
            ViewData["buttonName"] = "CH";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            //return View("Index", changeHistoryViewModel.reportChangeLogs);
            //return View(criterias);
            return View();
        }
        public class ChangeHistoryToDelete
        {
            public int ID { get; set; }

        }

        public int DeleteCH(ChangeHistoryToDelete para)
        {
            var de = DB_MSBDW.ReportChangeLogs.FirstOrDefault(pa => pa.ID == para.ID);
            DB_MSBDW.ReportChangeLogs.Remove(de);
            DB_MSBDW.SaveChanges();
            return 1;
        }
    }
}