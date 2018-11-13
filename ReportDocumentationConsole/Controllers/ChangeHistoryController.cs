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
            List<int> UnSelectedRepIds = new List<int>();

            List<DB.Report_ReportSP> relatedRpts = new List<DB.Report_ReportSP>();
            int selectedSPId = -1;
            if (selectedSPName != "" && selectedSPName != null)
            {
                selectedSPId = DB_MSBDW.ReportSPs.FirstOrDefault(s => s.SPName == selectedSPName).ID;
                relatedRpts = DB_MSBDW.Report_ReportSP.Where(sp => sp.ReportSPId == selectedSPId).Distinct().ToList();
                string cbl1 = "checkboxlist[", cbl2 = "].id", cbl3 = "].isselected";
                
                if (Request.Form[cbl1 + "0" + cbl3] != null)
                {
                    for (int i = 0; i < relatedRpts.Count - 1; i++)
                    {
                        if (!Request.Form[cbl1 + i.ToString() + cbl3].Contains("true".ToLower()))
                        {
                            UnSelectedRepIds.Add(Convert.ToInt32(Request.Form[cbl1 + i.ToString() + cbl2]));
                        }
                    }

                    
                }
                
            }
            string selectedEuName = Request.Form["CreatedEndUser"];
            
             for (int i = 0; i < relatedRpts.Count || (Request.Form["IsRDLChange"] == "Yes" && relatedRpts.Count == 0); i++) // both with SP selected(at least one in relatedreports list, including "Yes" + SP and "No" + SP) and "Yes" + no SP  can go through, and prevent "Yes" + SP index out of range exception
            {
                
                DB.ReportChangeLog temp = new DB.ReportChangeLog();
                temp.ITComment = Request.Unvalidated.Form["ITComment"];
                temp.PublicComment = Request.Form["PublicComment"];
                temp.RowCreateDate = DateTime.Now;
                temp.ChangeEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;
                temp.ChangeReason = Request["ChangeReason"];
                if (selectedSPId == -1 && Request.Form["IsRDLChange"] == "Yes") // For IsRdlChange is yes and no SP selected, break from the loop 
                {
                    temp.SSRSReportId = SSRSReportId;
                    temp.IsRDLChange = true;
                    DB_MSBDW.ReportChangeLogs.Add(temp);
                    DB_MSBDW.SaveChanges();
                    break;
                }
                temp.ReportSPId = selectedSPId;
                
                temp.SSRSReportId = relatedRpts[i].SSRSReportId;
                if (Request.Form["IsRDLChange"] == "Yes" && !UnSelectedRepIds.Contains((int)relatedRpts[i].SSRSReportId)) // Only the selected reports(from checkboxlist) and current report will get flagged  isrdgchange true when user selects IsRDlChange Yes
                {
                    temp.IsRDLChange = true;
                }
                else
                {
                    temp.IsRDLChange = false;
                }

                
                
                
                DB_MSBDW.ReportChangeLogs.Add(temp);
                DB_MSBDW.SaveChanges();
            }
            
            
          
            

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