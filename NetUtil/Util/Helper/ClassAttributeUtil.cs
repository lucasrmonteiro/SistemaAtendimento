using NetUtil.Util.Filter.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetUtil.Util.Helper
{
    public class ClassAttributeUtil
    {
        /// <summary>
        /// Pega a descricao '[Description("descricao")]' do atributo na classe
        /// </summary>
        public static string PropertyDescription(PropertyInfo item)
        {
            var descriptions = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }

        /// <summary>
        /// Pega a descricao '[Description("descricao")]' da classe
        /// </summary>
        public static string ClassDescription(Type tipo)
        {
            var descriptions = (DescriptionAttribute[])tipo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }

        /// <summary>
        /// Retorna o RestrictionAttribute da propriedade
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static RestrictionAttribute RestrictionAtttribute(PropertyInfo item)
        {
            var attributes = (RestrictionAttribute[])item.GetCustomAttributes(typeof(RestrictionAttribute), false);

            if (attributes.Length == 0)
            {
                return null;
            }
            else
            {
                return attributes[0].Restriction == null ? null : attributes[0];
            }
        }

        /// <summary>
        /// Retorna a lista de JoinAttribute da classe ou null caso nao tenha JoinAttribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<JoinAttribute> JoinAlias(Type type)
        {
            IList<JoinAttribute> joins = null;

                var attributes = (JoinAttribute[])type.GetCustomAttributes(typeof(JoinAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    joins = new List<JoinAttribute>(attributes);
                }

            return joins;            
        }

        /// <summary>
        /// Retorna a lista de propriedade que precisam ser inicializadas de ecordo com o RestrictionAttribute
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string InitializeProperty(PropertyInfo item)
        {
            var retrictions = (RestrictionAttribute[])item.GetCustomAttributes(typeof(RestrictionAttribute), false);

            if (retrictions.Length == 0)
            {
                return null;
            }

            return retrictions[0].Initialize? retrictions[0].Property : null ;
        }

        /// <summary>
        /// Obtem o alias do AliasAttribute da classe
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string MainAlias(Type type)
        {
            var alias = (AliasAttribute[])type.GetCustomAttributes(typeof(AliasAttribute), false);

            if (alias.Length == 0)
            {
                return null;
            }

            return String.IsNullOrWhiteSpace(alias[0].Alias) ? null : alias[0].Alias;
        }

    }
}
