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

        [ChildActionOnly]
        public ActionResult _SPNamesList(string selectedReportId)
        {
            int sReportId = Convert.ToInt32(selectedReportId);
            
            var spNames = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == sReportId).Select(sp => sp.SPName).ToList();
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
            //TempData["selectedReportName"] = Request.Form["selectedReportName"];
            //return RedirectToAction("Index1", "ChangeHistory");
            //ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            //return View();
            ViewData["ReportDescription"] = Request.Form["ReportDescription"];
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            return PartialView("_ReportDescription");
        }
        



    }
}