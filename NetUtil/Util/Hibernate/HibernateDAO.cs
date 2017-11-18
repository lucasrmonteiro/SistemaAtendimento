using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NetUtil.Util.Enums;
using NetUtil.Util.Expression;
using NetUtil.Util.Helper;
using NetUtil.Util.Hibernate.Interfaces;
using NetUtil.Util.Filter;
using NetUtil.Util.DTO;

namespace NetUtil.Util.Hibernate {
    [Serializable]
    public class HibernateDAO : IDAO {

        /// <summary>
        /// Utilizades para uso do ICriteria
        /// </summary>
        private CriteriaUtil _criteriaUtil = CriteriaUtil.GetInstance();
                
        /// <summary>
        /// Inclui objeto
        /// </summary>
        /// <typeparam name="T">Objeto a ser incluído</typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object Incluir<T>(T entity) {
            object identifier;
            ITransaction tx  = null;
            ISession session = null;

            try {
                // Obtêm session
                session = HibernateUtil.GetCurrentSession();

                // Abre transação
                tx = session.BeginTransaction();

                // Persiste objeto
			    identifier = session.Save(entity);

			    tx.Commit();            
            } catch (Exception e) {
                if (tx != null) 
                    tx.Rollback();
                throw new SystemException(e.Message, e);
            } finally {
                HibernateUtil.CloseSession(session);
            } // end try

            // Retorna chave        
            return identifier;
	    }

        /// <summary>
        /// Inclui objeto
        /// </summary>
        /// <typeparam name="T">Objeto a ser incluído</typeparam>
        /// <param name="entity"></param>
        /// <param name="pk"></param>
        public void Incluir<T, K>(T entity, K pk) {
            ITransaction tx  = null;
            ISession session = null;

            try {
                // Obtêm session
                session = HibernateUtil.GetCurrentSession();

                // Abre transação
                tx = session.BeginTransaction();

                // Persiste objeto
			    session.Save(entity, pk);
			    tx.Commit();            
            } catch (Exception e) {
                if (tx != null) 
                    tx.Rollback();
                throw new SystemException(e.Message, e);
            }
            finally
            {
                HibernateUtil.CloseSession(session);
            } // end try
	    }
        
        /// <summary>
        /// Altera objeto
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="noTransaction"></param>
        public void Alterar(object entity) {
            ITransaction tx = null;
            ISession session = null;

            try {
                // Obtêm session
                session = HibernateUtil.GetCurrentSession();

                // Abre transação
                tx = session.BeginTransaction();

                // Persiste objeto
                session.Merge(entity);

                // Commit transaction
                tx.Commit();
            } catch (Exception e) {
                if (tx != null)
                    tx.Rollback();
                throw new SystemException(e.Message, e);
            }
            finally
            {
                HibernateUtil.CloseSession(session);
            } // end try
        }

        /// <summary>
        /// Exclui objeto
        /// </summary>
        /// <typeparam name="T">Objeto a ser excluído</typeparam>
        /// <param name="entity"></param>
        public void Excluir<T>(T entity) {
            ITransaction tx = null;
            ISession session = null;
            
            try {
                // Obtêm session
                session = HibernateUtil.GetCurrentSession();

                // Abre transação
                tx = session.BeginTransaction();

                // Persiste objeto
			    session.Delete(entity);
			    tx.Commit();            
            } catch (Exception e) {
                if (tx != null) 
                    tx.Rollback();
                throw new SystemException(e.Message, e);
            }
            finally
            {
                HibernateUtil.CloseSession(session);
            } // end try
	    }

        /// <summary>
        /// Executa uma query com um parametro (um filtro "where")
        /// </summary>
        /// <param name="parameter">Paramentro usado na condicao where</param>
        /// <param name="table">Tabela de exucaçao da query</param>
        /// <param name="query">String da query</param>
        public void ExecuteQuery(object table, object parameter, string query)
        {
            ISession session = null;
            ITransaction tx = null;
            var queryExecute = string.Format(query, table);

            try
            {
                // Obtêm session
                session = HibernateUtil.GetCurrentSession();
                // Abre transação
                tx = session.BeginTransaction();

                session.CreateQuery(queryExecute)
                   .SetParameter("parameter", parameter)
                   .ExecuteUpdate();

            tx.Commit();            
            }
            catch (Exception e)
            {
                if (tx != null)
                    tx.Rollback();
                throw new SystemException(e.Message, e);
            }
            finally
            {
                HibernateUtil.CloseSession(session);
            } // end try
        }

