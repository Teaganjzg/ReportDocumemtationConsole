using System;
using System.Collections.Generic;
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
        public string LinkageType { get; set; }
        public string LinkageLocation { get; set; }
        public DateTime RowCreateDate { get; set; }
        public string CreateEndUserName { get; set; }
        
    }
}