using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroAlterarSenha
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
        ///     Propriedade referente ao campo "Dsc_senha" do banco.
        ///     Aplica-se o filtro de campo obrigatório, com mensagem de erro.
        ///     Aplica-se o definidor de tipo de dado 'Password'.
        ///     Aplica-se o filtro de tamanho de campo, com mensagem de erro.
        /// </summary>
        [Required(ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_senha_obrigatoria")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength=6, ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_tamanho_senha_min_max")]
        public string Dsc_senha { get; set; }

        /// <summary>
        ///     Propriedade referente à nova senha.
        ///     Aplica-se o filtro de campo obrigatório, com mensagem de erro.
        ///     Aplica-se o definidor de tipo de dado 'Password'.
        ///     Aplica-se o filtro de tamanho de campo, com mensagem de erro.
        /// </summary>
        [Required(ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = "erro_nova_senha_obrigatoria")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType =
                    typeof(Resources),
                ErrorMessageResourceName = "erro_tamanho_nova_senha_min_max")]
        public string NovaSenha { get; set; }

        /// <summary>
        ///     Propriedade referente à confirmação de nova senha.
        ///     Aplica-se o filtro de campo obrigatório, com mensagem de erro.
        ///     Aplica-se o definidor de tipo de dado 'Password'.
        ///     Aplica-se o filtro de tamanho de campo, com mensagem de erro.
        /// </summary>
        [Required(ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName = 
                        "erro_confirmacao_nova_senha_obrigatoria")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType =
                    typeof(Resources),
                  ErrorMessageResourceName =
                        "erro_tamanho_confirmacao_nova_senha_min_max")]
        public string ConfirmaNovaSenha { get; set; }
    }
}