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

            var spIds = DB_MSBDW.Report_ReportSP.Where(rsp => rsp.SSRSReportId == selectedReport).Select(rsp => rsp.ReportSPId);
            List<DB.ReportSP> reportSP = DB_MSBDW.ReportSPs.Where(sp => spIds.Contains(sp.ID)).OrderByDescending(sp => sp.RowCreateDate).ToList();
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
            if (Request.Form["UseExisting"] == "Yes")
            {
                string reportSPName = Request.Form["ReportSPName"];
                int reportSPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == reportSPName).ID;
                if (!DB_MSBDW.Report_ReportSP.Any(sp => sp.ReportSPId == reportSPId && sp.SSRSReportId == SSRSReportId))
                { 
                    DB.Report_ReportSP r_sp_temp = new DB.Report_ReportSP();
                    r_sp_temp.ReportSPId = reportSPId;
                    r_sp_temp.SSRSReportId = SSRSReportId;

                    DB_MSBDW.Report_ReportSP.Add(r_sp_temp);
                    DB_MSBDW.SaveChanges();
                }
            }
            else
            {
                
                DB.ReportSP temp = new DB.ReportSP();
                temp.SPName = Request.Form["SPName"];
                temp.PermissionsNotes = Request.Form["PermissionNotes"];
                temp.CreateEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;
                temp.RowCreateDate = DateTime.Now;
                temp.RowModifyDate = DateTime.Now;
                temp.ModifyEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;
                temp.IsRDLDropdown = false;

                DB_MSBDW.ReportSPs.Add(temp);
                DB_MSBDW.SaveChanges();

                DB.Report_ReportSP r_sp_temp = new DB.Report_ReportSP();
                r_sp_temp.ReportSP = temp;
                r_sp_temp.SSRSReport = DB_MSBDW.SSRSReport1.FirstOrDefault(r => r.id == SSRSReportId);

                DB_MSBDW.Report_ReportSP.Add(r_sp_temp);
                DB_MSBDW.SaveChanges();
            }
            

            ViewData["selectedReportId"] = SSRSReportId;
            ViewData["buttonName"] = "SP";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            

            return RedirectToAction("Index", "StoredProcs", new { id = SSRSReportId, name = Request.Form["selectedReportName"] });
        }

        public class SPToDelete
        {
            public int ID { get; set; }
            public Boolean deleteAll { get; set; }
            public int report { get; set; }
        }

        public int DeleteSP(SPToDelete sp)
        {
            if (sp.deleteAll == true)
            {
                var de = DB_MSBDW.ReportSPs.FirstOrDefault(s => s.ID == sp.ID);
                DB_MSBDW.ReportSPs.Remove(de);
                var deCH = DB_MSBDW.ReportChangeLogs.Where(s => s.ReportSPId == sp.ID);
                DB_MSBDW.ReportChangeLogs.RemoveRange(deCH);
                var dePA = DB_MSBDW.ReportSPParameters.Where(s => s.ReportSPId == sp.ID);
                DB_MSBDW.ReportSPParameters.RemoveRange(dePA);
                var deRSP = DB_MSBDW.Report_ReportSP.Where(s => s.ReportSPId == sp.ID );
                DB_MSBDW.Report_ReportSP.RemoveRange(deRSP);
                DB_MSBDW.SaveChanges();

            }
            else {
                var deRSP = DB_MSBDW.Report_ReportSP.Where(s => s.ReportSPId == sp.ID && s.SSRSReportId == sp.report);
                DB_MSBDW.Report_ReportSP.RemoveRange(deRSP);
                DB_MSBDW.SaveChanges();
            }
            

            return 1;
        }
    }
}