using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DBBrokerSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //HttpCookie lang = new HttpCookie(Cookies.Language, 
            //                                    Request.UserLanguages.First() != null && Request.UserLanguages.First().StartsWith("pt") ? "pt" : "en");
            //lang.Expires = DateTime.Now.AddDays(30);
            //Response.SetCookie(lang);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