        /// <summary>
        /// Save or Update Entity
        /// </summary>
        /// <typeparam name="T">Object to include</typeparam>
        /// <param name="monitor"></param>
        /// <param name="client"></param>
        public void SaveOrUpdate<T>(T entity) {
            ITransaction tx = null;
            ISession session = null;

            try {
                // Obtêm session
                session = HibernateUtil.GetCurrentSession();

                // Abre transação
                tx = session.BeginTransaction();

                // Persiste objeto
                session.SaveOrUpdate(entity);

                // Commit transaction
                tx.Commit();
            } catch (Exception e) {
                if (tx != null)
                    tx.Rollback();
                throw new SystemException(e.Message, e);
            }
            finally
            {
                HibernateUtil.CloseSession(session);
            } // end try
        }

        /// <summary>
        /// Inclui ou Altera entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySearch"></param>
        /// <param name="valuePropertySearch"></param>
        /// <param name="entity"></param>
        /// <returns>Retorna null caso seja Update. Caso seja Insert retorna a PK</returns>
        public object SaveOrUpdate<T>(string propertySearch, object valuePropertySearch, T entity) {
            // Find entity by property
            IDictionary<string, object> objectFilter = new Dictionary<string, object>() { { propertySearch, valuePropertySearch } };
            T baseEntity = (T) FindEntityByFilter<T>(objectFilter, null);

            // Valida se é Insert ou Update
            if (baseEntity == null) {
                return Incluir<T>(entity);
            } else {
                Alterar(baseEntity);
                return null;
            } // end else
        }


        /// <summary>
        /// Retorna uma uma instância da classe T que se encontra no banco de dados identificada pela chave <code>pk</code> 
        /// ou <code>null</code>, caso o mesmo não seja encontrado.
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public T FindByPK<T>(object pk) {
            return FindByPK<T>(pk, null);
        }

        /// <summary>
        /// Retorna uma uma instância da classe T que se encontra no banco de dados identificada pela chave <code>pk</code> 
        /// ou <code>null</code>, caso o mesmo não seja encontrado. com os atributos informados carregados
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pk"></param>
        /// <param name="atributosAInicializar"></param>
        /// <returns></returns>
        public T FindByPK<T>(object pk, IList<string> atributosAInicializar) {
            try {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();
                T result;

                // Efetua consulta e executa commit
                result = (T) session.Get(typeof(T), pk);


                // Inicializa os atributos
                HibernateUtil.InitializeProperties(atributosAInicializar, result);

                // Retorna object
                return result;
            } catch (HibernateException e) {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados.
        /// </summary>
        /// <param name="atributosAInicializar"></param>
        /// <returns></returns>
        public IList<T> FindAll<T>(IList<string> atributosAInicializar)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();
                IList<T> lista = null;

                // Efetua consulta
                ICriteria criteria = session.CreateCriteria(typeof(T)).SetCacheable(true);
                lista = (IList<T>)criteria.List<T>();

                // Inicializa os atributos
                HibernateUtil.InitializeProperties(atributosAInicializar, lista);

                // Retorna list
                return lista;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados.
        /// </summary>
        /// <returns></returns>
        public IList<T> FindAll<T>()
        {
            return FindAll<T>(null);
        }

        public IList<T> FindByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            return FindByFilter<T>(predicate, null, null);
        }

        public IList<T> FindByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int? pageSize, int? currentPage) where T : class
        {
            // Obtêm session
            ISession session = HibernateUtil.GetCurrentSession();
            
            // Retorna list
            if (pageSize != null && currentPage != null)
            {
                return session.QueryOver<T>().Where(predicate).Skip((currentPage.Value-1)*pageSize.Value).Take(pageSize.Value).List();
            }
            else
            {
                return session.QueryOver<T>().Where(predicate).List();
            }

        }

