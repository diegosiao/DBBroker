using DBBrokerSite.Controllers;

namespace DBBrokerSite.Models
{
    public class FeaturesViewModel
    {
        public SupportedLanguages UserLanguage { get; set; }

        public string Title { get; set; }

        public string ScriptGen { get; set; }
        public string ScriptGenParag1 { get; set; }
        public string ScriptGenParag2 { get; set; }
        public string ScriptGenParag3 { get; set; }
        public string ScriptGenParag4 { get; set; }

        public string ConnStrEncryption { get; set; }
        public string ConnStrEncryptionParag1 { get; set; }
        public string ConnStrEncryptionParag2 { get; set; }
        public string ConnStrEncryptionParag3 { get; set; }

        public FeaturesViewModel(SupportedLanguages userLanguage)
        {
            UserLanguage = userLanguage;

            if (UserLanguage == SupportedLanguages.English)
            {
                Title = "Features";
                ScriptGen = "Script Generation";
                ScriptGenParag1 = "DBBroker does not change database structures, because it believes in the principle of <a href='https://en.wikipedia.org/wiki/Separation_of_concerns' target='_blank'>Separation of Concerns</a>.";
                ScriptGenParag2 = "Gennerally the greatest responsibility of an application is manipulate data, a human should think an then change data structure. To give DBBroker this power, it would require the user given to it other privileges than reading and writing data. That would not be good.";
                ScriptGenParag3 = "The good news is DBBroker can deliver you the script to generate the database that reflects the classes of the mapped domain. Just access the methods of the class:";
                ScriptGenParag4 = "DBBroker.Engine.SqlScriptMaker";

                ConnStrEncryption = "Connection String Encryption";
                ConnStrEncryptionParag1 = "Security should be a constant concern in software development. To garantee that only the appropriated persons will have access to the sensitive information of the Connection String some additional and, sometimes, hard work is necessary.";
                ConnStrEncryptionParag2 = "To simplify that, DBBroker provides a method that encrypts the DBBroker.config file contents, just call the static method:";
                ConnStrEncryptionParag3 = "DBBroker.Engine.Configuration.EncryptConfigFile()";
            }
            else if(UserLanguage == SupportedLanguages.Português)
            {
                Title = "Funções";
                ScriptGen = "Geração de Script";
                ScriptGenParag1 = "O DBBroker não altera a estrutura de sua base de dados, isso porque ele acredita no princípio da <a href='https://en.wikipedia.org/wiki/Separation_of_concerns' target='_blank'>Separação de Responsabilidades</a> (em inglês, Separation of Concerns).";
                ScriptGenParag2 = "Geralmente a maior responsabilidade de uma aplicação é a manipulação de dados, um ser humano deve pensar e então mudar a estrutura dos dados. Para dar ao DBBroker este poder, seria necessário que o usuário dado a ele tivesse outros privilégios além de ler e escrever dados. Isso não seria bom.";
                ScriptGenParag3 = "A boa notícia é que o DBBroker pode disponibilizar o script de criação da base de dados que reflita as classes do domínio mapeado. Basta acessar os métodos da classe:";
                ScriptGenParag4 = "DBBroker.Engine.SqlScriptMaker";

                ConnStrEncryption = "Encriptação da String de Conexão";
                ConnStrEncryptionParag1 = "Segurança deve ser uma preocupação constante no desenvolvimento de software. Para garantir que apenas as pessoas apropriadas irão ter acesso ao conteúdo sensível da String de Conexão algum esforço adicional, e algumas vezes, trabalho duro é necessário.";
                ConnStrEncryptionParag2 = "Para simplificar isso, o DBBroker possui um método que encripta o conteúdo do arquivo DBBroker.config, basta acessar o método estático:";
                ConnStrEncryptionParag3 = "DBBroker.Engine.Configuration.EncryptConfigFile()";
            }
        }
    }
}
