using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportDocumentationConsole.ViewModels;
using ReportDocumentationConsole.DB;

namespace ReportDocumentationConsole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
            

        }

        [ChildActionOnly]
        public ActionResult _DropDownList(string selectedReportName)
        {
            DB.MSBDWEntities DB_MSDBW = new DB.MSBDWEntities();
            var re = DB_MSDBW.SSRSReport1.Where(r => r.rpt_name != null).OrderBy(r => r.rpt_name).Select(r => r.rpt_name).ToList();
            
            ReportsViewModel reports = new ReportsViewModel() { names = re };
            ViewData["selectedReportName"] = selectedReportName;
            return PartialView("_DropDownList",reports);

        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}