using NetUtil.Util.Enums;
using NetUtil.Util.Filter.Attributes;
using NetUtil.Util.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Filter
{
    /// <summary>
    /// Classe Abstrata para preenchimento do criterio de busca
    /// 
    /// Especificar <T> com a classe do mecanismo de busca utilizado
    /// 
    ///  ex.: 
    ///  
    /// Para o NHibernate
    ///     - CriteriaUtil : AbstractFilterUtil<ICriteria>
    ///     
    /// Para o EntityFrameWork   
    ///     - SqlQueryUtil : AbstractFilterUtil<StringBuilder>
    ///     
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractFilterUtil<T>
    {
        #region - Using Attributes
        /// <summary>
        /// Preenche o criteria de acordo com os attributes nas propriedades e na classe
        /// 
        /// ex.:
        ///
        ///     [Alias("aliasclasse")]
        ///     [Join("aliasclasse.Outra2", "aliasoutra2", JoinType.InnerJoin)]
        ///     public class SampleFilter
        ///     {
        ///         [Restriction("aliasoutra2.Create", Order.Asc, 0)]
        ///         public object Order_Only { get; set; }
        ///         
        ///         [Restriction("aliasclasse.Id", Restriction.Between, true)]
        ///         public int? Id_start { get; set; }
        ///     }
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="criteria"></param>
        public void FillSearchCriteria(object filter, T criteria, bool useOrder)
        {
            // tabelas de join, caso exista
            AddJoins(filter, criteria);
            //adiciona as restricoes da consulta e de ordenacao
            AddRestrictions(filter, criteria, useOrder);
        }

        /// <summary>
        /// Adiciona os joins anotados na classe ao criteria
        /// 
        /// ex.:
        ///     [Alias("aliasclasse")]
        ///     [Join("aliasclasse.Outra2", "aliasoutra2", JoinType.InnerJoin)]
        ///     public class SampleFilter 
        ///     {
        ///         
        ///     }
        /// 
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="criteria"></param>
        public void AddJoins(object filter, T criteria)
        {
            if (filter == null || criteria == null)
            {
                return;
            }
            //obtem a lista de JoinAttribute da classe
            IList<JoinAttribute> joins = ClassAttributeUtil.JoinAlias(filter.GetType());

            if (joins == null)
            {
                return;
            }

            foreach (var join in joins)
            {
                if (join.JoinType != null)
                {
                    AddJoin(join.Property, join.Alias, join.JoinType.Value, criteria);
                }
            }
        }

        /// <summary>
        /// 
        /// Adiciona restricoes ao criteria de acordo com os attributes das propriedades do objeto
        /// 
        /// ex.:
        ///     [Alias("aliasclasse")]
        ///     [Join("aliasclasse.Outra2", "aliasoutra2", JoinType.InnerJoin)]
        ///     public class SampleFilter
        ///     {
        ///         [Restriction("aliasoutra2.Create", Order.Asc, 0)]
        ///         public object Order_Only { get; set; }
        ///         
        ///         [Restriction("aliasclasse.Id", Restriction.Between, true)]
        ///         public int? Id_start { get; set; }
        ///     }
        ///     
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="criteria"></param>
        /// <param name="order"></param>
        public void AddRestrictions(object filter, T criteria, bool useOrder)
        {
            if (filter == null || criteria == null)
            {
                return;
            }
            //variaveis auxiliares para o caso de ser um between
            int qtd = 0;
            object value1 = null;
            string property1 = String.Empty;

            List<RestrictionAttribute> order = new List<RestrictionAttribute>();
            //percorre todas as propriedades da classe
            foreach (var property in filter.GetType().GetProperties())
            {
                //obtem o RestrictionAttribute da propriedade
                RestrictionAttribute restriction = ClassAttributeUtil.RestrictionAtttribute(property);
                //se nao possui RestrictionAttribute ou a propriedade que sera restringida na consulta nao foi informada passa para a proxima propriedade
                if (restriction == null ||
                    String.IsNullOrWhiteSpace(restriction.Property))
                {
                    continue;
                }

                // se na restricao existe indicacao de ordenar pelo campo, armazena a informacao para futura ordenacao
                if (restriction.Order != null)
                {
                    order.Add(restriction);
                }

                //se nao foi informada a restricao passa para a proxima propriedade
                if (restriction.Restriction == null)
                {
                    continue;
                }
                // obtem o valor da propriedade
                object value = property.GetValue(filter, null);
                
                switch (restriction.Restriction.Value)
                {
                        //Caso a restricao seja between
                    case Restriction.Between:
                    case Restriction.BetweenDateTime:
                        DealBetweens(value, restriction.Property, restriction.Restriction.Value, criteria,
                                                                    ref qtd, ref value1, ref property1);
                        break;
                        //caso a restricao seja IsNull ou IsNotNull
                    case Restriction.IsNull:
                    case Restriction.IsNotNull:
                        AddRestriction(value, restriction.Restriction.Value, restriction.Property, criteria);
                        break;
                    default:
                        //para o caso de outras restricoes verifica se o valor informado eh diferente de null, se for null nao leva em consideracao na consulta
                        if (value != null)
                        {
                            if (Restriction.Eq.Equals(restriction.Restriction.Value))
                            {
                                AddEqual(value, restriction.Property, criteria);
                            }
                            else
                            {
                                AddRestriction(value, restriction.Restriction.Value, restriction.Property, criteria);
                            }
                        }
                        break;
                }                
            }
            //adicionar lista de ordenacao ordenada por prioridade
            if (useOrder)
            {
                AddOrders(order, criteria);
            }
                
        }

        /// <summary>
        /// Adiciona ordenacao ao criteria de acordo com atributos anotados na classe
        /// 
        /// ex.:
        /// 
        ///     public class SampleFilter
        ///     {
        ///         [Restriction("Create", Order.Asc, 0)]
        ///         public object Order_Only { get; set; }
        ///         
        ///         [Restriction("Modify", Order.Asc, 1)]
        ///         public object Order_Only { get; set; }
        ///     }
        /// 
        /// 
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="criteria"></param>
        public void AddOrders(IList<RestrictionAttribute> orders, T criteria)
        {
            if (orders == null || criteria == null)
            {
                return;
            }

            if (orders.Count > 0)
            {
                //ordena por proridade
                ((List<RestrictionAttribute>)orders).Sort((o1, o2) => o1.Priority.CompareTo(o2.Priority));

                foreach (var item in orders)
                {
                    AddOrder(item.Property, item.Order.Value, criteria);
                }
            }
        }

        /// <summary>
        /// Trata o caso de RestrictionsAttributes com restricao Between.
        /// 
        /// Utilizado no loop que verifica cada propriedade da classe para montar as restricoes.
        /// 
        /// propriedades com restricao between devem estar uma abaixo da outra na classe 
        /// e com o mesmo nome de propriedade diferencidado de *_de *_ate
        /// 
        /// ex.:
        /// 
        ///     [Alias("aliasclasse")]
        ///     [Join("aliasclasse.Outra2", "aliasoutra2", JoinType.InnerJoin)]
        ///     public class SampleFilter
        ///     {
        ///         [Restriction("aliasoutra2.Create", Order.Asc, 0)]
        ///         public object Order_Only { get; set; }
        ///     
        ///         [Restriction("aliasclasse.Id", Restriction.Between)]
        ///         public int? Id_de { get; set; }
        ///     
        ///         [Restriction("aliasclasse.Id", Restriction.Between)]
        ///         public int? Id_ate { get; set; }
        ///     }
        ///      
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <param name="criteria"></param>
        /// <param name="qtd"></param>
        /// <param name="value1"></param>
        /// <param name="property1"></param>
        private void DealBetweens(object value, string property, Restriction restriction, T criteria, 
            ref int qtd, ref object value1, ref string property1)
        {
            object value2 = null;
            string property2 = String.Empty;
            //entra nesse if quando o primeiro restriction between é encontrado
            //armazena em variaveis e segue para proxima propriedade
            if (qtd == 0)
            {
                value1 = value;
                property1 = property;
                qtd++;
            }
            //entra nesse if na propriedade seguinte que deve tambem estar anotada com restricao between
            //armazena em variaveis que serao utilizadas para compor o between
            else if (qtd == 1)
            {
                value2 = value;
                property2 = property;
                qtd++;
            }

            //entra nesse if depois que passou pela segunda propriedade
            //e adiciona o between no criterio de busca 
            if (qtd == 2)
            {
                //verifica se as 2 propriedades sao a mesma e nao sao vazias
                if (property1.Equals(property2) && (!String.IsNullOrWhiteSpace(property1)))
                {
                    AddBetween(value1, value2, restriction, property1, criteria);
                }
                //apaga os valores das variaveis
                property1 = String.Empty;
                property2 = String.Empty;
                value1 = null;
                value2 = null;
                qtd = 0;
            }
        }
        
        /// <summary>
        /// Adiciona no criteria a restricao 
        /// 
        /// Between 'valor1' menor ou igual 'propriedade' menor ou igual 'valor2'
        /// 
        /// *caso valor1 seja null entao 'propriedade' menor ou igual 'valor2'
        /// 
        /// **caso valor2 seja null entao 'propriedade' maior ou igual 'valor1'
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="property"></param>
        /// <param name="criteria"></param>
        public void AddBetween(object value1, object value2, Restriction restriction, string property, T criteria)
        {
            if (String.IsNullOrWhiteSpace(property) || criteria == null)
            {
                return;
            }

            if (value1 != null)
            {
                if (value2 != null)
                {
                    // Se ambas datas forem preenchidas, cria um 'BETWEEN'                 
                    //IDictionary<Restriction, object> restriction = new Dictionary<Restriction, object>();
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

                    AddRestriction(values, restriction, property, criteria);
                }
                else
                {
                    // Se apenas "valor de" for preenchida, cria um '>='
                    Restriction ge;
                    if (Restriction.BetweenDateTime.Equals(restriction))
                    {
                        ge = Restriction.GeDateTime;
                    }
                    else if (value1 != null && value1.GetType().ToString().Contains("DateTime"))
                    {
                        ge = Restriction.GeDate;
                    }
                    else
                    {
                        ge = Restriction.Ge;
                    }
                    AddRestriction(value1, ge, property, criteria);
                }
            }
            else if (value2 != null)
            {
                // Se apenas "valor até" for preenchida, cria um '<=' Restriction ge;
                Restriction le;
                if (Restriction.BetweenDateTime.Equals(restriction))
                {
                    le = Restriction.LeDateTime;
                }
                else if (value2 != null && value2.GetType().ToString().Contains("DateTime"))
                {
                    le = Restriction.LeDate;
                }
                else
                {
                    le = Restriction.Le;
                }
                AddRestriction(value1, le, property, criteria);
            }
        }
        #endregion

        #region - Using FieldsFilter
        /// <summary>
        /// Prrenche o criteria de acordo com os filtros passados
        /// </summary>
        /// <param name="aliasJoin">Alias para inner joins</param>
        /// <param name="aliasJoinTyped">Alias para os joins definidos</param>
        /// <param name="fieldsFilter">Propriedades comparadas com equal aos valores <"propriedade", valor> </param>
        /// <param name="fieldsFilterWithRestriction">Propriedades comparadas com a restricao definida em relacao aos valores 
        /// <"propriedade", <Restricao, Valor>></param>
        /// <param name="fieldsOrder"></param>
        /// <param name="criteria"></param>
        public void FillSearchCriteria(
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder,
            T criteria)
        {
            // adiciona inners joins
            if (aliasJoin != null)
            {
                AddInnerJoins(aliasJoin, criteria);
            } // end if

            // adiciona joins definidos
            if (aliasJoinTyped != null)
            {
                AddJoins(aliasJoinTyped, criteria);
            } // end if

            // adiciona restricao equals
            if (fieldsFilter != null)
            {
                AddRestrictionsEq(fieldsFilter, criteria);
            }

            //adiciona restricao definida
            if (fieldsFilterWithRestriction != null)
            {
                AddRestrictions(fieldsFilterWithRestriction, criteria);
            }
            // end else if

            //adiciona ordenacao
            if (fieldsOrder != null)
            {
                AddOrders(fieldsOrder, criteria);
            } // end if

        }

        /// <summary>
        /// Cria os Joins no criteria <propriedade, <alias, JoinType>>
        /// </summary>
        /// <param name="aliasJoin"></param>
        /// <param name="criteria"></param>
        public void AddJoins(IDictionary<string, IDictionary<string, JoinType>> aliasJoin, T criteria)
        {
            ICollection<string> properties = aliasJoin.Keys;
            // Carrega propriedades tables
            foreach (string property in properties)
            {
                IDictionary<string, JoinType> join = aliasJoin[property];
                ICollection<string> aliasList = join.Keys;
                foreach (string alias in aliasList)
                {
                    AddJoin(property, alias, join[alias], criteria);
                } // end for
            } // end for
        }

        /// <summary>
        /// Cria os innerJoins no criteria <propriedade, alias>
        /// </summary>
        /// <param name="aliasJoin"></param>
        /// <param name="criteria"></param>
        public void AddInnerJoins(IDictionary<string, string> aliasJoin, T criteria)
        {
            ICollection<string> properties = aliasJoin.Keys;
            // Carrega propriedades tables
            foreach (string property in properties)
            {
                string alias = aliasJoin[property];

                AddJoin(property, alias, JoinType.InnerJoin, criteria);
            } // end for
        }

        /// <summary>
        /// Adiciona ordenacao no criteria <propriedade, Order>
        /// </summary>
        /// <param name="fieldsOrder"></param>
        /// <param name="criteria"></param>
        public void AddOrders(IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder, T criteria)
        {
            ICollection<string> properties = fieldsOrder.Keys;

            // Load orders
            foreach (string property in properties)
            {
                AddOrder(property, fieldsOrder[property], criteria);
            } // end for
        }

        /// <summary>
        /// Adiciona restricoes equal no criteria <propriedade, valor>
        /// </summary>
        /// <param name="fieldsFilter"></param>
        /// <param name="criteria"></param>
        public void AddRestrictionsEq(IDictionary<string, object> fieldsFilter, T criteria)
        {
            ICollection<string> properties = fieldsFilter.Keys;

            foreach (string property in properties)
            {
                AddEqual(fieldsFilter[property], property, criteria);
            } // end for
        }

        /// <summary>
        /// Adiciona restricoes not in no criteria <propriedade, valores>
        /// </summary>
        /// <param name="fieldsFilterNotIn"></param>
        /// <param name="criteria"></param>
        public void AddRestrictionsNotIn(IDictionary<string, int[]> fieldsFilterNotIn, T criteria)
        {
            ICollection<string> properties = fieldsFilterNotIn.Keys;
            foreach (string property in properties)
            {
                AddRestriction(fieldsFilterNotIn[property], Restriction.NotIn, property, criteria);
            } // end for
        }

        /// <summary>
        /// Adicionar restricoes no criteria <propriedade, <restricao, valor>>
        /// </summary>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="criteria"></param>
        public void AddRestrictions(IDictionary<string,
                    IDictionary<Restriction, object>> fieldsFilterWithRestriction, T criteria)
        {
            ICollection<string> properties = fieldsFilterWithRestriction.Keys;
            foreach (string property in properties)
            {
                IDictionary<Restriction, object> restrictions = fieldsFilterWithRestriction[property];

                // Read restrictions
                foreach (Restriction keyRestriction in restrictions.Keys)
                {
                    object value = restrictions[keyRestriction];

                    AddRestriction(value, keyRestriction, property, criteria);
                } // end for                        
            } // end for
        }
        #endregion
        
        /// <summary>
        /// Método que ira efetivamente adicionar o join na consulta
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="joinType"></param>
        /// <param name="criteria"></param>
        public abstract void AddJoin(string value1, string value2, JoinType joinType, T criteria);

        /// <summary>
        /// Método qeu ira efetivamente adicionar a ordenacao na consulta
        /// </summary>
        /// <param name="property"></param>
        /// <param name="order"></param>
        /// <param name="criteria"></param>
        public abstract void AddOrder(string property, Order order, T criteria);

        /// <summary>
        /// Adiciona restricao equals no criteria
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <param name="criteria"></param>
        public void AddEqual(object value, string property, T criteria)
        {
            if (String.IsNullOrWhiteSpace(property) || criteria == null)
            {
                return;
            }

            Restriction restriction;
            if (value != null && value.GetType().ToString().Contains("DateTime"))
            {
                restriction = Restriction.EqDate;
            }
            else
            {
                restriction = Restriction.Eq;
            }
            AddRestriction(value, restriction, property, criteria);
        }

        /// <summary>
        /// Método que adiciona a restricao na consulta
        /// </summary>
        /// <param name="value"></param>
        /// <param name="restriction"></param>
        /// <param name="property"></param>
        /// <param name="criteria"></param>
        public void AddRestriction(object value, Restriction restriction, string property, T criteria)
        {
            switch (restriction)
            {
                case Restriction.Like:
                    AddLike(value, property, criteria);
                    break;
                case Restriction.LikeLeft:
                    AddLikeLeft(value, property, criteria);
                    break;
                case Restriction.LikeRight:
                    AddLikeRight(value, property, criteria);
                    break;
                case Restriction.NotEq:
                    AddNotEq(value, property, criteria);
                    break;
                case Restriction.Eq:
                    AddEq(value, property, criteria);
                    break;
                case Restriction.EqDate:
                    AddEqDate(value, property, criteria);
                    break;
                case Restriction.EqDateTime:
                    AddEqDateTime(value, property, criteria);
                    break;
                case Restriction.Ge:
                    AddGe(value, property, criteria);
                    break;
                case Restriction.GeDate:
                    AddGeDate(value, property, criteria);
                    break;
                case Restriction.GeDateTime:
                    AddGeDateTime(value, property, criteria);
                    break;
                case Restriction.Le:
                    AddLe(value, property, criteria);
                    break;
                case Restriction.LeDate:
                    AddLeDate(value, property, criteria);
                    break;
                case Restriction.LeDateTime:
                    AddLeDateTime(value, property, criteria);
                    break;
                case Restriction.ExtractDay:
                    AddExtractDay(value, property, criteria);
                    break;
                case Restriction.ExtractMonth:
                    AddExtractMonth(value, property, criteria);
                    break;
                case Restriction.ExtractYear:
                    AddExtractYear(value, property, criteria);
                    break;
                case Restriction.IsNull:
                    AddIsNull(value, property, criteria);
                    break;
                case Restriction.IsNotNull:
                    AddIsNotNull(value, property, criteria);
                    break;
                case Restriction.Between:
                    AddBetween(value, property, criteria);
                    break;
                case Restriction.BetweenDateTime:
                    AddBetweenDateTime(value, property, criteria);
                    break;
                case Restriction.In:
                    AddIn(value, property, criteria);
                    break;
                case Restriction.NotIn:
                    AddNotIn(value, property, criteria);
                    break;
            } // end case
        }

        #region - Métodos abstratos que adicionam as restricoes espeficicas para cada item previsto no enum de restricao
        public abstract void AddBetween         (object value, string property, T criteria);
        public abstract void AddBetweenDateTime (object value, string property, T criteria);
        public abstract void AddEq              (object value, string property, T criteria);
        public abstract void AddEqDate          (object value, string property, T criteria);
        public abstract void AddEqDateTime      (object value, string property, T criteria);
        public abstract void AddExtractDay      (object value, string property, T criteria);
        public abstract void AddExtractMonth    (object value, string property, T criteria);
        public abstract void AddExtractYear     (object value, string property, T criteria);
        public abstract void AddGe              (object value, string property, T criteria);
        public abstract void AddGeDate          (object value, string property, T criteria);
        public abstract void AddGeDateTime      (object value, string property, T criteria);
        public abstract void AddIn              (object value, string property, T criteria);
        public abstract void AddIsNotNull       (object value, string property, T criteria);
        public abstract void AddIsNull          (object value, string property, T criteria);
        public abstract void AddLe              (object value, string property, T criteria);
        public abstract void AddLeDate          (object value, string property, T criteria);
        public abstract void AddLeDateTime      (object value, string property, T criteria);
        public abstract void AddLike            (object value, string property, T criteria);
        public abstract void AddLikeLeft        (object value, string property, T criteria);
        public abstract void AddLikeRight       (object value, string property, T criteria);
        public abstract void AddNotEq           (object value, string property, T criteria);
        public abstract void AddNotIn           (object value, string property, T criteria);
        #endregion

    }
}