        public DataTableData FindDataTableByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int pageSize, int currentPage) where T : class
        {
            // Obtêm session
            ISession session = HibernateUtil.GetCurrentSession();

            int total = CountByFilter<T>(predicate);
            List<object> lista = FindByFilter<T>(predicate, pageSize, currentPage).ToList<object>();

            return new DataTableData(lista, total);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidass no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(object filter)
        {
            return FindByFilter<T>(filter, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidass no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(AbstractPagingFilter filter)
        {
            if (filter.TotalItems == null)
            {
                filter.TotalItems = CountByFilter<T>(filter);
            }

            return this.FindByFilter<T>(filter, filter.PageSize, filter.CurrentPage);
        }

        /// <summary>
        /// Retorna os registros encontrados na consulta.
        /// 
        /// se informados a pagina corrente e o tamanho da pagina a consulta eh realizada com paginacao
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(object filter, int? pageSize, int? currentPage)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();

                // Cria o criteria para consulta
                ICriteria criteria = _criteriaUtil.CreateSearchCriteria<T>(ClassAttributeUtil.MainAlias(filter.GetType()), session);

                // Preenche o criteria considerando a ordenação
                _criteriaUtil.FillSearchCriteria(filter, criteria, true);

                // Executa a consulta
                IList<T> list = _criteriaUtil.ExecuteSearch<T>(pageSize, currentPage, criteria);

                // Inicializa as propriedades
                HibernateUtil.InitializeProperties(FieldsFilterUtil.GetInitializeProperties(filter), list);

                // Retorna list
                return list;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        public int CountByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            // Obtêm session
            ISession session = HibernateUtil.GetCurrentSession();

            return session.QueryOver<T>().Where(predicate).RowCount();
        }

        /// <summary>
        /// Retorna o total de registros encontrados na consulta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int CountByFilter<T>(object filter)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();

                // Efetua consulta

                // Cria o criteria para consulta
                ICriteria criteria = _criteriaUtil.CreateSearchCriteria<T>(ClassAttributeUtil.MainAlias(filter.GetType()), session);

                // Preenche o criteria sem considerar a ordenação
                _criteriaUtil.FillSearchCriteria(filter, criteria, false);

                // Executa a consulta
                IFutureValue<Int32> fv = criteria.SetProjection(Projections.RowCount()).FutureValue<Int32>();

                return (fv != null) ? fv.Value : 0;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Retorna o total de registros encontrados na consulta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT">Alias para T</param>
        /// <param name="aliasJoin">Alias para inner joins</param>
        /// <param name="aliasJoinTyped">Alias para os joins definidos</param>
        /// <param name="fieldsFilter">Propriedades comparadas com equal aos valores <"propriedade", valor> </param>
        /// <param name="fieldsFilterWithRestriction">Propriedades comparadas com a restricao definida em relacao aos valores 
        /// <"propriedade", <Restricao, Valor>></param>
        /// <returns></returns>
        public int CountByFilter<T>(
            string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();

                // Efetua consulta

                // Cria o criteria para consulta
                ICriteria criteria = _criteriaUtil.CreateSearchCriteria<T>(aliasToT, session);

                // Preenche o criteria sem considerar a ordenação
                _criteriaUtil.FillSearchCriteria(aliasJoin, 
                    aliasJoinTyped, fieldsFilter, fieldsFilterWithRestriction, null, criteria);

                // Executa a consulta
                IFutureValue<Int32> fv = criteria.SetProjection(Projections.RowCount()).FutureValue<Int32>();

                return (fv != null) ? fv.Value : 0;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT">Alias para T</param>
        /// <param name="aliasJoin">Alias para inner joins</param>
        /// <param name="aliasJoinTyped">Alias para os joins definidos</param>
        /// <param name="fieldsFilter">Propriedades comparadas com equal aos valores <"propriedade", valor> </param>
        /// <param name="fieldsFilterWithRestriction">Propriedades comparadas com a restricao definida em relacao aos valores 
        /// <"propriedade", <Restricao, Valor>></param>
        /// <param name="fieldsOrder"></param>
        /// <param name="attrInitialized"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder,
            IList<string> attrInitialized,
            int? pageSize,
            int? currentPage)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();

                // Cria o criteria para consulta
                ICriteria criteria = _criteriaUtil.CreateSearchCriteria<T>(aliasToT, session);

                // Preenche o criteria
                _criteriaUtil.FillSearchCriteria(aliasJoin,
                                          aliasJoinTyped,
                                          fieldsFilter,
                                          fieldsFilterWithRestriction,
                                          fieldsOrder,
                                          criteria);
                // Executa a consulta
                IList<T> list = _criteriaUtil.ExecuteSearch<T>(pageSize, currentPage, criteria);

                // Inicializa as propriedades
                HibernateUtil.InitializeProperties(attrInitialized, list);

                // Retorna list
                return list;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilter"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="fieldsOrder"></param>
        /// <param name="attrInitialized"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder,
            IList<string> attrInitialized,
            int? pageSize,
            int? currentPage)
        {
            return this.FindByFilter<T>(aliasToT, aliasJoin, null, fieldsFilter, fieldsFilterWithRestriction, fieldsOrder, attrInitialized, pageSize, currentPage);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilter"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="fieldsOrder"></param>
        /// <param name="attrInitialized"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoin,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder,
            IList<string> attrInitialized,
            int? pageSize,
            int? currentPage)
        {
            return this.FindByFilter<T>(
                aliasToT, null, aliasJoin, fieldsFilter, fieldsFilterWithRestriction, fieldsOrder, attrInitialized, pageSize, currentPage);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsFilter"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(IDictionary<string, object> fieldsFilter, IList<string> attrInitialized)
        {
            return this.FindByFilter<T>(null, null, null,  fieldsFilter, null, null, attrInitialized, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsFilter"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction, IList<string> attrInitialized)
        {
            return this.FindByFilter<T>(null, null, null, null, fieldsFilterWithRestriction, null, attrInitialized, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>z
        /// <param name="attrInitialized"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, IDictionary<string, JoinType>> aliasJoin, IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction, IList<string> attrInitialized)
        {
            return this.FindByFilter<T>(aliasToT, null, aliasJoin, null, fieldsFilterWithRestriction, null, attrInitialized, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="attrInitialized"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction, IList<string> attrInitialized)
        {
            return this.FindByFilter<T>(aliasToT, aliasJoin, null, null, fieldsFilterWithRestriction, null, attrInitialized, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction)
        {
            return this.FindByFilter<T>(aliasToT, null, aliasJoin, null, fieldsFilterWithRestriction, null, null, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(
            string aliasToT, 
            IDictionary<string, string> aliasJoin, 
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction)
        {
            return this.FindByFilter<T>(aliasToT, aliasJoin, null, null, fieldsFilterWithRestriction, null, null, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            int? pageSize,
            int? currentPage)
        {
            return this.FindByFilter<T>(aliasToT, null, aliasJoin, null, fieldsFilterWithRestriction, null, null, pageSize, currentPage);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(
            string aliasToT, 
            IDictionary<string, string> aliasJoin, 
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction, 
            int? pageSize, 
            int? currentPage)
        {
            return this.FindByFilter<T>(aliasToT, aliasJoin, null, null, fieldsFilterWithRestriction, null, null, pageSize, currentPage);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>z
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsOrder"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, IDictionary<string, JoinType>> aliasJoin, IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder, IList<string> attrInitialized)
        {
            return this.FindByFilter<T>(aliasToT, null, aliasJoin, null, fieldsFilterWithRestriction, fieldsOrder, attrInitialized, null, null);
        }

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>z
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsOrder"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction, IDictionary<string, Enums.Order> fieldsOrder, IList<string> attrInitialized)
        {
            return this.FindByFilter<T>(aliasToT, aliasJoin, null, null, fieldsFilterWithRestriction, fieldsOrder, attrInitialized, null, null);
        }

        /// <summary>
        /// Return entity by filter
        /// </summary>
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsFilter"></param>
        /// <returns></returns>
        public T FindEntityByFilter<T>(IDictionary<string, object> fieldsFilter, IList<string> attrInitialized)
        {
            try
            {
                // Get session
                ISession session = HibernateUtil.GetCurrentSession();
                T entity;

                // Cria o criteria para consulta
                ICriteria criteria = _criteriaUtil.CreateSearchCriteria<T>(session);

                // Preenche o criteria
                _criteriaUtil.AddRestrictionsEq(fieldsFilter, criteria);

                // Executa a consulta
                entity = (T)criteria.UniqueResult<T>();

                HibernateUtil.InitializeProperties(attrInitialized, entity);

                // Return entity
                return entity;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Valida se registro se encontra na base pela <code>propertyName</code> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ExistsEntity<T>(string propertyName, string value)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();

                // Efetua consulta
                object count = session.CreateCriteria(typeof(T))
                    .SetProjection(Projections.Count(Projections.Id()))
                    .Add(Restrictions.Eq(propertyName, value))
                    .UniqueResult();

                // Retorna result
                return (count != null && long.Parse(count.ToString()) > 0);
            }
            catch (HibernateException e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Valida se registro se encontra na base pela <code>propertyName</code> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertiesName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ExistsEntity<T>(IDictionary<string, object> propertiesName)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();

                // Cria o criteria para consulta
                ICriteria criteria = _criteriaUtil.CreateSearchCriteria<T>(session);

                // Preenche o criteria
                _criteriaUtil.AddRestrictionsEq(propertiesName, criteria);

                //define projecao para count
                criteria.SetProjection(Projections.Count(Projections.Id()));

                // Execute query
                object count = criteria.UniqueResult();

                // Retorna result
                return (count != null && long.Parse(count.ToString()) > 0);
            }
            catch (HibernateException e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

        /// <summary>
        /// Validate entity exists on update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValue"></param>
        /// <param name="propertyCompare"></param>
        /// <param name="propertiesSelectCompare"></param>
        /// <param name="propertiesSelectIn"></param>
        /// <returns></returns>
        public bool ExistsEntityOnUpdate<T>(object pkValue, string propertyCompare, IDictionary<string, object> propertiesSelectCompare, IDictionary<string, object> propertiesSelectIn)
        {
            try
            {
                // Obtêm session
                ISession session = HibernateUtil.GetCurrentSession();

                // Definy select in to compare
                var selectCompare = DetachedCriteria.For<T>()
                    .SetProjection(Projections.Id())
                    .Add(Restrictions.Not(Restrictions.Eq(propertyCompare, pkValue)));

                // Add property, case propertiesName is not null
                if (propertiesSelectCompare != null)
                {
                    ICollection<string> keys = propertiesSelectCompare.Keys;
                    foreach (string key in keys)
                    {
                        selectCompare.Add(Restrictions.Eq(key, propertiesSelectCompare[key]));
                    } // end for
                } // end if

                // Definy select in to compare
                var selectIn = DetachedCriteria.For<T>()
                .SetProjection(Projections.Count(Projections.Id()))
                .Add(Subqueries.PropertyIn(propertyCompare, selectCompare));

                // Add property, case propertiesName is not null
                if (propertiesSelectIn != null)
                {
                    ICollection<string> keys = propertiesSelectIn.Keys;
                    foreach (string key in keys)
                    {
                        selectIn.Add(Restrictions.Eq(key, propertiesSelectIn[key]));
                    } // end for
                } // end if

                // Execute query
                object count = selectIn.GetExecutableCriteria(session).UniqueResult();

                // Retorna result
                return (count != null && long.Parse(count.ToString()) > 0);
            }
            catch (HibernateException e)
            {
                throw new SystemException(e.Message, e);
            } // end try
        }

    }
}
