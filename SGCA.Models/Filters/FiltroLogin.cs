using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroLogin
    {
        /// <summary>
        ///     Propriedade referente ao campo "Dsc_login" do banco.
        ///     Aplica-se o filtro de campo obrigatório, com mensagem de erro.
        ///     Aplica-se o filtro de tamanho de campo, com mensagem de erro.
        /// </summary>             
        [Required(ErrorMessageResourceType=
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_login_obrigatorio")]
        [StringLength(100, ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_tamanho_login")]        
        public string Dsc_login { get; set; }
        
        /// <summary>
        ///     Propriedade referente ao campo "Dsc_senha" do banco.
        ///     Aplica-se o filtro de campo obrigatório, com mensagem de erro.
        ///     Aplica-se o definidor de tipo de dado 'Password'.
        ///     Aplica-se o filtro de tamanho de campo, com mensagem de erro.
        /// </summary>
        [Required(ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_senha_obrigatoria")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_tamanho_senha")]
        public string Dsc_senha  { get; set; }           
    }
}