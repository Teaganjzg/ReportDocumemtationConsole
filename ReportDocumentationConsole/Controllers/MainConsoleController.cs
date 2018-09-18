using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportDocumentationConsole.ViewModels;

namespace ReportDocumentationConsole.Controllers
{
    public class MainConsoleController : Controller
    {

        // GET: MainConsole
        //public ActionResult Index()
        //{
        //    return View();
        //}
        DB.MSBDWEntities DB_MSBDW = new DB.MSBDWEntities();

        [HttpPost]
        public ActionResult Index()
        {
            int selectedReport;
            string SelectedReportName;

            if (Request.Form["selectedReportName"] != null && Request.Form["selectedReportName"] != "select report" && Request.Form["selectedReportName"] != "")
            {
                SelectedReportName = Request.Form["selectedReportName"];
                selectedReport = DB_MSBDW.SsrsReports.FirstOrDefault(r => r.rpt_name == SelectedReportName).id;

            }
            else
            {
                
                return RedirectToAction("Index", "Home");
            }

            if (Request.Form["ReportDescription"] != null)
            {
                DB.SsrsReport report = DB_MSBDW.SsrsReports.FirstOrDefault(re => re.id == selectedReport);
                report.rpt_desc = Request.Form["ReportDescription"];
                DB_MSBDW.SaveChanges();

            }

            if (Request.Form["buttonName"] == "SP")
            {
                return RedirectToAction("Index", "StoredProcs", new { id = selectedReport, name = SelectedReportName });
            }
            else if (Request.Form["buttonName"] == "LK")
            {
                return RedirectToAction("Index", "Linkages", new { id = selectedReport, name = SelectedReportName });
            }
            else if (Request.Form["buttonName"] == "PA")
            {
                return RedirectToAction("Index", "Parameters", new { id = selectedReport, name = SelectedReportName });
            }
            else
            { return RedirectToAction("Index", "ChangeHistory", new { id = selectedReport, name = SelectedReportName }); }


                
        }

        

       

        [ChildActionOnly]
        public ActionResult _SPNamesList(string selectedReportId)
        {
            int sReportId = Convert.ToInt32(selectedReportId);
            var spIds = DB_MSBDW.Report_ReportSP.Where(rsp => rsp.SSRSReportId == sReportId).Select(rsp => rsp.ReportSPId);
            //var spNames = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == sReportId).Select(sp => sp.SPName).ToList();
            var spNames = DB_MSBDW.ReportSPs.Where(sp => spIds.Contains(sp.ID)).Select(sp => sp.SPName).ToList();
            ViewModels.ReportsViewModel spnames = new ReportsViewModel() { names = spNames };
            return PartialView(spnames);
        }

        [ChildActionOnly]
        public ActionResult _AllSPNames()
        {
           
            var spNames = DB_MSBDW.ReportSPs.Where(sp => sp.SPName != null).OrderBy(sp => sp.SPName).Select(sp => sp.SPName).ToList();
            ViewModels.ReportsViewModel spnames = new ReportsViewModel() { names = spNames };
            return PartialView(spnames);
        }

        [ChildActionOnly]
        public ActionResult _EndUserNamesList()
        {
            
            var endUserNames = DB_MSBDW.endusers.Where(eu => eu.ad_active_flag == "Y" && eu.primary_ad_role_id == 2031).Select(eu => eu.full_name).ToList();
            ViewModels.ReportsViewModel EndUserNames = new ReportsViewModel() { names = endUserNames };
            return PartialView(EndUserNames);
            
        }

        [ChildActionOnly]
        public ActionResult _ReportDescription(string selectedReportId)
        {
            int sReportId = Convert.ToInt32(selectedReportId);
            
            string reportDesc = DB_MSBDW.SsrsReports.FirstOrDefault(re => re.id == sReportId).rpt_desc;
            ViewData["ReportDescription"]= reportDesc;
            ViewData["selectedReportName"] = DB_MSBDW.SsrsReports.FirstOrDefault(re => re.id == sReportId).rpt_name;
            return PartialView();
        }
        [HttpPost]
        public ActionResult SaveReportDesc()
        {
            string test = Request.Form["selectedReportName"];
            int reportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            DB.SsrsReport report = DB_MSBDW.SsrsReports.FirstOrDefault(re => re.id == reportId);
            report.rpt_desc = Request.Form["ReportDescription"];
            DB_MSBDW.SaveChanges();
            
            ViewData["ReportDescription"] = Request.Form["ReportDescription"];
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            return PartialView("_ReportDescription");
        }

        
        public ActionResult RelatedReports(string SPName)
        {
            int SPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == SPName).ID;
            var relatedReports = DB_MSBDW.Report_ReportSP.Where(re => re.ReportSPId == SPId).Select(re => re.SSRSReportId).ToList();
            var relatedReportNames = DB_MSBDW.SsrsReports.Where(r => relatedReports.Contains(r.id)).Select(r => r.rpt_name).ToList();
            ViewModels.ReportsViewModel reportNames = new ReportsViewModel() { names = relatedReportNames };
            return View(reportNames);
        }



    }
}