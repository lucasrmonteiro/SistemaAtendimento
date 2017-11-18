using NetUtil.Util.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Filter.Attributes
{
    /// <summary>
    /// Define o Join para a consulta a ser realizada
    /// 
    /// ex. hibernate:
    /// [Join("A.Propriedade", "P")]
    /// 
    /// ex. entity:
    /// [Join("A.Tabela.IdTablea", "P")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class JoinAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public JoinType? JoinType { get; set; }

        /// <summary>
        /// construtor que considera a propriedade e o alias para o join
        /// </summary>
        /// <param name="property"></param>
        /// <param name="alias"></param>
        public JoinAttribute(string property, string alias)
        {
            this.Property = property;
            this.Alias = alias;
            this.JoinType = NetUtil.Util.Enums.JoinType.None;
        }

        /// <summary>
        /// construtor que considera a propriedade e o alias para o join
        /// </summary>
        /// <param name="property"></param>
        /// <param name="alias"></param>
        public JoinAttribute(string property, string alias, JoinType joinType)
        {
            this.Property = property;
            this.Alias = alias;
            this.JoinType = joinType;
        }

    }
}
