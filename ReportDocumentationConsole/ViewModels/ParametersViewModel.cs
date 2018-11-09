using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                temp.IsUserControlled = rp.IsUserControlled != null?(rp.IsUserControlled == true ? "User Controlled" : "Set Value"): null;
                temp.UserControlType = rp.UserControlType;
                temp.IsSetvalue = rp.IsSetValue != null?(rp.IsSetValue == true ? "True" : "False"):null;
                temp.DefaultValue = rp.DefaultValue;
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
        [DisplayName("Name")]
        public string SPName { get; set; }
        [DisplayName("Parameter Name")]
        public string RDLParamName { get; set; }
        [DisplayName("Description")]
        public string ParameterDesc { get; set; }
        [DisplayName("User Controlled?")]
        public string IsUserControlled { get; set; }
        [DisplayName("User Control Type")]
        public string UserControlType { get; set; }
        [DisplayName("Set value?")]
        public string IsSetvalue { get; set; }
        [DisplayName("Default Value")]
        public string DefaultValue { get; set; }
        [DisplayName("Additional Info")]
        public string AdditionalInfo { get; set; }
        [DisplayName("Create Date")]
        public DateTime RowCreateDate { get; set; }
        [DisplayName("Create EndUser")]
        public string CreateEndUserName { get; set; }

    }
}