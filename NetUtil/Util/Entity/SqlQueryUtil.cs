using MySql.Data.MySqlClient;
using NetUtil.Util.Enums;
using NetUtil.Util.Filter;
using NetUtil.Util.Filter.Attributes;
using NetUtil.Util.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Entity
{
    public class SqlQueryUtil : AbstractFilterUtil<StringBuilder>
    {
        private static SqlQueryUtil _sqlQueryUtil;

        private SqlQueryUtil() { }

        public static SqlQueryUtil GetInstance()
        {
            if (_sqlQueryUtil == null)
            {
                _sqlQueryUtil = new SqlQueryUtil();
            }
            return _sqlQueryUtil;
        }

        /// <summary>
        /// Método que adiciona o join na consulta
        /// </summary>
        /// <param name="property"></param>
        /// <param name="alias"></param>
        /// <param name="joinType"></param>
        /// <param name="criteria"></param>
        public override void AddJoin(string property, string alias, Enums.JoinType joinType, StringBuilder criteria)
        {
            string[] propertySplited = property.Split('.');
            if ((propertySplited.Length == 3) && (AddJoinType(joinType, criteria)))
            {
                string classAlias = property.Split('.')[0];
                string table = propertySplited[1];
                string tableId = propertySplited[2];
                AddJoinTable(classAlias, table, tableId, alias, criteria);
            }
        }

        /// <summary>
        /// Metodo auxiliar ao AddJoin para adiconar o join efetivamente
        /// </summary>
        /// <param name="classAlias"></param>
        /// <param name="table"></param>
        /// <param name="tableId"></param>
        /// <param name="alias"></param>
        /// <param name="criteria"></param>
        private void AddJoinTable( string classAlias, string table, string tableId, string alias, StringBuilder criteria)
        {
            criteria.Append(table).Append(" AS ").Append(alias).
                     Append(" ON ").Append(alias).Append(".ID = ").Append(classAlias).Append(".").Append(tableId);
        }

        /// <summary>
        /// Método auxliar ao AddJoin para adicionar o inicio do join fazenro o de-> do tipo de join do netUtil para o string SQL
        /// </summary>
        /// <param name="joinType"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private bool AddJoinType(Enums.JoinType joinType, StringBuilder criteria)
        {
            string join;
            switch (joinType)
            {
                case NetUtil.Util.Enums.JoinType.InnerJoin:
                    join = " INNER JOIN ";
                    break;
                case NetUtil.Util.Enums.JoinType.LeftOuterJoin:
                    join = " LEFT OUTER JOIN ";
                    break;
                case NetUtil.Util.Enums.JoinType.RightOuterJoin:
                    join = " RIGHT OUTER JOIN ";
                    break;
                case NetUtil.Util.Enums.JoinType.FullJoin:
                    join = " FULL JOIN ";
                    break;
                default:
                    join = string.Empty;
                    return false;
            }

            criteria.Append(join);
            return true;
        }

        /// <summary>
        /// Método que adiciona a ordenacao na consulta
        /// </summary>
        /// <param name="property"></param>
        /// <param name="order"></param>
        /// <param name="criteria"></param>
        public override void AddOrder(string property, Enums.Order order, StringBuilder criteria)
        {
            if (!criteria.ToString().Contains("ORDER"))
            {
                criteria.Append(" ORDER BY ");
            }
            else
            {
                criteria.Append(" , ");
            }
            criteria.Append(property).Append(" ").Append(order.ToString());
        }

        /// <summary>
        /// Inicia a construcao do Sql para a consulta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mainAlias"></param>
        /// <returns></returns>
        public StringBuilder CreateSearchCriteria<T>(string mainAlias)
        {
            StringBuilder returns = new StringBuilder();
            returns.Append(" SELECT ");

            if (string.IsNullOrWhiteSpace(mainAlias))
            {
                returns.Append(" * FROM ").Append(typeof(T).Name);
            }
            else
            {
                returns.Append(mainAlias).Append(".* FROM ").Append(typeof(T).Name).Append(" as ").Append(mainAlias);
            }
            
            returns.Append(" ");

            return returns;
        }

        /// <summary>
        /// Método qeu popula a lista de parametros da sql query parametrizada
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="parameters"></param>
        /// <param name="parameterType"></param>
        public void GetQueryParameters(object filter, List<DbParameter> parameters, Type parameterType)
        {
            foreach (var property in filter.GetType().GetProperties())
            {
                RestrictionAttribute restriction = ClassAttributeUtil.RestrictionAtttribute(property);

                if (restriction == null ||
                    String.IsNullOrWhiteSpace(restriction.Property))
                {
                    continue;
                }

                // Add parameter, case exists restriction
                if (restriction.Restriction == null)
                {
                    continue;
                }

                object value = property.GetValue(filter, null);

                AlterValueIfIsLike(restriction.Restriction.Value, value);

                AddParameter(parameters, parameterType, restriction.Property, value);

            }
        }

        private static void AlterValueIfIsLike(Restriction restriction, object value)
        {
            if (Restriction.Like.Equals(restriction))
            {
                value = '%' + (string)value + '%';
            }
            else if (Restriction.LikeLeft.Equals(restriction))
            {
                value = '%' + (string)value;
            }
            else if (Restriction.LikeRight.Equals(restriction))
            {
                value = (string)value + '%';
            }
        }
        
        /// <summary>
        /// Método qeu popula a lista de parametros da sql query parametrizada
        /// </summary>
        /// <param name="fieldsFilter"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="parameters"></param>
        /// <param name="parameterType"></param>
        public void GetQueryParameters(IDictionary<string, object> fieldsFilter, IDictionary<string, IDictionary<Enums.Restriction, object>> fieldsFilterWithRestriction, List<DbParameter> parameters, Type parameterType)
        {
            if (fieldsFilter != null && fieldsFilter.Count > 0)
            {
                foreach (string property in fieldsFilter.Keys)
                {
                    object value = fieldsFilter[property];

                    AddParameter(parameters, parameterType, property, value);
                }
            }
            if (fieldsFilterWithRestriction != null && fieldsFilterWithRestriction.Count > 0)
            {
                foreach (string property in fieldsFilterWithRestriction.Keys)
                {
                    IDictionary<Enums.Restriction, object> restrictions = fieldsFilterWithRestriction[property];
                    foreach (Enums.Restriction restriction in restrictions.Keys)
                    {
                        object value = restrictions[restriction];

                        AlterValueIfIsLike(restriction, value);

                        AddParameter(parameters, parameterType, property, value);
                    }
                }
            }
        }

        /// <summary>
        /// Método qeu adiciona um parametro na lista de parametros
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parameterType"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private void AddParameter(List<DbParameter> parameters, Type parameterType, string property, object value)
        {
            DbParameter parameter = null;

            object o = Activator.CreateInstance(parameterType);

            string parameterProperty = GetParameterProperty(property);

            if (o is SqlParameter)
            {
                parameter = new SqlParameter(parameterProperty, value);
            }
            else if (o is MySqlParameter)
            {
                parameter = new MySqlParameter(parameterProperty, value);
            }

            if (parameter != null)
            {
                parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Método qeu executa a query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> ExecuteQuery<T>(DbSqlQuery<T> query) where T: class
        {
            return ExecuteQuery<T>(null,null, query);
        }

        /// <summary>
        /// Método que executa a query fazendo paginacao
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> ExecuteQuery<T>(int? pageSize, int? currentPage, DbSqlQuery<T> query) where T : class
        {
            if (currentPage != null && pageSize != null)
            {
                int skipRows = (currentPage.Value - 1) * pageSize.Value;
                return query.Skip(skipRows).Take(pageSize.Value).AsQueryable().ToList();
            }
            else
            {
                return query.AsQueryable().ToList();
            }
        }

        #region - Implementacao dos métodos abstratos que adicionam as restricoes espeficicas para cada item previsto no enum de restricao
        public override void AddBetween(object value, string property, StringBuilder criteria)
        {
            throw new NotImplementedException();
        }

        public override void AddBetweenDateTime(object value, string property, StringBuilder criteria)
        {
            throw new NotImplementedException();
        }

        public override void AddEq(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" = ").Append("@").Append(parameterProperty).Append(" ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddEqDate(object value, string property, StringBuilder criteria)
        {
            AddEq(value, property, criteria);
        }

        public override void AddEqDateTime(object value, string property, StringBuilder criteria)
        {
            AddEq(value, property, criteria);
        }

        public override void AddExtractYear(object value, string property, StringBuilder criteria)
        {
            throw new NotImplementedException();
        }

        public override void AddExtractMonth(object value, string property, StringBuilder criteria)
        {
            throw new NotImplementedException();
        }

        public override void AddExtractDay(object value, string property, StringBuilder criteria)
        {
            throw new NotImplementedException();
        }

        public override void AddGe(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" >= ").Append("@").Append(parameterProperty).Append(" ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddGeDate(object value, string property, StringBuilder criteria)
        {
            AddGe(value, property, criteria);
        }

        public override void AddGeDateTime(object value, string property, StringBuilder criteria)
        {
            AddGe(value, property, criteria);
        }

        public override void AddIn(object value, string property, StringBuilder criteria)
        {
            throw new NotImplementedException();
        }

        public override void AddIsNotNull(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" IS NOT NULL ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddIsNull(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" IS NULL ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddLe(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" <= ").Append("@").Append(parameterProperty).Append(" ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddLeDate(object value, string property, StringBuilder criteria)
        {
            AddLe(value, property, criteria);
        }

        public override void AddLeDateTime(object value, string property, StringBuilder criteria)
        {
            AddLe(value, property, criteria);
        }

        public override void AddLike(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" LIKE ").Append("@").Append(parameterProperty).Append(" ").ToString();
            AddRestriction(strRestriction.ToString(), criteria);
        }

        public override void AddLikeLeft(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" LIKE ").Append("@").Append(parameterProperty).Append(" ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddLikeRight(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" LIKE ").Append("@").Append(parameterProperty).Append(" ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddNotEq(object value, string property, StringBuilder criteria)
        {
            string parameterProperty = GetParameterProperty(property);
            string strRestriction = new StringBuilder().
                Append(property).Append(" != ").Append("@").Append(parameterProperty).Append(" ").ToString();
            AddRestriction(strRestriction, criteria);
        }

        public override void AddNotIn(object value, string property, StringBuilder criteria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtem o string utilizado para parametrizar
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private string GetParameterProperty(string property)
        {
            string[] propertySplited = property.Split('.');
            string parameterProperty = propertySplited.Length > 1 ? propertySplited[1] : propertySplited[0];
            return parameterProperty;
        }

        /// <summary>
        /// Adiciona a restricao AND ao sql que esta sendo construido, se nao foi adicionado restricoa ainda entao adiciona WHERE
        /// </summary>
        /// <param name="strRestriction"></param>
        /// <param name="criteria"></param>
        private void AddRestriction(string strRestriction, StringBuilder criteria)
        {
            if (!string.IsNullOrWhiteSpace(strRestriction) && criteria != null)
            {
                if (!criteria.ToString().Contains("WHERE"))
                {
                    criteria.Append(" WHERE ");
                }
                else
                {
                    criteria.Append(" AND ");
                }

                criteria.Append(strRestriction);
            }
        }
        #endregion
    }
}
