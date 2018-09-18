using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using ReportDocumentationConsole.DB;

namespace ReportDocumentationConsole.ViewModels
{
    public class ChangeHistoryViewModel
    {
        public List<ReportChangeLog> reportChangeLogs { get; set; }

        public ChangeHistoryViewModel(List<DB.ReportChangeLog> res)
        {
            DB.MSBDWEntities DB_MSBDW = new MSBDWEntities();
            reportChangeLogs = new List<ReportChangeLog>();
            foreach (var re in res)
            {
                ReportChangeLog temp = new ReportChangeLog();
                temp.ID = re.ID;
                temp.ItComment = re.ITComment;
                temp.PublicComment = re.PublicComment;
                temp.RowCreateDate = Convert.ToDateTime(re.RowCreateDate);
                temp.IsRDLChange = Convert.ToBoolean(re.IsRDLChange);
                temp.ChangeEndUserName = re.ChangeEnduserId == null ? "" : DB_MSBDW.endusers.FirstOrDefault(eu => eu.id == re.ChangeEnduserId).full_name;
                temp.ChangeReason = re.ChangeReason;
                try { temp.ReportSPName = DB_MSBDW.ReportSPs.FirstOrDefault(sp => sp.ID == re.ReportSPId).SPName; }
                catch (Exception e)
                {
                }
                
                reportChangeLogs.Add(temp);
            }
            
        }

       
    }

    public class ReportChangeLog
    {
        public int ID { get; set; }
        [DisplayName("Name")]
        public string ReportSPName { get; set; }

        [DisplayName("Create Date")]
        public DateTime RowCreateDate { get; set; }

        [DisplayName("Internal Comment")]
        public string ItComment { get; set; }
        [DisplayName("Public Comment")]
        public string PublicComment { get; set; }
        [DisplayName("RDL Changed? ")]
        public bool IsRDLChange { get; set; }
        [DisplayName("Change EndUser")]
        public string ChangeEndUserName { get; set; }
        [DisplayName("Change Reason")]
        public string  ChangeReason { get; set; }
    }

}