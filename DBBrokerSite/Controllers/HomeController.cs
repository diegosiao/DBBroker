using DBBrokerSite.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace DBBrokerSite.Controllers
{
    public class HomeController : DBBrokerSiteController
    {
        public ActionResult Index(string r = null)
        {
            if (r != null)
                Log("visit_from_" + r);
            
            return View(new HomeIndexViewModel(UserLanguage));
        }

        public FileResult Download(string d = null)
        {
            string file = d == null ? "DBBroker.zip" : d;

            Log("download");

            byte[] fileBytes = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\Downloads\\" + file);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
        
        public ActionResult InstallViaNuget()
        {
            return View();
        }

        public ActionResult GettingStarted(string v = null)
        {
            return View(new GettingStartedViewModel(UserLanguage, v));
        }

        public ActionResult Features()
        {
            return View(new FeaturesViewModel(UserLanguage));
        }

        public ActionResult About()
        {
            return View(new AboutViewModel(UserLanguage));
        }

        public ActionResult License()
        {
            return View();
        }
                
        public ActionResult HelpUs()
        {
            return View();
        }

        public ActionResult Thanks()
        {
            return View();
        }

        public ActionResult SetLanguage(string lang)
        {
            Response.SetCookie(new HttpCookie(Cookies.Language, lang) { Secure = false });
            return Redirect(Request.UrlReferrer.LocalPath);
        }
    }
}