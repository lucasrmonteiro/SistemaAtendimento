using NetUtil.Util.Enums;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Filter.Attributes
{
    /// <summary>
    /// Define o Alias da classe a ser retornada
    /// 
    /// ex.:
    /// [Alias("A")]
    /// public class Entidade...
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// construtor que considera a propriedade e o alias para o join
        /// </summary>
        /// <param name="property"></param>
        /// <param name="alias"></param>
        public AliasAttribute(string alias)
        {
            this.Alias = alias;
        }

    }
}
