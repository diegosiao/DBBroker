using DBBrokerSite.Controllers;

namespace DBBrokerSite.Models
{
    public class HomeIndexViewModel
    {
        public SupportedLanguages UserLanguage { get; set; }

        public string MainSubtitle { get; set; }

        public string LearnMore { get; set; }

        public string Block1Title { get; set; }
        public string Block1Content { get; set; }

        public string Block2Title { get; set; }
        public string Block2Content { get; set; }

        public string Block3Title { get; set; }
        public string Block3Content { get; set; }

        public HomeIndexViewModel(SupportedLanguages userLanguage)
        {
            this.UserLanguage = userLanguage;

            if (UserLanguage == SupportedLanguages.English)
            {
                MainSubtitle = "A free, lightweight and simple to use .NET library for accessing SQL Server Databases in your application. You're gonna love it.";

                LearnMore = "Learn More »";

                Block1Title = "Getting started";
                Block1Content = @"You know Entity Framework... the problem it's not you, it's me. I'm looking for something faster and simpler.
                                Get ready to use DBBroker in your applications quickly.";

                Block2Title = "Features";
                Block2Content = "Some interesting features of DBBroker that you should know while using it.";

                Block3Title = "Help Us";
                Block3Content = @"We've made DBBroker thinking about you. Now we are needing you... Your donation is incredibly important for us!";
            }
            else // Português
            {
                MainSubtitle = "Uma biblioteca .NET grátis, leve e simples de usar para acessar as bases SQL Server de sua aplicação. Você vai adorar conhecer.";

                LearnMore = "Saiba mais »";

                Block1Title = "Primeiros passos";
                Block1Content = @"Sabe de uma coisa Entity Framework, o problema não é você, sou eu. Estou procurando algo mais leve e simples.
                                Descubra rapidamente como usar o DBBroker em suas aplicações.";

                Block2Title = "Funções";
                Block2Content = "Algumas funções interessantes que você deve conhecer enquanto usa o DBBroker.";

                Block3Title = "Contribua ;)";
                Block3Content = @"O DBBroker foi desenvolvido para ser seu gratuitamente, mas contamos com sua contribuição, ela é muito importante pra nós.";
            }
        }
    }
}
