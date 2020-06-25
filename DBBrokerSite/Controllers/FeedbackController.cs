using DBBroker.Engine;
using DBBrokerSite.Model;
using DBBrokerSite.Models;
using DBBrokerSite.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBBrokerSite.Controllers
{
    public class FeedbackController : DBBrokerSiteController
    {
        public ActionResult Index()
        {
            return View(new FeedbackViewModel(UserLanguage));
        }

        [HttpPost]
        public ActionResult SaveNewFeedback(FeedbackViewModel model)
        {
            try
            {
                DBUser.SaveNewUser(model.NewFeedback.User);
                model.NewFeedback.Nature = new Nature() { Id = 1 };
                model.NewFeedback.Language = UserLanguage.ToString();
                DB_DBBrokerIssue.Save(model.NewFeedback);
            }
            catch (Exception ex)
            {
                AppLog.Log(ex);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ActivateIssue(string t)
        {
            DBUser.ActivateIssue(t);
            return View(new ActivateIssueViewModel(UserLanguage, t));
        }

        [HttpPost]
        public ActionResult SaveNewIssue(FeedbackViewModel model)
        {
            try
            {
                DBUser.SaveNewUser(model.NewIssue.User);
                model.NewIssue.Nature = new Nature() { Id = 2 };
                model.NewIssue.Language = UserLanguage.ToString();
                DB_DBBrokerIssue.Save(model.NewIssue);
            }
            catch (Exception ex)
            {
                AppLog.Log(ex);
            }
            return RedirectToAction("Index");
        }

    }
}