using DBBrokerSite.Controllers;
using System;
using System.Collections.Generic;
using System.IO;

namespace DBBrokerSite.Models
{
    public class YouTubeVideo
    {
        public string Code { get; set; }

        public string YouTubeCode { get; set; }

        public string Title { get; set; }

        public string DefaultPictureUrl
        {
            get
            {
                if (YouTubeCode == null)
                    return null;
                                
                return string.Format("//i.ytimg.com/vi/{0}/mqdefault.jpg", YouTube_v);
            }
        }

        public string YouTube_v { get; set; }

        public string YouTube_list { get; set; }

        public string EmbedUrl { get
            {
                if (YouTubeCode == null)
                    return null;
                else if (YouTubeCode.StartsWith("list="))
                {
                    return string.Format("https://www.youtube.com/embed/{0}?{1}", YouTube_v, YouTube_list);
                }
                else
                    return string.Format("https://www.youtube.com/embed/{0}", YouTubeCode);
            }
        }

        public string YouTubeUrl
        {
            get
            {
                return string.Format("https://www.youtube.com/watch?{0}", (YouTubeCode.StartsWith("list=") ? YouTubeCode : "v=" + YouTubeCode));
            }
        }

        public YouTubeVideo(string code, string youtubecode, string title)
        {
            Code = code;
            YouTubeCode = youtubecode;
            Title = title;

            string[] vars = YouTubeCode.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string var in vars)
            {
                if (var.StartsWith("v="))
                    YouTube_v = var.Substring(2);
                else if (var.StartsWith("list="))
                    YouTube_list = var.Substring(5);
                else
                    YouTube_v = var;
            }
        }
    }

    public class GettingStartedViewModel
    {
        public SupportedLanguages UserLanguage { get; private set; }

        public string Title { get; set; }
        public string New { get; set; }

        public YouTubeVideo MainVideo { get; set; }
        public Dictionary<string, YouTubeVideo> Videos { get; set; }

        /// <summary>
        /// Tab ativa (tab_steps ou tab_video)
        /// </summary>
        public string ActiveTab { get; set; }
        public string TabSteps { get; set; }
        public string TabVideo { get; set; }

        public string WhatIsTitle { get; set; }
        public string WhatIsContent { get; set; }

        public string WhyUseItTitle { get; set; }
        public string WhyUseItContent1 { get; set; }
        public string WhyUseItContent2 { get; set; }

        public string Step1Title { get; set; }
        public string Step1Parag1 { get; set; }

        public string Step2Title { get; set; }
        public string Step2Parag1 { get; set; }
        public string Step2Parag2 { get; set; }
        public string Step2Parag3 { get; set; }
        public string Step2Parag3Hint { get; set; }
        public string Step2Parag4 { get; set; }
        public string Step2Parag5 { get; set; }
        public string Step2Parag6 { get; set; }
        public string Step2Parag7 { get; set; }
        public string Step2Parag8 { get; set; }
        public string Step2Parag9 { get; set; }
        public string Step2Parag10 { get; set; }

        public string Step3Title { get; set; }
        public string Step3Parag1 { get; set; }
        public string Step3Parag2 { get; set; }
        public string Step3Parag3 { get; set; }

        public string LastThing { get; set; }
        public string LastThingParag1 { get; set; }
        public string LastThingParag2 { get; set; }
        public string LastThingParag3 { get; set; }
        public string LastThingParag4 { get; set; }
        public string LastThingParag5 { get; set; }
        public string LastThingParag6 { get; set; }
        
        public GettingStartedViewModel(SupportedLanguages userLanguage, string main_video_code = null)
        {
            UserLanguage = userLanguage;
            LoadLanguageContext(userLanguage, main_video_code);

            ActiveTab = "tab_video";
        }

        public void LoadLanguageContext(SupportedLanguages userLanguage, string main_video_code = null)
        {
            UserLanguage = userLanguage;
            Videos = new Dictionary<string, YouTubeVideo>();
            
            string file = AppDomain.CurrentDomain.BaseDirectory + (userLanguage == SupportedLanguages.English ? "youtube_videos_en.txt" : "youtube_videos_pt.txt");

            try
            {
                if (File.Exists(file))
                {
                    StreamReader reader = new StreamReader(file);
                    string[] lines = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        var video = new YouTubeVideo(line.Split('|')[0], line.Split('|')[1], line.Split('|')[2]);

                        if (main_video_code == null)
                            main_video_code = video.Code;

                        Videos.Add(video.Code, video);
                    }
                    reader.Dispose();

                    if (Videos.ContainsKey(main_video_code))
                    {
                        MainVideo = Videos[main_video_code];
                        Videos.Remove(main_video_code);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
                        
            if (UserLanguage == SupportedLanguages.English)
            {
                Title = "Tutorial";

                TabSteps = "Step by step";
                TabVideo = "Videos";
                New = "New!";

                WhatIsTitle = "What is DBBroker?";
                WhatIsContent = "It is a free .NET library which purpose is to provide transparent access and data manipulation on SQL Server databases through ORM (Object/Relational Mapping).";
                
                WhyUseItTitle = "Why should I use DBBroker?";

                WhyUseItContent1 = "Because DBBroker is simple and discomplicated. Also because writing inserts, updates, deletes or queries in SQL language use to be a very tedious job for developers. Things get even worse when changes in the application model require a large revision in code. That is because Relational Objects must reflect database structure or vice versa.";
                WhyUseItContent2 = "That is why using DBBroker is a good idea, the greatest part of the job in data manipulation it is made by it. DBBroker makes the layer of data loading and manipulation transparent.";
                
                Step1Title = "Step 1: Referencing DBBroker";
                Step1Parag1 = @"Before anything else, is necessary to download the DBBroker library at <a href='\Home\Index' target='_blank'>getdbroker.com</a> and reference it in your project, or you can reference via <a href='https://www.nuget.org/packages/DBBroker' target='_blank'>NuGet</a> in Visual Studio, if you do so, you won't need to worry about anything else, NuGet will take care of the rest.";
                
                Step2Title = "Step 2: Mapping the domain classes";

                Step2Parag1 = "About the domain classes, DBBroker depends on 2 easy to follow conventions:";
                Step2Parag2 = "1. The required inclusion of a property of type int called 'Id'";
                Step2Parag3 = "2. The name of the properties of the class must match the name of the respective columns in the database";
                Step2Parag3Hint = "Although it is possible to map a property to column with a different name with the attribute 'DBMappedTo', you should not. Let theses cases be exceptions, not a rule";
                Step2Parag4 = "The mapping of the classes and properties uses 5 attributes (or decoration), they are the following:";
                Step2Parag5 = "<strong>DBMappedClass:</strong> Maps classes of the domain to respective tables and primary keys;";
                Step2Parag6 = "<strong>DBMappedList:</strong> Used in properties of type System.Collections.Generic.List to map lists of domain elements;";
                Step2Parag7 = "<strong>DBTransient:</strong> To properties that will not be persisted;";
                Step2Parag8 = "<strong>DBReadOnly:</strong> To read only properties, their values will be generated by database; ";
                Step2Parag9 = "<strong>DBMappedTo:</strong> Works as alias to the properties, defining the name of the mapped column in database.";

                Step3Title = "Step 3: Criar classes de acesso aos dados";
                Step3Parag1 = "As convention, for each domain class that application will access directly, must be created a corresponding data access class, it is in these classes that the magic happens.";
                Step3Parag2 = "These classes must have the same name of the domain class plus 'DB' (e.g. DBPerson for a domain class named Person).";
                Step3Parag3 = "After you do that, you will already be able to access methods like Save(), Delete(), GetById(), GetAll(), and ExecCmdSQL() to manipulate the data of the domain class of the context.";
                
                LastThing = "Finally...";
                LastThingParag1 = "To make DBBroker able to reach your database it is necessary to create a simple configuration file in the base directory of your application, this file must be named 'DBBroker.config'.";

                LastThingParag2 = "The format of each line of this file has 3 values separated by the character pipe '|': (1) Namespace of de mapped domain, (2) constant value 'SQLServer', and (3) respective SQL Server Connection String.";
                LastThingParag3 = "<span class='badge'>MyGreatApplication.Model|SQLServer|Data Source=MyServer; Initial Catalog=GreatDB; User Id=minime; Password=Super§ecret;</span>";
                LastThingParag4 = "Have some fun! If you like, please share. If you can, make a donation ;)";
                LastThingParag5 = "Thank you! ☺";                
            }
            else
            {
                Title = "Tutorial";

                TabSteps = "Passo a passo";
                TabVideo = "Vídeos";
                New = "Novo!";

                WhatIsTitle = "O que é DBBroker?";
                WhatIsContent = "É uma biblioteca .NET gratuita que tem a finalidade de tornar transparente a carga de objetos a partir de uma base de dados SQL Server através do Mapeamento de Objetos Relacionais (Object/Relational Mapping – ORM).";

                WhyUseItTitle = "Por que usar DBBroker?";
                WhyUseItContent1 = "Porque DBBroker é simples e descomplicado. Também porque escrever consultas, inserções, atualizações ou exclusões de registro na base de dados costuma ser um trabalho maçante para desenvolvedores. Isso porque geralmente exige a escrita de muitos comandos na linguagem SQL que precisam refletir os objetos, isso pode ser muito trabalhoso.";
                WhyUseItContent2 = "É por isso que usar o DBBroker é uma boa ideia, a maior parte do trabalho de manipulação de dados é ele quem faz. O DBBroker torna transparente toda a camada de carga de dados e as tarefas de inserção e atualização dos dados.";

                Step1Title = "Passo 1: Referenciar o DBBroker";
                Step1Parag1 = @"Antes de qualquer coisa é preciso fazer o download da biblioteca em <a href='\Home\Index' target='_blank'>getdbroker.com</a> e referenciar em seu projeto, ou instalar via <a href='https://www.nuget.org/packages/DBBroker' target='_blank'>NuGet</a> no Visual Studio, se fizer assim não terá que se preocupar com a referência, pois o NuGet cuida disso para você.";

                Step2Title = "Passo 2: Mapear as classes do domínio";
                Step2Parag1 = "No que diz respeito às classes de domínio, o funcionamento do DBBroker depende de 2 convenções fáceis de seguir:";
                Step2Parag2 = "1. A existência obrigatória de uma propriedade Id do tipo int";
                Step2Parag3 = "2. As propriedades devem ter o mesmo nome das colunas respectivamente mapeadas";
                Step2Parag3Hint = "Embora seja possível mapear uma propriedade para uma coluna de nome diferente, você não vai querer fazer isso, deixe esses casos ser exceção, não uma regra";
                Step2Parag4 = "O mapeamento das classes e propriedades se dá com anotação de 5 atributos, que são os seguintes:";
                Step2Parag5 = "<strong>DBMappedClass:</strong> Mapeia as classes do domínio às tabelas e chaves primárias;";
                Step2Parag6 = "<strong>DBMappedList:</strong> Usado em propriedades do tipo System.Collections.Generic.List para mapear listas com elementos de domínio;";
                Step2Parag7 = "<strong>DBTransient:</strong> Para propriedades que não serão persistidas;";
                Step2Parag8 = "<strong>DBReadOnly:</strong> Mapeia propriedades somente leitura, ou seja, seus valores são criados pelo banco de dados;";
                Step2Parag9 = "<strong>DBMappedTo:</strong> Funciona como alias para propriedades, define o nome da coluna mapeada no banco de dados.";
                
                Step3Title = "Passo 3: Criar classes de acesso aos dados";
                Step3Parag1 = "Como convenção, para cada classe de domínio que a aplicação irá acessar diretamente, deve ser criada uma classe de acesso a dados correspondente, são nessas classes que a mágica acontece.";
                Step3Parag2 = "Essas classes devem se chamar pelo nome da classe de domínio acrescido do prefixo ‘DB’, como no exemplo abaixo.";
                Step3Parag3 = "Depois que fizer isso, você já poderá acessar os métodos: Save(), Delete(), GetById(), GetAll() e ExecCmdSQL() para manipular os dados da classe de domínio do contexto.";

                LastThing = "Por último...";
                LastThingParag1 = "Para que o DBBroker consiga encontrar sua base de dados é preciso criar um arquivo de configuração simples no diretório base de sua aplicação, esse arquivo deve se chamar ‘DBBroker.config’.";
                LastThingParag2 = "O formato de cada linha do arquivo é composto por três valores separados pelo caractere pipe ‘|’: (1) Namespace do domínio mapeado, (2) o valor literal: “SQLServer” e (3) A Connection String para acesso à base SQL Server.";
                LastThingParag3 = "<span class='badge'>MyGreatApplication.Model|SQLServer|Data Source=MyServer; Initial Catalog=GreatDB; User Id=minime; Password=Super§ecret;</span>";
                LastThingParag4 = "Aproveite! Se gostou, compartilhe. Se puder, colabora ;)";
                LastThingParag5 = "Obrigado!";
                LastThingParag6 = "<strong>Eu</strong>quipe DBBroker <span style='font-size: 32px;'>☺</span>";
            }
        }
    }
}
