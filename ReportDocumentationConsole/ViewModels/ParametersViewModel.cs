using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportDocumentationConsole.ViewModels
{
    public class ParametersViewModel
    {
        public List<ReportSPParameter> reportSPParameters { get; set; }

        public ParametersViewModel(List<DB.ReportSPParameter> rps)
        {
            reportSPParameters = new List<ReportSPParameter>();
            DB.MSBDWEntities DB_MSBDW = new DB.MSBDWEntities();
            foreach (var rp in rps)
            {
                ReportSPParameter temp = new ReportSPParameter();
                temp.ID = rp.ID;
                try { temp.SPName = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.ID == rp.ReportSPId).SPName; }
                catch (Exception)
                { }
                
                temp.RDLParamName = rp.RDLParameterName;
                temp.ParameterDesc = rp.ParamDescription;
                temp.IsUserControlled = rp.IsUserControlled == true ? "True" : "False";
                temp.UserControlType = rp.UserControlType;
                temp.IsSetvalue = rp.IsSetValue == true ? "True" : "False";
                temp.DefaultValue =rp.DefaultValue;
                temp.AdditionalInfo = rp.AdditionalInfo;
                temp.RowCreateDate = Convert.ToDateTime(rp.RowCreateDate);
                temp.CreateEndUserName = rp.CreateEnduserId == null ? "":DB_MSBDW.endusers.FirstOrDefault(eu => eu.id == rp.CreateEnduserId).full_name;
                reportSPParameters.Add(temp);
            }
        }
    }
    public class ReportSPParameter
    {
        public int ID { get; set; }
        public string SPName { get; set; }
        public string RDLParamName { get; set; }
        public string ParameterDesc { get; set; }
        public string IsUserControlled { get; set; }
        public string UserControlType { get; set; }
        public string IsSetvalue { get; set; }
        public string DefaultValue { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime RowCreateDate { get; set; }
        public string CreateEndUserName { get; set; }

    }
}