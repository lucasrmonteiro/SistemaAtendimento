using NetUtil.Util.Enums;
using NetUtil.Util.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetUtil.Util.Entity.Interfaces
{
    public interface IEntityDAO
    {
        /// <summary>
        /// Insere novo objeto do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Insert<T>(T entity) where T : class;

        /// <summary>
        /// Insere a lista de novos objetos do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity"></param>
        void Insert<T>(IList<T> listEntity) where T : class;

        /// <summary>
        /// Atualiza o objeto do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Atualiza uma lista de objetos do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity"></param>
        void Update<T>(IList<T> listEntity) where T : class;

        /// <summary>
        /// Deleta o objeto do tipo T do banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Deleta a lista de objetos do tipo T do banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity"></param>
        void Delete<T>(IList<T> listEntity) where T : class;

        /// <summary>
        /// Deleta os objetos do tipo T que correspondam ao filtro da função lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        void Delete<T>(Func<T, bool> predicate) where T : class;

        /// <summary>
        /// Salva as alterações realizadas no banco [insert;update;delete]
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Retorna todos objetos do tipo T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IList<T> FindAll<T>() where T : class;

        /// <summary>
        /// Retorno um objeto cuja chave corresponda com a fornecida
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T FindEntityByPK<T>(params object[] key) where T : class;

        /// <summary>
        /// Retorna uma lista de objetos do tipo T que correspondam ao filtro da função lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(Func<T, bool> predicate) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(Func<T, bool> predicate, int? pageSize, int? currentPage) where T : class;

        /// <summary>
        /// Retorna uma lista de objetos do tipo T de acordo com as anotações do objeto filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(object filter) where T : class;

        /// <summary>
        /// Retorna uma lista paginada de objetos do tipo T de acordo com as anotações do objeto filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(AbstractPagingFilter filter) where T : class;

        /// <summary>
        /// Retorna uma lista paginada de objetos do tipo T de acordo com as anotações do objeto filtro e os parametros para paginação
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(object filter, int? pageSize, int? currentPage) where T : class;

        /// <summary>
        /// Retorna uma lista paginada de objetos do tipo T de acordo com os dicionarios de filtros e parametros para paginação
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="aliasJoinTyped"></param>
        /// <param name="fieldsFilter"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <param name="fieldsOrder"></param>
        /// <param name="attrInitialized"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        IList<T> FindByFilter<T>(string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string,
            Enums.JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Enums.Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, Enums.Order> fieldsOrder,
            IList<string> attrInitialized, int? pageSize, int? currentPage) where T : class;

        IList<T> FindByFilter<T>(IDictionary<string, object> fieldsFilter) where T : class;

        /// <summary>
        /// Retorna o total de objetos do tipo T que seriam retornados em uma consulta com o filtro da função lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int CountByFilter<T>(Func<T, bool> predicate) where T : class;

        /// <summary>
        /// Retorna o total de objetos do tipo T que seriam retornados em uma consulta de acordo com as anotações no objeto filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        int CountByFilter<T>(object filter) where T : class;

        /// <summary>
        /// Retorna o total de objetos do tipo T que seriam retornados em uma consulta de acordo com os dicionarios de filtros
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT"></param>
        /// <param name="aliasJoin"></param>
        /// <param name="aliasJoinTyped"></param>
        /// <param name="fieldsFilter"></param>
        /// <param name="fieldsFilterWithRestriction"></param>
        /// <returns></returns>
        int CountByFilter<T>(
            string aliasToT, IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string, JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction)
             where T : class;
    }
}
