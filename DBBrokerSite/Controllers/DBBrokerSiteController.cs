using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DBBrokerSite.Controllers
{
    public enum SupportedLanguages { English, Português }
    
    public class DBBrokerSiteController : Controller
    {
        public SupportedLanguages UserLanguage { get; set; }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (Request.Cookies[Cookies.Language] == null)
            {
                string lang = Request.UserLanguages[0];
                Response.SetCookie(new HttpCookie(Cookies.Language, !string.IsNullOrEmpty(lang) && lang.StartsWith("pt") ? "pt" : "en") { Expires = DateTime.Now.AddDays(40) });
            }
            else
                UserLanguage = Request.Cookies[Cookies.Language].Value == "pt" ? SupportedLanguages.Português : SupportedLanguages.English;

            ViewBag.UserLanguage = UserLanguage == SupportedLanguages.English ? "en" : "pt";
            return base.BeginExecuteCore(callback, state);
        }

        public void Log(string acao)
        {
            try
            {
                FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "access_log.txt", FileMode.Append);
                byte[] content = Encoding.UTF8.GetBytes("\r\n" + acao + ";" + DateTime.Now + ";" + Request.UserLanguages[0] + ";" + Request.ServerVariables["REMOTE_ADDR"]);
                file.Write(content, 0, content.Length);
                file.Dispose();
            }
            catch (Exception) { }
        }

    }
}
