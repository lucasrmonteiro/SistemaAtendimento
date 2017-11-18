using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using NetUtil.Util.Enums;
using NetUtil.Util.Filter;
using NetUtil.Util.DTO;

namespace NetUtil.Util.Hibernate.Interfaces {
    /// 
    /// Interface para o design pattern Data Access Object. 
    /// 
    /// author Roberto Campello
    /// 
    /// typeparam name="T" Classe a ser gerenciadao pelo DAO.
    /// typeparam name="K" Tipo da chave identificadora da classe gerenciada.
    ///
    public interface IDAO {
        /// <summary>
        /// Inclui objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        object Incluir<T>(T entity);

        /// <summary>
        /// Incluir objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="entity"></param>
        /// <param name="pk"></param>
        void Incluir<T, K>(T entity, K pk);

        /// <summary>
        /// Altera objeto
        /// </summary>
        /// <param name="entity"></param>
        void Alterar(object entity);

        /// <summary>
        /// Exclui objeto   
        /// </summary>
        /// <typeparam name="T">Objeto a ser excluído</typeparam>
        /// <param name="entity"></param>
        void Excluir<T>(T entity);

        /// <summary>
        /// Executa uma query com um parametro (um filtro "where")
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="table"></param>
        /// <param name="query"></param>
        void ExecuteQuery(object table, object parameter, string query);

        /// <summary>
        /// Save or Update Entity
        /// </summary>
        /// <typeparam name="T">Object to include</typeparam>
        /// <param name="monitor"></param>
        /// <param name="client"></param>
        void SaveOrUpdate<T>(T entity);

        /// <summary>
        /// Inclui ou Altera entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySearch"></param>
        /// <param name="valuePropertySearch"></param>
        /// <param name="entity"></param>
        /// <returns>Retorna null caso seja Update. Caso seja Insert retorna a PK</returns>
        object SaveOrUpdate<T>(string propertySearch, object valuePropertySearch, T entity);

        /// <summary>
        /// Retorna uma uma instância da classe T que se encontra no banco de dados identificada pela chave <code>pk</code> 
        /// ou <code>null</code>, caso o mesmo não seja encontrado.
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        T FindByPK<T>(object pk);

        /// <summary>
        /// Retorna uma uma instância da classe T que se encontra no banco de dados identificada pela chave <code>pk</code> 
        /// ou <code>null</code>, caso o mesmo não seja encontrado. com os atributos informados carregados
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pk"></param>
        /// <param name="atributosAInicializar"></param>
        /// <returns></returns>
        T FindByPK<T>(object pk, IList<string> atributosAInicializar);

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados.
        /// </summary>
        /// <param name="atributosAInicializar"></param>
        /// <returns></returns>
        IList<T> FindAll<T>(IList<string> atributosAInicializar);

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados.
        /// </summary>
        /// <returns></returns>
        IList<T> FindAll<T>();

        IList<T> FindByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class;

        IList<T> FindByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int? pageSize, int? currentPage) where T : class;

        DataTableData FindDataTableByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int start, int length) where T : class;
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(object filter);

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidass no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(AbstractPagingFilter filter);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(object filter, int? pageSize, int? currentPage);

        /// <summary>
        /// Retorna o total de registros encontrados na consulta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        int CountByFilter<T>(object filter);

        /// <summary>
        /// Retorna o total de registros encontrados na consulta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilter"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="fieldsFilterNotIn"></param>
        /// <returns></returns>
        int CountByFilter<T>(
            string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction);


        int CountByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class;

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
        IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder,
            IList<string> attrInitialized,
            int? pageSize,
            int? currentPage);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            int? pageSize,
            int? currentPage);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(
            string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            int? pageSize,
            int? currentPage);

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsFilter"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(IDictionary<string, object> fieldsFilter, IList<string> attrInitialized);

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsFilter"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IList<string> attrInitialized);

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>z
        /// <param name="attrInitialized"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(string aliasToT,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>>
            fieldsFilterWithRestriction, IList<string> attrInitialized);

        /// <summary>
        /// Retorna uma lista com todas as instâncias da class <code>T</code> que se encontrem persistidas no banco de dados através do filtro aplicado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="fieldsFilterWithRestriction"></param>z
        /// <param name="attrInitialized"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>>
            fieldsFilterWithRestriction, IList<string> attrInitialized);

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
        IList<T> FindByFilter<T>(string aliasToT,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder, IList<string> attrInitialized);

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
        IList<T> FindByFilter<T>(string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder, IList<string> attrInitialized);

        /// <summary>
        /// Return entity by filter
        /// </summary>
        /// <param name="attrInitialized"></param>
        /// <param name="fieldsFilter"></param>
        /// <returns></returns>
        T FindEntityByFilter<T>(IDictionary<string, object> fieldsFilter, IList<string> attrInitialized);

        /// <summary>
        /// Valida se registro se encontra na base pela <code>propertyName</code> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ExistsEntity<T>(string propertyName, string value);

        /// <summary>
        /// Valida se registro se encontra na base pela <code>propertyName</code> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertiesName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ExistsEntity<T>(IDictionary<string, object> propertiesName);

        /// <summary>
        /// Validate entity exists on update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValue"></param>
        /// <param name="propertyCompare"></param>
        /// <param name="propertiesSelectCompare"></param>
        /// <param name="propertiesSelectIn"></param>
        /// <returns></returns>
        bool ExistsEntityOnUpdate<T>(object pkValue, string propertyCompare,
            IDictionary<string, object> propertiesSelectCompare,
            IDictionary<string, object> propertiesSelectIn);
    }
}
