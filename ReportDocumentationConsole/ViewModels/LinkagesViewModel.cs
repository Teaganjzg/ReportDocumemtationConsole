using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ReportDocumentationConsole.ViewModels
{
    public class LinkagesViewModel
    {
        public List<ReportLinkage> reportLinkages { get; set; }

        public LinkagesViewModel(List<DB.ReportLinkage> rls)
        {
            reportLinkages = new List<ReportLinkage>();
            DB.MSBDWEntities Db_MSBDW = new DB.MSBDWEntities();
            foreach (var rl in rls)
            {
                ReportLinkage temp = new ReportLinkage();
                temp.ID = rl.ID;
                temp.LinkageType = rl.LinkageType;
                temp.LinkageLocation = rl.LinkLocation;
                temp.RowCreateDate = Convert.ToDateTime(rl.RowCreateDate);
                temp.CreateEndUserName = rl.CreateEnduserId == null ? "":Db_MSBDW.endusers.FirstOrDefault(eu => eu.id == rl.CreateEnduserId).full_name;
                reportLinkages.Add(temp);
            }

        }
    }

    public class ReportLinkage
    {
        public int ID { get; set; }
        [DisplayName("Linkage Type")]
        public string LinkageType { get; set; }
        [DisplayName("Linkage Location")]
        public string LinkageLocation { get; set; }
        [DisplayName("Create Date")]
        public DateTime RowCreateDate { get; set; }
        [DisplayName("Create EndUser")]
        public string CreateEndUserName { get; set; }
        
    }
}