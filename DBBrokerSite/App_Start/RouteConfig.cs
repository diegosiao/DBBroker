using System.Web.Mvc;
using System.Web.Routing;

namespace DBBrokerSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Default2",
            //    url: "{controller}/{action}/{lang}",
            //    defaults: new { controller = "Home", action = "GettingStarted", lang = UrlParameter.Optional }
            //);
        }
    }
}
