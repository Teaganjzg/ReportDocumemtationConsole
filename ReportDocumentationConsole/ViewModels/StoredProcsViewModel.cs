using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportDocumentationConsole.ViewModels
{
    public class StoredProcsViewModel
    {
         
        public List<ReportSP> reportSPs { get; set; }
        public StoredProcsViewModel(List<DB.ReportSP> sPs)
        {
            reportSPs = new List<ReportSP>();
            DB.MSBDWEntities DB_MSBDW = new DB.MSBDWEntities();
            
            foreach (var sp in sPs)
            {
                ReportSP temp = new ReportSP();
                temp.ID = sp.ID;
                temp.SPName = sp.SPName;
                temp.IsRDLDropdown = Convert.ToBoolean(sp.IsRDLDropdown);
                temp.PermissionNotes = sp.PermissionsNotes;
                temp.CreateEndUserName = sp.CreateEnduserId == null ? "": DB_MSBDW.endusers.FirstOrDefault(eu => eu.id == sp.CreateEnduserId).full_name;
                temp.RowCreateDate = Convert.ToDateTime(sp.RowModifyDate);
                temp.ModifyEndUserName = sp.ModifyEnduserId == null ? "":DB_MSBDW.endusers.FirstOrDefault(eu => eu.id == sp.ModifyEnduserId).full_name;
                reportSPs.Add(temp);
            }

        }
    }

    public class ReportSP
    {
        public int ID { get; set; }
        public string SPName { get; set; }
        public bool IsRDLDropdown { get; set; }
        public string PermissionNotes { get; set; }
        public DateTime RowCreateDate { get; set; }
        public string CreateEndUserName { get; set; }
        public DateTime RowModifyDate { get; set; }
        public string ModifyEndUserName { get; set; }
    }
}