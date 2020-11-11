using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WorkScheduleSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 班表查詢
            routes.MapRoute(
                "shiftSystem", // Route name
                "shiftSystem/search", // Route pattern
                new { controller = "ShiftService", action = "Index" } // Default values for defined parameters above
            );

            // 班表寫入API
            routes.MapRoute(
                "shiftSchedule", // Route name
                "shiftSchedule/", // Route pattern
                new { controller = "ShiftService", action = "Index" } // Default values for defined parameters above
            );

            // Normal
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
