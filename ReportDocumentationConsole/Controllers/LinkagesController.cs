﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportDocumentationConsole.ViewModels;

namespace ReportDocumentationConsole.Controllers
{
    public class LinkagesController : Controller
    {
        private DB.MSBDWEntities DB_MSBDW = new DB.MSBDWEntities();
        // GET: Linkages
        
        public ActionResult Index(int id, string name)
        {
            int selectedReport = id;
            string selectedReportName = name;
            
            
            List<DB.ReportLinkage> reportLink = DB_MSBDW.ReportLinkages.Where(sp => sp.SSRSReportId == selectedReport).OrderByDescending(sp => sp.RowCreateDate).ToList();
            LinkagesViewModel storedProcsViewModel = new LinkagesViewModel(reportLink);

            ViewData["selectedReportId"] = selectedReport;
            ViewData["buttonName"] = "LK";
            ViewData["selectedReportName"] = selectedReportName;
            return View(storedProcsViewModel.reportLinkages);


        }

        
        [HttpPost]
        
        public ActionResult AddLinkagesResult()
        {

            int SSRSReportId = Convert.ToInt32(Request.Form["selectedReportId"]);
            string selectedEuName = Request.Form["CreatedEndUser"];
            DB.ReportLinkage temp = new DB.ReportLinkage();

            temp.SSRSReportId = SSRSReportId;
            temp.LinkageType = Request.Form["LinkageType"];
            temp.LinkLocation = Request.Form["LinkLocation"];
            temp.CreateEnduserId = DB_MSBDW.endusers.FirstOrDefault(eu => eu.full_name == selectedEuName).id;
            temp.RowCreateDate = DateTime.Now;


            DB_MSBDW.ReportLinkages.Add(temp);
            DB_MSBDW.SaveChanges();


            List<DB.ReportLinkage> reportLink = DB_MSBDW.ReportLinkages.Where(sp => sp.SSRSReportId == SSRSReportId).OrderByDescending(sp => sp.RowCreateDate).ToList();
            LinkagesViewModel storedProcsViewModel = new LinkagesViewModel(reportLink);
           
            ViewData["selectedReportId"] = SSRSReportId;
            ViewData["buttonName"] = "LK";
            ViewData["selectedReportName"] = Request.Form["selectedReportName"];
            
            return RedirectToAction("Index", "Linkages", new { id = SSRSReportId, name = Request.Form["selectedReportName"] });
        }

        public class LinkageToUpdate
        {
            public int ID { get; set; }
            public string FieldValue { get; set; }
            public string ColumnToUpdate { get; set; }
        }


      
        public int SaveLinkage(LinkageToUpdate linkage)
        {
            DB.ReportLinkage lk = DB_MSBDW.ReportLinkages.FirstOrDefault(l => l.ID == linkage.ID);
            switch (linkage.ColumnToUpdate)
            {
                case "LinkageLocation":
                    lk.LinkLocation = linkage.FieldValue;
                    break;

                default:
                    break;
            }
           
            DB_MSBDW.SaveChanges();
            return 1;
        }

        public class LinkageToDelete
        {
            public int ID { get; set; }

        }

        public int DeleteLK(LinkageToDelete link)
        {
            var de = DB_MSBDW.ReportLinkages.FirstOrDefault(pa => pa.ID == link.ID);
            DB_MSBDW.ReportLinkages.Remove(de);
            DB_MSBDW.SaveChanges();
            return 1;
        }
    }
}