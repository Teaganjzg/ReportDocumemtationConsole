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
                selectedReport = DB_MSBDW.SSRSReport1.FirstOrDefault(r => r.rpt_name == SelectedReportName).id;

            }
            else
            {

                return RedirectToAction("Index", "Home");
            }

            if (Request.Form["ReportDescription"] != null)
            {
                DB.SSRSReport1 report = DB_MSBDW.SSRSReport1.FirstOrDefault(re => re.id == selectedReport);
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

        public ActionResult AddReportForm()
        {
            return View();
        }

        [HttpPost]

        public ActionResult AddReport()
        {
            DB.SSRSReport1 temp = new DB.SSRSReport1();
            temp.rpt_name = Request.Form["ReportName"];
            temp.rpt_desc = Request.Form["ReportDescription"];
            string endUserName = Request.Form["CreatedEndUser"];
            temp.OwnerEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == endUserName).id;
            temp.RdlName = Request.Form["RDLName"];
            temp.create_date = DateTime.Now;
            temp.reportcat_id = 0;
            temp.ReportLogId = Convert.ToInt32(Request.Form["ReportLogId"]);
            DB.Report_ReportSP r_sp_temp = new DB.Report_ReportSP();
            if (Request.Form["UseExisting"] == "Yes")
            {
                temp.sp_name = Request.Form["ReportSPName"];

                DB_MSBDW.SSRSReport1.Add(temp);
                DB_MSBDW.SaveChanges();

                int SpId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == temp.sp_name).ID;
                r_sp_temp.ReportSPId = SpId;


            }
            else
            {
                DB.ReportSP SP = new DB.ReportSP();
                SP.SPName = Request.Form["SPName"];
                SP.PermissionsNotes = Request.Form["PermissionNotes"];
                SP.CreateEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == endUserName).id;
                SP.RowCreateDate = DateTime.Now;
                SP.RowModifyDate = DateTime.Now;
                SP.ModifyEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == endUserName).id;
                SP.IsRDLDropdown = false;

                DB_MSBDW.ReportSPs.Add(SP);
                DB_MSBDW.SaveChanges();

                temp.sp_name = SP.SPName;
                DB_MSBDW.SSRSReport1.Add(temp);
                DB_MSBDW.SaveChanges();

                r_sp_temp.ReportSP = SP;
                
            }

            r_sp_temp.SSRSReport = temp;
            r_sp_temp.RowCreateDate = DateTime.Now;

            DB_MSBDW.Report_ReportSP.Add(r_sp_temp);
            DB_MSBDW.SaveChanges();


            return RedirectToAction("Index", "ChangeHistory", new { id = r_sp_temp.SSRSReportId, name = temp.rpt_name });
        }



        [ChildActionOnly]
        public ActionResult _SPNamesList(string selectedReportId)
        {
            int sReportId = Convert.ToInt32(selectedReportId);
            var spIds = DB_MSBDW.Report_ReportSP.Where(rsp => rsp.SSRSReportId == sReportId).Select(rsp => rsp.ReportSPId);
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

            string reportDesc = DB_MSBDW.SSRSReport1.FirstOrDefault(re => re.id == sReportId).rpt_desc;
            ViewData["ReportDescription"] = reportDesc;
            ViewData["selectedReportName"] = DB_MSBDW.SSRSReport1.FirstOrDefault(re => re.id == sReportId).rpt_name;
            return PartialView();
        }
        [HttpPost]
        public ActionResult SaveReportDesc()
        {
            string test = Request.Form["selectedReportName"];
            int reportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            DB.SSRSReport1 report = DB_MSBDW.SSRSReport1.FirstOrDefault(re => re.id == reportId);
            report.rpt_desc = Request.Form["ReportDescription"];
            DB_MSBDW.SaveChanges();

            ViewData["ReportDescription"] = Request.Form["ReportDescription"];
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            return PartialView("_ReportDescription");
        }


        public ActionResult ShowRelatedReports(string SPName)
        {
            if (DB_MSBDW.ReportSPs.Any(s => s.SPName == SPName))
            {
                int SPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == SPName).ID;
                var relatedReports = DB_MSBDW.Report_ReportSP.Where(re => re.ReportSPId == SPId).Select(re => re.SSRSReportId).ToList();
                var relatedReportNames = DB_MSBDW.SSRSReport1.Where(r => relatedReports.Contains(r.id)).Select(r => r.rpt_name).ToList();
                ViewModels.ReportsViewModel reportNames = new ReportsViewModel() { names = relatedReportNames };
                return View(reportNames);
            }
            else
            {
                List<string> noI = new List<string> { "no related reports", "************" };

                ViewModels.ReportsViewModel noInput = new ReportsViewModel() { names = noI };
                return View(noInput);

            }

        }


        [HttpPost]
        public ActionResult _RelatedReports(string SPName, string CurrentReportId)
        {
            ViewModels.CheckboxList relRepList = new CheckboxList();
            if (!(SPName == null || SPName == "")) // if no SP selected  then don't return checkboxlist( if is rdl change is no then this method won't be called)
            {
                if (DB_MSBDW.ReportSPs.Any(s => s.SPName == SPName))
                {
                    int SPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == SPName).ID;
                    var relatedReportsId = DB_MSBDW.Report_ReportSP.Where(re => re.ReportSPId == SPId).Select(re => re.SSRSReportId).Distinct().ToList();
                    int rId = Convert.ToInt32(CurrentReportId);
                    relatedReportsId.Remove(rId);
                    var relatedReports = DB_MSBDW.SSRSReport1.Where(r => relatedReportsId.Contains(r.id)).OrderBy(r => r.rpt_name).ToList();

                    foreach (var r in relatedReports)
                    {
                        ViewModels.CheckboxItem temp = new CheckboxItem();
                        temp.Id = r.id;
                        temp.Name = r.rpt_name;
                        temp.IsSelected = false;
                        relRepList.checkboxList.Add(temp);
                    }

                }

                relRepList.info = "All Related reports will reflect this change. Select additional reports that had their RDLs altered";

            }
            return PartialView(relRepList);

        }



    }
}