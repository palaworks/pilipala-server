using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PILIPALA
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "List",
                url: "",
                defaults: new { controller = "Panel", action = "List", ajax = false }
            );
            routes.MapRoute(
                name: "Content",
                url: "{ID}",
                defaults: new { controller = "Panel", action = "Content", ajax = false }
            );

            routes.MapRoute(
                name: "@List",
                url: "@/-1",
                defaults: new { controller = "Panel", action = "List", ajax = true }
            );
            routes.MapRoute(
                name: "@Content",
                url: "@/{ID}",
                defaults: new { controller = "Panel", action = "Content", ajax = true }
            );
        }
    }
}
