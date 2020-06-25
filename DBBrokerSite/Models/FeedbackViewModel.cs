using DBBroker.Engine;
using DBBrokerSite.Controllers;
using DBBrokerSite.Model;
using DBBrokerSite.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBBrokerSite.Models
{
    public class FeedbackViewModel
    {
        public SupportedLanguages UserLanguage { get; set; }

        public string Title { get; set; }

        public string ThTitle { get; set; }

        public string ThDescription { get; set; }

        public string ThStatus { get; set; }

        public string BtnFeedback { get; set; }

        public string BtnReportIssue { get; set; }

        public string LbNothingMessage { get; set; }

        public string LbClose { get; set; }

        public string LbSave { get; set; }

        public string LbThanksFeedback { get; set; }

        public string LbThanksIssue { get; set; }

        public List<DBBrokerIssue> Issues { get; set; }

        public DBBrokerIssue NewFeedback { get; set; }

        public DBBrokerIssue NewIssue { get; set; }

        public string KnownIssues { get; set; }

        public FeedbackViewModel()
        {
            UserLanguage = SupportedLanguages.English;
            LoadLanguageContext(UserLanguage);
        }

        public FeedbackViewModel(SupportedLanguages UserLanguage)
        {
            this.UserLanguage = UserLanguage;
            LoadLanguageContext(UserLanguage);
        }

        public void LoadLanguageContext(SupportedLanguages language, string Search = "", int IdStatus = -1)
        {
            Issues = DB_DBBrokerIssue.GetActiveBugs();
            
            if (language == SupportedLanguages.Português)
            {
                Title = "Feedback";

                KnownIssues = "Bugs conhecidos";
                LbNothingMessage = "Nenhum bug conhecido. Tudo muito quieto :O Espere aí, isso é ótimo! :)";

                ThTitle = "Título";
                ThDescription = "Descrição";
                ThStatus = "Status";
                BtnFeedback = "Nos dê algum feedback...";
                BtnReportIssue = "Relate um bug";
                LbClose = "Fechar";
                LbSave = "Enviar";

                LbThanksFeedback = "Obrigado por seu feedback! Se uma resposta for necessária breve estaremos em contato ;)";
                LbThanksIssue = "Obrigado por sua contribuição! O seu relato sobre uma falha será analizada e respondida o mais breve possível. ";
            }
            else
            {
                Title = "Feedback";

                KnownIssues = "Known issues";
                LbNothingMessage = "None known issue. So quiet :O Wait, that's great! :)";

                ThTitle = "Title";
                ThDescription = "Description";
                ThStatus = "Status";
                BtnFeedback = "Give some feedback...";
                BtnReportIssue = "Report an issue";
                LbClose = "Close";
                LbSave = "Send";

                LbThanksFeedback = "Thanks for your feedback! If a response is necessary we are going to be in touch very soon ;)";
                LbThanksIssue = "Thanks for contributing! Your information about an issue will be analyzed and responded as soon as possible. ";
            }
        }
    }
}