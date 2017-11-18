using NetUtil.Util.Enums;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetUtil.Util.Expression
{
    public class ExtractDateFunctionExpression : ICriterion {
        private String      _property;
        private int         _value;
        private ExtractType _extractType;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="property"></param>
        /// <param name="extractType"></param>
        /// <param name="value"></param>
        public ExtractDateFunctionExpression(String property, ExtractType extractType, int value) {
            this._property = property;
            this._extractType = extractType;
            this._value = value;
        }

        /// <summary>
        /// GetProjections
        /// </summary>
        /// <returns></returns>
        public IProjection[] GetProjections() {
            IProjection[] projections = { Projections.Cast(NHibernateUtil.Date, Projections.Property(_property)) };
            return projections;
        }

        /// <summary>
        /// GetTypedValues
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criteriaQuery"></param>
        /// <returns></returns>
        public TypedValue[] GetTypedValues(NHibernate.ICriteria criteria, ICriteriaQuery criteriaQuery) {
            return new TypedValue[] { new TypedValue(NHibernateUtil.String, _value, EntityMode.Poco) };
        }

        /// <summary>
        /// Cria a Expression para extrair a DD, MM, YYYY da data
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criteriaQuery"></param>
        /// <param name="enabledFilters"></param>
        /// <returns></returns>
        public SqlString ToSqlString(NHibernate.ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, NHibernate.IFilter> enabledFilters) {
            String column = criteriaQuery.GetColumnsUsingProjection(criteria, _property)[0];

            // Cria string extract function
            StringBuilder sb = new StringBuilder();
            sb.Append(" (").Append(_extractType.ToString()).Append(" (").Append(column).Append(") = ").Append(_value).Append(") ");

            // Retorna string
            return new SqlString(sb.ToString());
        }
    }
}
