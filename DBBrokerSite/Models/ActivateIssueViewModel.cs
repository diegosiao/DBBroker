using DBBrokerSite.Controllers;
using DBBrokerSite.Model;
using DBBrokerSite.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBBrokerSite.Models
{
    public class ActivateIssueViewModel
    {
        public string Title { get; set; }

        public string TxSubtitle { get; set; }

        public string TxTitle { get; set; }

        public string TxDescription { get; set; }
        
        public DBBrokerIssue Issue { get; set; }

        public ActivateIssueViewModel(SupportedLanguages lang, string token)
        {
            LoadLanguageContext(lang, token);
        }

        private void LoadLanguageContext(SupportedLanguages lang, string token)
        {
            try
            {
                Issue = DB_DBBrokerIssue.GetByToken(token);
            }
            catch (Exception ex)
            {
                AppLog.Log(ex);
            }

            if (Issue == null)
                Issue = new DBBrokerIssue();

            if (lang == SupportedLanguages.Português)
            {
                Title = "Ativação do relato";
                TxSubtitle = Issue.Id == 0 ? ":( Nenhuma interação encontrada" : "O seu feedback ou relato de um bug foi ativado. Entraremos em contato o mais breve possível.";
                TxTitle = "Título";
                TxDescription = "Descrição";
            }
            else
            {
                Title = "Issue activation";
                TxSubtitle = Issue.Id == 0 ? ":( Nothing found" :  "Your feedback or issue has been activated. We will respond as soon as possible.";
                TxTitle = "Title";
                TxDescription = "Description";
            }

        }
    }
}