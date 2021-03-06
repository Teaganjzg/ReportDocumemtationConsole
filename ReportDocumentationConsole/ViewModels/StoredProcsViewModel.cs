﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ReportDocumentationConsole.ViewModels
{
    public class StoredProcsViewModel
    {
         
        public List<ReportSP> reportSPs { get; set; }
       
        public StoredProcsViewModel(List<DB.ReportSP> sPs, int ReportId)
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
                temp.RowCreateDate = Convert.ToDateTime(sp.RowCreateDate);
                temp.ModifyEndUserName = sp.ModifyEnduserId == null ? "":DB_MSBDW.endusers.FirstOrDefault(eu => eu.id == sp.ModifyEnduserId).full_name;
                temp.RowModifyDate = Convert.ToDateTime(sp.RowModifyDate);
                temp.LinkedDate = Convert.ToDateTime(DB_MSBDW.Report_ReportSP.FirstOrDefault(s => s.ReportSPId == sp.ID && s.SSRSReportId == ReportId).RowCreateDate);
                reportSPs.Add(temp);
            }

        }
    }

    public class ReportSP
    {
        public int ID { get; set; }
        [DisplayName("Name")]
        public string SPName { get; set; }
        [DisplayName("RDL Dropdown?")]
        public bool IsRDLDropdown { get; set; }
        [DisplayName("Permission Notes")]
        public string PermissionNotes { get; set; }
        [DisplayName("Create Date")]
        public DateTime RowCreateDate { get; set; }
        [DisplayName("Create EndUser")]
        public string CreateEndUserName { get; set; }
        [DisplayName("Modify Date")]
        public DateTime RowModifyDate { get; set; }
        [DisplayName("Modify EndUser")]
        public string ModifyEndUserName { get; set; }
        [DisplayName("Linked Date")]
        public DateTime LinkedDate { get; set; }
    }
}