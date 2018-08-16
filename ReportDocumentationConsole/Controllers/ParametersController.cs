using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportDocumentationConsole.ViewModels;

namespace ReportDocumentationConsole.Controllers
{
    public class ParametersController : Controller
    {
        private DB.MSBDWEntities DB_MSBDW = new DB.MSBDWEntities();
        // GET: Parameters
        [HttpPost]
        public ActionResult Index()
        {
            
            
            //int selectedReport = Convert.ToInt32(collection["selectedReportId"]);
            int selectedReport = Convert.ToInt32(Request.Form["selectedReportId"]);
            //string buttonName = Request.Form["buttonName"];
            List<DB.ReportSPParameter> reportPara = DB_MSBDW.ReportSPParameters.Where(sp => sp.SSRSReportId == selectedReport).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ParametersViewModel parametersViewModel = new ParametersViewModel(reportPara);
            ViewData["selectedReportId"] = selectedReport;
            ViewData["buttonName"] = "PA";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            return View(parametersViewModel.reportSPParameters);
        }



        //public ActionResult AddParameters()
        //{
        //    int selectedReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
        //    ViewData["selectedReportId"] = Request.Form["selectedReportId"];
        //    ViewData["buttonName"] = "PA";
        //    ViewData["selectedReportName"] = Request.Form["selectedReportName"];
        //    var spNames = DB_MSBDW.ReportSPs.Where(sp => sp.SSRSReportId == selectedReportId).Select(sp => sp.SPName).ToList();
        //    var endUserNames = DB_MSBDW.endusers.Where(eu => eu.ad_active_flag == "Y" && eu.primary_ad_role_id == 2031).Select(eu => eu.full_name).ToList();
        //    ViewModels.ReportsViewModel spEuNames = new ReportsViewModel() { names = spNames, names2 = endUserNames };
        //    return View(spEuNames);
        //}

        //public ActionResult AddParametersResult(int SSRSReportId, string selectedEuName, string selectedSPName)
        //{
        //    //int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);

        //    //string selectedEuName = Request.Form["CreatedEndUser"];
        //    //string selectedSPName = Request.Form["ReportSPName"];
        //    DB.ReportSPParameter temp = new DB.ReportSPParameter();

        //    temp.ReportSPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == selectedSPName).ID;
        //    temp.SSRSReportId = SSRSReportId;
        //    temp.RDLParameterName = Request.Form["RDLParameterName"];
        //    temp.ParamDescription = Request.Form["ParamDescription"];
        //    temp.IsUserControlled = Request.Form["IsUserControlled"] == "Yes" ? true : false;
        //    temp.UserControlType = Request.Form["UserControlType"];
        //    temp.IsSetValue = Request.Form["IsSetValue"] == "Yes" ? true : false;
        //    temp.DefaultValue = Request.Form["DefaultValue"];
        //    temp.AdditionalInfo = Request.Form["AdditionalInfo"];
        //    temp.RowCreateDate = DateTime.Now;
        //    temp.CreateEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;

        //    Dispose();
        //    DB_MSBDW.ReportSPParameters.Add(temp);
        //    DB_MSBDW.SaveChanges();

        //    return View();
        //}

        [HttpPost]
        public ActionResult AddParametersResult()
        {

            int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            
            string selectedEuName = Request.Form["CreatedEndUser"];
            string selectedSPName = Request.Form["ReportSPName"];
            DB.ReportSPParameter temp = new DB.ReportSPParameter();

            temp.ReportSPId = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.SPName == selectedSPName).ID;
            temp.SSRSReportId = SSRSReportId;
            temp.RDLParameterName = Request.Form["RDLParameterName"];
            temp.ParamDescription = Request.Form["ParamDescription"];
            temp.IsUserControlled = Request.Form["IsUserControlled"] == "Yes" ? true : false;
            temp.UserControlType = Request.Form["UserControlType"];
            temp.IsSetValue = Request.Form["IsSetValue"] == "Yes" ? true : false;
            temp.DefaultValue = Request.Form["DefaultValue"];
            temp.AdditionalInfo = Request.Form["AdditionalInfo"];
            temp.RowCreateDate = DateTime.Now;
            temp.CreateEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;

            //if(!(DB_MSBDW.ReportSPParameters.Any(pa => pa.ReportSPId == temp.ReportSPId)))
            DB_MSBDW.ReportSPParameters.Add(temp);
            DB_MSBDW.SaveChanges();

            //return RedirectToAction("RerenderIndex", new { reportId = SSRSReportId });

            //return RedirectToAction("Index");
            ViewData["selectedReportId"] = SSRSReportId;
            ViewData["buttonName"] = "PA";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];

            return View();
        }

        public ActionResult RerenderIndex(int ReportId)
        {
            List<DB.ReportSPParameter> reportPara = DB_MSBDW.ReportSPParameters.Where(sp => sp.SSRSReportId == ReportId).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ParametersViewModel parametersViewModel = new ParametersViewModel(reportPara);
            //ViewData["selectedReportId"] = selectedReport;
            ViewData["selectedReportId"] = ReportId;
            ViewData["buttonName"] = "PA";
            ViewData["selectedReportName"] = DB_MSBDW.SsrsReports.FirstOrDefault(re => re.id == ReportId).rpt_name;
            return View(parametersViewModel.reportSPParameters);

        }

        public class ParameterToUpdate
        {
            public int ID { get; set; }
            public string FieldValue { get; set; }
            public string ColumnToUpdate { get; set; }
        }
        public int SavePara(ParameterToUpdate para)
        {
            DB.ReportSPParameter pa = DB_MSBDW.ReportSPParameters.FirstOrDefault(l => l.ID == para.ID);
            Boolean input_isUserControlled = true, input_isSetValue = true;
            switch (para.ColumnToUpdate)
            {
                case "RDLParamName":
                    pa.RDLParameterName = para.FieldValue;
                    break;
                case "ParameterDesc":
                    pa.ParamDescription = para.FieldValue;
                    break;
                case "IsUserControlled":
                    var debug = para.FieldValue.ToUpper();
                    if (para.FieldValue.ToUpper() != "TRUE" && para.FieldValue.ToUpper() != "FALSE")
                    {
                        input_isUserControlled = false;
                        break;
                    }
                    pa.IsUserControlled = para.FieldValue.ToUpper() == "TRUE"? true : false;
                    break;
                case "UserControlType":
                    pa.UserControlType = para.FieldValue;
                    break;
                case "IsSetvalue":
                    if (para.FieldValue.ToUpper() != "TRUE" && para.FieldValue.ToUpper() != "FALSE")
                    {
                        input_isSetValue = false;
                        break;
                    }
                    pa.IsSetValue = para.FieldValue.ToUpper() == "TRUE" ? true : false;
                    break;
                case "AdditionalInfo":
                    pa.AdditionalInfo = para.FieldValue;
                    break;

                default:
                    break;
            }
            //db.UpdateCustomers(upd.CustomerID, upd.CustomerFirstName, upd.CustomerLastName);
            if (input_isUserControlled == false || input_isSetValue == false)
            {
                return 0;
            }
            DB_MSBDW.SaveChanges();

            return 1;
        }

        public class ParameterToDelete
        {
            public int ID { get; set; }
           
        }

        public int DeletePara(ParameterToDelete para)
        {
            var de = DB_MSBDW.ReportSPParameters.FirstOrDefault(pa => pa.ID == para.ID );
            DB_MSBDW.ReportSPParameters.Remove(de);
            DB_MSBDW.SaveChanges();
            return 1;
        }

    }
}