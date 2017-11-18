using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SGCA.Models.Properties;

namespace SGCA.Models.Filters
{
    public class FiltroEsqueciSenha
    {
        /// <summary>
        ///     Propriedade referente ao campo "Dsc_login" do banco.
        ///     Aplica-se o filtro de campo obrigatório, com mensagem de erro.
        ///     Aplica-se o filtro de tamanho de campo, com mensagem de erro.
        /// </summary>
        [Required(ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_login_obrigatorio")]
        [StringLength(100, ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_tamanho_login")] 
        public string Dsc_login { get; set; }

        /// <summary>
        ///     Propriedade referente ao campo "Dsc_email" do banco.
        ///     Aplica-se o filtro de campo obrigatório, com mensagem de erro.
        ///     Aplica-se o definidor de tipo de dado 'EmailAddress'.
        ///     Aplica-se o filtro de tamanho de campo, com mensagem de erro.
        /// </summary>
        [Required(ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_email_obrigatorio")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_tamanho_email")] 
        public string Dsc_email { get; set; } 
    }
}