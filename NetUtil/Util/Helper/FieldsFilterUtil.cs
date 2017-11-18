using NetUtil.Util.Enums;
using NetUtil.Util.Filter.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NetUtil.Util.Helper
{
    public class FieldsFilterUtil
    {

        /// <summary>
        /// 
        /// Retorna a lista de propriedades que devem ser inicializadas <propriedade>
        /// de acordo com as propriedades anotadas com RestrictionAttribute
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<string> GetInitializeProperties(object filter)
        {
            IList<string> properties = new List<string>();

            foreach (var item in filter.GetType().GetProperties())
            {
                string property = ClassAttributeUtil.InitializeProperty(item);
                if (!String.IsNullOrWhiteSpace(property))
                {
                    properties.Add(property);
                }
            }

            return properties.Count > 0 ? properties : null;
        }

        /// <summary>
        /// Adiciona um join a lista de <propriedade, <alias, JoinType>> para fazer join na consulta 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="alias"></param>
        /// <param name="joinType"></param>
        /// <param name="typedJoins"></param>
        /// <returns></returns>
        public static void AddJoin(string property, string alias, JoinType joinType, IDictionary<string, IDictionary<string, JoinType>> typedJoins)
        {
            IDictionary<string, JoinType> join = new Dictionary<string, JoinType>();
            join.Add(alias, joinType);
            typedJoins.Add(property, join);
        }

        /// <summary>
        /// Adiciona datas no criteria verificando
        /// as datas passadas no parametro para a contrucao. <propriedade,<restricao,valor>>
        /// 
        /// if (dataDe != null)
        /// {
        ///     if (dataAte != null)
        ///     {
        ///         // Se ambas datas forem preenchidas, cria um 'BETWEEN' 
        ///     }
        ///     else
        ///     {
        ///         // Se apenas "Data de" for preenchida, cria um '>=' 
        ///     }
        /// }
        /// else if (dataAte != null)
        /// {
        ///     // Se apenas "Data até" for preenchida, cria um '<=' 
        /// }
        /// 
        /// </summary>
        /// <param name="dataDe">data 'de' no periodo de data</param>
        /// <param name="dataAte">data 'ate' no periodo de data</param>
        /// <param name="coluna">coluna a ser comparada</param>
        /// <param name="criteria">criteria que a expressao sera adicionada</param>
        /// <returns>true - caso alguma data tenha entrado no criteria / false - se ambas as datas forem igual a null</returns>
        public static bool AdicionaDatasDeAte(DateTime? dataDe, DateTime? dataAte, String property,
                                                IDictionary<string, IDictionary<Restriction, object>> fieldsFilter)
        {
            if (dataDe != null)
            {
                if (dataAte != null)
                {
                    // Se ambas datas forem preenchidas, cria um 'BETWEEN'                 
                    AddBetween(dataDe.Value, dataAte.Value, property, fieldsFilter);
                    return true;
                }
                else
                {
                    // Se apenas "Data de" for preenchida, cria um '>=' 
                    AddGe(dataDe.Value, property, fieldsFilter);
                    return true;
                }
            }
            else if (dataAte != null)
            {
                // Se apenas "Data até" for preenchida, cria um '<=' 
                AddLe(dataAte.Value, property, fieldsFilter);
                return true;
            }
            return false;
        }

        /// <summary>
        ///
        /// Adiciona between no criteria verificando
        /// os valores passados no parametro para a contrucao.
        /// 
        /// if (valor1 != null)
        /// {
        ///     if (valor2 != null)
        ///     {
        ///         // Se ambos estiverem preenchidos, cria um 'BETWEEN' 
        ///     }
        ///     else
        ///     {
        ///         // Se apenas "valor1" for preenchido, cria um '>=' 
        ///     }
        /// }
        /// else if (valor2 != null)
        /// {
        ///     // Se apenas "valor2" for preenchido, cria um '<=' 
        /// }
        /// 
        /// </summary>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        /// <param name="coluna"></param>
        /// <param name="restrictions"></param>
        public static void AddBetween(object value1, object value2, string property,
            IDictionary<string, IDictionary<Restriction, object>> restrictions)
        {
            if (String.IsNullOrWhiteSpace(property) || restrictions == null)
            {
                return;
            }

            if (value1 != null)
            {
                if (value2 != null)
                {
                    // Se ambas datas forem preenchidas, cria um 'BETWEEN'                 
                    IDictionary<Restriction, object> restriction = new Dictionary<Restriction, object>();
                    IList<object> values = new List<object>();

                    if (value1.GetType().ToString().Contains("DateTime"))
                    {
                        values.Add(Convert.ToDateTime(value1));
                        values.Add(Convert.ToDateTime(value2));
                    }
                    else
                    {
                        values.Add(value1);
                        values.Add(value2);
                    }

                    restriction.Add(Restriction.Between, values);
                    restrictions.Add(property, restriction);
                }
                else
                {
                    // Se apenas "valor de" for preenchida, cria um '>=' 
                    AddGe(value1, property, restrictions);
                }
            }
            else if (value2 != null)
            {
                // Se apenas "valor até" for preenchida, cria um '<=' 
                AddLe(value2, property, restrictions);
            }
        }

        /// <summary>
        /// Adiciona o caso [menor ou igual] a lista de restricoes
        /// 
        /// * se valor for do tipo "DateTime" adiciona o Restriction.LeDate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <param name="restrictions"><propriedade,<restricao,valor>></param>
        public static void AddLe(object value, string property,
            IDictionary<string, IDictionary<Restriction, object>> restrictions)
        {
            Restriction restriction;
            if (value != null && value.GetType().ToString().Contains("DateTime"))
            {
                restriction = Restriction.LeDate;
            }
            else
            {
                restriction = Restriction.Le;
            }
            AddRestriction(value, restriction, property, restrictions);
        }

        /// <summary>
        /// Adicionar o caso [maior ou igual] a lista de restricoes
        /// 
        /// * se valor for do tipo "DateTime" adiciona o Restriction.GeDate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <param name="restrictions"><propriedade,<restricao,valor>></param>
        public static void AddGe(object value, string property,
            IDictionary<string, IDictionary<Restriction, object>> restrictions)
        {
            Restriction restriction;
            if (value != null && value.GetType().ToString().Contains("DateTime"))
            {
                restriction = Restriction.GeDate;
            }
            else
            {
                restriction = Restriction.Ge;
            }
            AddRestriction(value, restriction, property, restrictions);
        }

        /// <summary>
        /// Adiciona o caso equals a lista de restricoes
        /// 
        /// * se valor for do tipo "DateTime" adiciona o restriction.EqDate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <param name="restrictions"><propriedade,<restricao,valor>></param>
        public static void AddEqual(object value, string property, IDictionary<string, IDictionary<Restriction, object>> restrictions)
        {
            Restriction restriction;
            if (value != null && value.GetType().ToString().Contains("DateTime"))
            {
                restriction = Restriction.EqDate;
            }
            else
            {
                restriction = Restriction.Eq;
            }
            AddRestriction(value, restriction, property, restrictions);
        }

        /// <summary>
        /// Adiciona a restricao a lista de restricoes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="restriction"></param>
        /// <param name="property"></param>
        /// <param name="restrictions"><propriedade,<restricao,valor>></param>
        public static void AddRestriction(object value, Restriction restriction, string property,
            IDictionary<string, IDictionary<Restriction, object>> restrictions)
        {
            if (String.IsNullOrWhiteSpace(property) ||
                (restrictions == null) ||
                ((value == null) &&
                !Restriction.IsNotNull.Equals(restriction) &&
                !Restriction.IsNull.Equals(restriction)))
            {
                return;
            }

            IDictionary<Restriction, object> restrictionObject = new Dictionary<Restriction, object>();
            switch (restriction)
            {
                case Restriction.EqDate:
                case Restriction.GeDate:
                case Restriction.LeDate:
                case Restriction.EqDateTime:
                case Restriction.GeDateTime:
                case Restriction.LeDateTime:
                    restrictionObject.Add(restriction, Convert.ToDateTime(value));
                    break;
                default:
                    restrictionObject.Add(restriction, value);
                    break;
            }

            restrictions.Add(property, restrictionObject);
        }

    }
}