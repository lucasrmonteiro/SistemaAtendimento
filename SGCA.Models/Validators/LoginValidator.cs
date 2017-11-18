using NetUtil.Util.Hibernate;
using SGCA.Models.Entity;
using SGCA.Models.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;

namespace SGCA.Models.Validators
{
    /// <summary>
    /// Validação customizada para CPF
    /// </summary>
    public class LoginValidator : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public LoginValidator() { }

        /// <summary>
        /// Faz a validação propriamente dita
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            bool valido;
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            var session = HibernateUtil.OpenSession();

            var valida_login = (from p in session.Query<Usuario>()
                                where p.Dsc_login == value
                                      select p);
            if (valida_login.Any())
            {
                valido = false;
            }
            else
            {
                valido = true;
            }

            return valido;
        }

        /// <summary>
        /// Mensagem de Erro
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.FormatErrorMessage(null),
                ValidationType = "customvalidationlogin"
            };
        }
    }
}