using NetUtil.Util.Enums;
using NetUtil.Util.Expression;
using NetUtil.Util.Filter;
using NetUtil.Util.Filter.Attributes;
using NetUtil.Util.Helper;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Hibernate
{
    public class CriteriaUtil : AbstractFilterUtil<ICriteria>
    {
        private static CriteriaUtil _criteriaUtil;
        
        private CriteriaUtil() { }

        public static CriteriaUtil GetInstance()
        {
            if (_criteriaUtil == null)
            {
                _criteriaUtil = new CriteriaUtil();
            }
            return _criteriaUtil;
        }

        /// <summary>
        /// Criar o criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <returns></returns>
        public ICriteria CreateSearchCriteria<T>(ISession session)
        {
            return CreateSearchCriteria<T>(null, session);
        }

        /// <summary>
        /// Criar o criteria com alias para entidade <T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public ICriteria CreateSearchCriteria<T>(string aliasToT, ISession session)
        {
            ICriteria criteria = null;

            if (String.IsNullOrWhiteSpace(aliasToT))
            {
                criteria = session.CreateCriteria(typeof(T));
            }
            else
            {
                criteria = session.CreateCriteria(typeof(T), aliasToT);
            }

            return criteria;
        }

        /// <summary>
        /// Executa o criteria.List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<T> ExecuteSearch<T>(ICriteria criteria)
        {
            return ExecuteSearch<T>(null, null, criteria);
        }

        /// <summary>
        /// Executa o criteria.Future<T> caso sejam fornecidas as informacoes de paginacao
        /// caso contrario executa criteria.List<T> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<T> ExecuteSearch<T>(int? pageSize, int? currentPage, ICriteria criteria)
        {
            IList<T> lista = null;
            //Controla de Paginação para Grid
            if (pageSize != null && currentPage != null)
            {
                criteria.SetFirstResult((currentPage.Value - 1) * pageSize.Value);
                criteria.SetMaxResults(pageSize.Value);
                lista = new List<T>(criteria.Future<T>());
            }
            //Sem controle de paginação
            else
            {
                // Exec List
                lista = (IList<T>)criteria.List<T>();
            }

            return lista;
        }

        /// <summary>
        /// Adiciona uma ordenacao ao criteria
        /// </summary>
        /// <param name="property"></param>
        /// <param name="fieldsOrder"></param>
        /// <param name="criteria"></param>
        public override void AddOrder(string property, NetUtil.Util.Enums.Order fieldsOrder, ICriteria criteria)
        {
            NHibernate.Criterion.Order order = GetNHibernateOrder(property, fieldsOrder);

            if (order != null)
            {
                criteria.AddOrder(order);                
            }
        }

        /// <summary>
        /// Método que faz o De->Para do Order do netUtil para o Order do Nhibernate
        /// </summary>
        /// <param name="property"></param>
        /// <param name="fieldsOrder"></param>
        /// <returns></returns>
        private NHibernate.Criterion.Order GetNHibernateOrder(string property, Enums.Order fieldsOrder)
        {
            NHibernate.Criterion.Order order = null;
            switch (fieldsOrder)
            {
                case NetUtil.Util.Enums.Order.Asc:
                    order = NHibernate.Criterion.Order.Asc(property);
                    break;
                case NetUtil.Util.Enums.Order.Desc:
                    order = NHibernate.Criterion.Order.Desc(property);
                    break;
            } // end case
            return order;
        }

        /// <summary>
        /// Adiciona um join no criteria
        /// </summary>
        /// <param name="property"></param>
        /// <param name="alias"></param>
        /// <param name="join"></param>
        /// <param name="criteria"></param>
        public override void AddJoin(string property, string alias, JoinType join, ICriteria criteria)
        {
            NHibernate.SqlCommand.JoinType joinType = GetNHibernateJoinType(join);

            criteria.CreateAlias(property, alias, joinType);
        }

        /// <summary>
        /// Método qeu faz o de->para dos tipos de join do netUtil para o tipo de join do Nhibernate
        /// </summary>
        /// <param name="join"></param>
        /// <returns></returns>
        private NHibernate.SqlCommand.JoinType GetNHibernateJoinType(JoinType join)
        {
            NHibernate.SqlCommand.JoinType joinType;
            switch (join)
            {
                case JoinType.InnerJoin:
                    joinType = NHibernate.SqlCommand.JoinType.InnerJoin;
                    break;
                case JoinType.LeftOuterJoin:
                    joinType = NHibernate.SqlCommand.JoinType.LeftOuterJoin;
                    break;
                case JoinType.RightOuterJoin:
                    joinType = NHibernate.SqlCommand.JoinType.RightOuterJoin;
                    break;
                case JoinType.FullJoin:
                    joinType = NHibernate.SqlCommand.JoinType.FullJoin;
                    break;
                default:
                    joinType = NHibernate.SqlCommand.JoinType.None;
                    break;
            }
            return joinType;
        }

        #region - Implementacao dos métodos abstratos que adicionam as restricoes espeficicas para cada item previsto no enum de restricao
        public override void AddBetweenDateTime(object value, string property, ICriteria criteria)
        {
            ICriterion nhCriterion = null;

            // Validate value is IList<object>
            if (value is IList<object>)
            {
                IList<object> values = (IList<object>)value;
                nhCriterion = GetNHibernateBetweenCriterion(values, property, NHibernateUtil.DateTime);
            }

            if (nhCriterion != null)
            {
                criteria.Add(nhCriterion);
            }
        }

        public override void AddBetween(object value, string property, ICriteria criteria)
        {
            ICriterion nhCriterion = null;

            // Validate value is IList<object>
            if (value is IList<object>)
            {
                IList<object> values = (IList<object>)value;
                nhCriterion = GetNHibernateBetweenCriterion(values, property, null);
            }

            if (nhCriterion != null)
            {
                criteria.Add(nhCriterion);
            }
        }

        /// <summary>
        /// Métodoq eu retorna o ICriterion de between para ser adicionado no criteria
        /// </summary>
        /// <param name="values"></param>
        /// <param name="property"></param>
        /// <param name="castType"></param>
        /// <returns></returns>
        private ICriterion GetNHibernateBetweenCriterion(IList<object> values, string property, NHibernate.Type.NullableType castType)
        {
            ICriterion nhCriterion = null;
            // Validate list is not null and count == 2
            if (values != null && values.Count == 2)
            {
                if (castType != null)
                {
                    nhCriterion = Restrictions.Between(Projections.Cast(castType, Projections.Property(property)), ((DateTime)values[0]),
                        ((DateTime)values[1]));
                }
                else if (values[0] is DateTime && values[1] is DateTime)
                {
                    nhCriterion = Restrictions.Between(Projections.Cast(NHibernateUtil.Date, Projections.Property(property)), ((DateTime)values[0]),
                        ((DateTime)values[1]));
                }
                else
                {
                    nhCriterion = Restrictions.Between(Projections.Property(property), values[0], values[1]);
                } // end else
            }

            return nhCriterion;
        }

        public override void AddEq(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Eq(property, value));
        }

        public override void AddEqDate(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Eq(Projections.Cast(NHibernateUtil.Date, Projections.Property(property)), (DateTime)value));
        }

        public override void AddEqDateTime(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Eq(Projections.Cast(NHibernateUtil.DateTime, Projections.Property(property)), (DateTime)value));
        }

        public override void AddExtractYear(object value, string property, ICriteria criteria)
        {
            criteria.Add(new ExtractDateFunctionExpression(property, ExtractType.YEAR, (int)value));
        }

        public override void AddExtractMonth(object value, string property, ICriteria criteria)
        {
            criteria.Add(new ExtractDateFunctionExpression(property, ExtractType.MONTH, (int)value));
        }

        public override void AddExtractDay(object value, string property, ICriteria criteria)
        {
            criteria.Add(new ExtractDateFunctionExpression(property, ExtractType.DAY, (int)value));
        }

        public override void AddGe(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Ge(property, value));
        }

        public override void AddGeDate(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(property)), (DateTime)value));
        }

        public override void AddGeDateTime(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Ge(Projections.Cast(NHibernateUtil.DateTime, Projections.Property(property)), (DateTime)value));
        }

        public override void AddIn(object value, string property, ICriteria criteria)
        {
            if (value is IList<object>)
            {
                IList<object> values = (IList<object>)value;
                criteria.Add(Restrictions.In(property, values.ToArray()));
            }
        }

        public override void AddIsNotNull(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.IsNotNull(property));
        }

        public override void AddIsNull(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.IsNull(property));
        }

        public override void AddLe(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Le(property, value));
        }

        public override void AddLeDate(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Le(Projections.Cast(NHibernateUtil.Date, Projections.Property(property)), (DateTime)value));
        }

        public override void AddLeDateTime(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Le(Projections.Cast(NHibernateUtil.DateTime, Projections.Property(property)), (DateTime)value));
        }

        public override void AddLike(object value, string property, ICriteria criteria)
        {
            string likeBusca = (string)value;
            criteria.Add(Restrictions.Like(property, likeBusca, MatchMode.Anywhere));
        }

        public override void AddLikeLeft(object value, string property, ICriteria criteria)
        {
            string likeBuscaLeft = (string)value;
            criteria.Add(Restrictions.Like(property, "%" + likeBuscaLeft));
        }

        public override void AddLikeRight(object value, string property, ICriteria criteria)
        {
            string likeBuscaRight = (string)value;
            criteria.Add(Restrictions.Like(property, likeBuscaRight + "%"));
        }

        public override void AddNotEq(object value, string property, ICriteria criteria)
        {
            criteria.Add(Restrictions.Not(Restrictions.Eq(property, value)));
        }

        public override void AddNotIn(object value, string property, ICriteria criteria)
        {
            if (value is IList<object>)
            {
                IList<object> values = (IList<object>)value;
                criteria.Add(Restrictions.Not(Restrictions.In(property, values.ToArray())));
            }
        }
        #endregion
    }
}