using DBBroker.Engine;
using DBBrokerSite.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace DBBrokerSite.Persistence
{
    public class DBUser : DBBroker<DBBrokerUser>
    {
        public static void SaveNewUser(DBBrokerUser user)
        {
            try
            {
                user.TokenCode = Guid.NewGuid().ToString();
                Save(user);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.Credentials = new System.Net.NetworkCredential("diegosiao@gmail.com", "geladeira");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.Subject = "DBBroker - " + (user._UserLanguage == Controllers.SupportedLanguages.English ? "Support" : "Suporte.html");
                mail.From = new MailAddress("diegosiao@getdbbroker.com", "DBBroker Support");
                mail.To.Add(new MailAddress(user.Email));
                mail.Bcc.Add(new MailAddress("diegosiao@gmail.com"));

                mail.IsBodyHtml = true;

                string file_to_body = user._UserLanguage == Controllers.SupportedLanguages.English ? "issue_response_en.html" : "issue_response_pt.html";

                using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + file_to_body))
                {
                    mail.Body = reader.ReadToEnd();
                }

                mail.Body = mail.Body.Replace("{0}", user.TokenCode);

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                AppLog.Log(ex);
            }
        }

        public static void ActivateIssue(string Token)
        {
            try
            {
                List<DBBrokerUser> result =
                    ExecCmdSQL( "UPDATE Users SET Validation = GETDATE() WHERE TokenCode = @Token AND Validation IS NULL; SELECT @@ROWCOUNT AS IdUser; ",
                                new List<DbParameter>() { new SqlParameter("@Token", Token) });

                if (result[0].Id != 1)
                    throw new Exception("Nenhum usuário foi ativado com o token: " + Token);
            }
            catch (Exception ex)
            {
                AppLog.Log(ex);
            }
        }
    }
}