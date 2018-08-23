﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReportDocumentationConsole
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChangeHistoryDIndex",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ChangeHistory", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //     name: "ChangeHistoryDelete",
            //     url: "{controller}/{action}/{id}",
            //     defaults: new { controller = "ChangeHistory", action = "DeleteCH", id = UrlParameter.Optional }
            // );
            //routes.MapRoute(
            //    name: "ParameterDefault",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Parameter", action = "Index", id = UrlParameter.Optional }
            //);
            //routes.MapRoute(
            //    name: "ParameterReload",
            //    url: "{controller}/{action}/{isRefresh}",
            //    defaults: new { controller = "Parameter", action = "Index", isRefresh = "true" }
            //);


            //routes.MapRoute(
            //    name: "MainConsoleHome",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "MainConsole", action = "ChangeHistory", id = UrlParameter.Optional }
            //);


        }
    }
}
