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
      
        public ActionResult Index(int id, string name)
        {
            int selectedReport = id;
            string selectedReportName = name;
            
            
            List<DB.ReportChangeLog> reportChange;
            reportChange = DB_MSBDW.ReportChangeLogs.Where(sp => sp.SSRSReportId == selectedReport).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ChangeHistoryViewModel changeHistoryViewModel = new ChangeHistoryViewModel(reportChange);

            ViewData["selectedReportId"] = selectedReport;
            ViewData["buttonName"] = "CH";
            ViewData["selectedReportName"] = selectedReportName;
            return View(changeHistoryViewModel.reportChangeLogs);
            
        }

        
      

        [HttpPost]
        
        public ActionResult AddChangeHistoryResult()
        {

            int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            string selectedSPName = Request.Form["IsRDLChange"] == "No" ? Request.Form["ReportSPNameValidated"]: Request.Form["ReportSPName"];
            int selectedSPId = selectedSPName == ""?-1:DB_MSBDW.ReportSPs.FirstOrDefault(s => s.SPName == selectedSPName).ID;
            string selectedEuName = Request.Form["CreatedEndUser"];
            
            List<DB.Report_ReportSP> relatedRpts = DB_MSBDW.Report_ReportSP.Where(sp => sp.ReportSPId == selectedSPId).ToList();
            for (int i = 0; i < relatedRpts.Count || Request.Form["IsRDLChange"] == "Yes"; i++)
            {
                DB.ReportChangeLog temp = new DB.ReportChangeLog();
                temp.ITComment = Request.Unvalidated.Form["ITComment"];
                temp.PublicComment = Request.Form["PublicComment"];
                temp.RowCreateDate = DateTime.Now;
                temp.ChangeEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;
                temp.ChangeReason = Request["ChangeReason"];
                
                if (Request.Form["IsRDLChange"] == "Yes")
                {
                    temp.SSRSReportId = SSRSReportId;
                    temp.IsRDLChange = true;
                    if (selectedSPName != "" && selectedSPName != null)
                    {
                        temp.ReportSPId = selectedSPId;
                    }
                    DB_MSBDW.ReportChangeLogs.Add(temp);
                    break;
                }
                temp.SSRSReportId = relatedRpts[i].SSRSReportId;
                temp.ReportSPId = selectedSPId;
                temp.IsRDLChange = false;
                DB_MSBDW.ReportChangeLogs.Add(temp);
                DB_MSBDW.SaveChanges();
            }
            
            
            DB_MSBDW.SaveChanges();
            

            List<DB.ReportChangeLog> reportChange = DB_MSBDW.ReportChangeLogs.Where(sp => sp.SSRSReportId == SSRSReportId).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ChangeHistoryViewModel changeHistoryViewModel = new ChangeHistoryViewModel(reportChange);
            

            ViewData["selectedReportId"] = SSRSReportId;
            ViewData["buttonName"] = "CH";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];

            return RedirectToAction("Index", "ChangeHistory", new { id = SSRSReportId, name = Request.Form["selectedReportName"] });
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