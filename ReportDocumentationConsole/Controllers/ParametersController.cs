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
       
        public ActionResult Index(int id, string name)
        {

            int selectedReport = id;
            string selectedReportName = name;

           

            List<DB.ReportSPParameter> reportPara = DB_MSBDW.ReportSPParameters.Where(sp => sp.SSRSReportId == selectedReport).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ParametersViewModel parametersViewModel = new ParametersViewModel(reportPara);
            ViewData["selectedReportId"] = selectedReport;
            ViewData["buttonName"] = "PA";
            ViewData["selectedReportName"] = selectedReportName;
            return View(parametersViewModel.reportSPParameters);
        }


        

        [HttpPost]
        public ActionResult AddParametersResult()
        {

            int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            
            string selectedEuName = Request.Form["CreatedEndUser"];
            string selectedSPName = Request.Form["ReportSPNameValidated"];
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

           
            DB_MSBDW.ReportSPParameters.Add(temp);
            DB_MSBDW.SaveChanges();

          
            ViewData["selectedReportId"] = SSRSReportId;
            ViewData["buttonName"] = "PA";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];

            return RedirectToAction("Index", "Parameters", new { id = SSRSReportId, name = Request.Form["selectedReportName"] });
        }

        public ActionResult RerenderIndex(int ReportId)
        {
            List<DB.ReportSPParameter> reportPara = DB_MSBDW.ReportSPParameters.Where(sp => sp.SSRSReportId == ReportId).OrderByDescending(sp => sp.RowCreateDate).ToList();
            ParametersViewModel parametersViewModel = new ParametersViewModel(reportPara);
           
            ViewData["selectedReportId"] = ReportId;
            ViewData["buttonName"] = "PA";
            ViewData["selectedReportName"] = DB_MSBDW.SSRSReport1.FirstOrDefault(re => re.id == ReportId).rpt_name;
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