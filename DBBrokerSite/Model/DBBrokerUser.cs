using DBBroker.Mapping;
using DBBrokerSite.Controllers;
using System;
using System.ComponentModel.DataAnnotations;

namespace DBBrokerSite.Model
{
    [DBMappedClass(Table ="Users", PrimaryKey ="IdUser")]
    public class DBBrokerUser : BusinessObject
    {
        [Required]
        public string Email { get; set; }

        public string Name { get; set; }

        public string TokenCode { get; set; }

        [DBTransient]
        public SupportedLanguages _UserLanguage { get; set; }

        /// <summary>
        /// Representação da propriedade _UserLanguage: 'en' ou 'pt'
        /// </summary>
        public string UserLanguage
        {
            get
            {
                return _UserLanguage == SupportedLanguages.English ? "en" : "pt";
            }
            set
            {
                if (value == "pt")
                    _UserLanguage = SupportedLanguages.Português;
                else
                    _UserLanguage = SupportedLanguages.English;
            }
        }

        [DBTransient]
        public bool Validated
        {
            get
            {
                return Validation != null && Validation.HasValue;
            }
        }

        [DBReadOnly(DBDefaultValue ="GETDATE()")]
        public DateTime Registration { get; set; }

        public DateTime? Validation { get; set; }
    }
}