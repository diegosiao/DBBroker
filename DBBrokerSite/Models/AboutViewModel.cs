using DBBrokerSite.Controllers;

namespace DBBrokerSite.Models
{
    public class AboutViewModel
    {
        public SupportedLanguages UserLanguage { get; private set; }

        public string Title { get; set; }
        public string Especialization { get; set; }
        public string Certification { get; set; }

        public string TxLittleHistory { get; set; }

        public string History1 { get; set; }

        public string History2 { get; set; }

        public string History3 { get; set; }

        public AboutViewModel(SupportedLanguages userLanguage)
        {
            this.UserLanguage = userLanguage;

            if (UserLanguage == SupportedLanguages.English)
            {
                Title = "About";
                Especialization = "Corporative Systems Development Specialist - FARN | Brazil";
                Certification = "Microsoft Certifed Professional";

                TxLittleHistory = "Little history...";

                History1 = @"DBBroker is a .NET library developed and used in production since 2012 
                            at an organ of the Government of the state of Rio Grande do Norte, Brazil, 
                            where is frequently put under severe workloads.";

                History2 = @"Only at July 2015 DBBroker became a public library available at 
                            <a href='http://www.nuget.org/packages/dbbroker' target='_blank'>nuget.org</a> 
                            after an agreement with the IT managers at PGE/RN, to whom I register my gratitude.";

                History3 = "I hope you to <a href='http://facebook.com/getdbbroker' target='_blank'>like</a> this tool ;)";                
            }
            else if(UserLanguage == SupportedLanguages.Português)
            {
                Title = "Sobre";
                Especialization = "Especialista em Desenvolvimento de Sistemas Corporativos - FARN | Brasil";
                Certification = "Profissional Certificado pela Microsoft";

                TxLittleHistory = "Um pouco de história...";

                History1 = @"O DBBroker é uma biblioteca .NET que foi desenvolvida e é utilizada em produção desde 2012 
                            na Procuradoria Geral do Estado do Rio Grande do Norte onde é, quase sempre, submetida 
                            a severas cargas de trabalho.";

                History2 = @"Somente em Julho de 2015 o DBBroker se tornou uma biblioteca pública disponível 
                            em <a href='http://www.nuget.org/packages/dbbroker' target='_blank'>nuget.org</a> 
                            depois de acordo com os gestores de TI da PGE/RN, para quem registro meu agradecimento.";

                History3 = "Espero que você <a href='http://facebook.com/getdbbroker' target='_blank'>curta</a> essa ferramenta ;)";
            }
        }
    }
}
