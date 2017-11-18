using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using NetUtil.Util.Entity.Interfaces;
using NetUtil.Util.Helper;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using NetUtil.Util.Filter;
using System.Data.Common;

namespace NetUtil.Util.Entity
{
    public class EntityDAO : IDisposable, IEntityDAO
    {
        //utilidades com SqlQuery
        private SqlQueryUtil _sqlQueryUtil = SqlQueryUtil.GetInstance();
        //variavel virtual, deve ser sobrescrita pela classe generica do projeto retornando um 
        //tipo do objeto que extenda de ConnDB
        public virtual Type contextType { get { return typeof(ConnDB); } }
        //variavel virtual, pode ser sobrescrita caso utilize outro tipo de parameter que xxtenda de DbParameter
        public virtual Type parameterType { get { return typeof(SqlParameter); } }

        /// <summary>
        /// Insere novo objeto do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Insert<T>(T entity) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            dbc.Set<T>().Add(entity);

            SaveChanges();
        }

        /// <summary>
        /// Insere a lista de novos objetos do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity"></param>
        public void Insert<T>(IList<T> listEntity) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            listEntity.ToList().ForEach(obj => dbc.Set<T>().Add(obj));

            SaveChanges();
        }

        /// <summary>
        /// Atualiza o objeto do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            dbc.Entry(entity).State = EntityState.Modified;

            SaveChanges();
        }

        /// <summary>
        /// Atualiza uma lista de objetos do tipo T no banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity"></param>
        public void Update<T>(IList<T> listEntity) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            listEntity.ToList().ForEach(obj => dbc.Entry(obj).State = EntityState.Modified);

            SaveChanges();
        }

        /// <summary>
        /// Deleta o objeto do tipo T do banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            dbc.Set<T>().Remove(entity);

            SaveChanges();
        }

        /// <summary>
        /// Deleta a lista de objetos do tipo T do banco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity"></param>
        public void Delete<T>(IList<T> listEntity) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            listEntity.ToList().ForEach(obj => dbc.Set<T>().Remove(obj));

            SaveChanges();
        }

        /// <summary>
        /// Deleta os objetos do tipo T que correspondam ao filtro da função lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public void Delete<T>(Func<T, bool> predicate) where T : class
        {
            Delete<T>(FindByFilter<T>(predicate));
        }

        /// <summary>
        /// Salva as alterações realizadas no banco [insert;update;delete]
        /// </summary>
        public void SaveChanges()
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            dbc.SaveChanges();
        }

        /// <summary>
        /// Retorna todos objetos do tipo T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> FindAll<T>() where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            return dbc.Set<T>().ToList();
        }

        /// <summary>
        /// Retorno um objeto cuja chave corresponda com a fornecida
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T FindEntityByPK<T>(params object[] key) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            return dbc.Set<T>().Find(key);
        }

        /// <summary>
        /// Retorna uma lista de objetos do tipo T que correspondam ao filtro da função lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(Func<T, bool> predicate) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            return dbc.Set<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(Func<T, bool> predicate, int? pageSize, int? currentPage) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);
            
            int skipRows = (currentPage.Value - 1) * pageSize.Value;
            
            return dbc.Set<T>().Where(predicate).Skip(skipRows).Take(pageSize.Value).ToList();
        }

        /// <summary>
        /// Retorna uma lista de objetos do tipo T de acordo com as anotações do objeto filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(object filter) where T : class
        {
            return FindByFilter<T>(filter, null, null);
        }

        /// <summary>
        /// Retorna uma lista paginada de objetos do tipo T de acordo com as anotações do objeto filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(AbstractPagingFilter filter) where T : class
        {
            if (filter.TotalItems == null)
            {
                filter.TotalItems = CountByFilter<T>(filter);
            }

            return this.FindByFilter<T>(filter, filter.PageSize, filter.CurrentPage);
        }

        /// <summary>
        /// Retorna uma lista paginada de objetos do tipo T de acordo com as anotações do objeto filtro e os parametros para paginação
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(object filter, int? pageSize, int? currentPage) where T : class
        {
            //pega o database context
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            //Inicia o stringBuilder com o sql que sera utilizado na consulta
            StringBuilder criteria = _sqlQueryUtil.CreateSearchCriteria<T>(ClassAttributeUtil.MainAlias(filter.GetType()));

            //preenche as restrições/joins/ordenacao
            _sqlQueryUtil.FillSearchCriteria(filter, criteria, true);

            //obtem a lista de parametros da query
            List<DbParameter> parameters = new List<DbParameter>();
            _sqlQueryUtil.GetQueryParameters(filter, parameters, parameterType);

            //pega o set que será consultado
            DbSet<T> dbSet = (DbSet<T>)dbc.Set<T>();

            // informa as propriedades que serão inicializadas
            EntityUtil.InitializeProperties(FieldsFilterUtil.GetInitializeProperties(filter), dbSet);

            // prepara o sqlQuery com os parametros
            DbSqlQuery<T> sqlQuery = dbSet.SqlQuery(criteria.ToString(), parameters.ToArray());

            // executa a consutla
            IList<T> list = _sqlQueryUtil.ExecuteQuery<T>(pageSize, currentPage, sqlQuery);

            // retorna list
            return list;
        }

        /// <summary>
        /// Retorna uma lista paginada de objetos do tipo T de acordo com os dicionarios de filtros e parametros para paginação
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT">Alias para T</param>
        /// <param name="aliasJoin">Alias para inner joins</param>
        /// <param name="aliasJoinTyped">Alias para os joins definidos</param>
        /// <param name="fieldsFilter">Propriedades comparadas com equal aos valores <"propriedade", valor> </param>
        /// <param name="fieldsFilterWithRestriction">Propriedades comparadas com a restricao definida em relacao aos valores 
        /// <"propriedade", <Restricao, Valor>></param>
        /// <param name="fieldsOrder">Ordenacao definida <"propriedade", ordenacao></param>
        /// <param name="attrInitialized">Atributos para inicializar</param>
        /// <param name="pageSize">Tamanho da pagina</param>
        /// <param name="currentPage">Pagina corrente</param>
        /// <returns></returns>
        public IList<T> FindByFilter<T>(string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string,
            Enums.JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Enums.Restriction, object>> fieldsFilterWithRestriction,
            IDictionary<string, Enums.Order> fieldsOrder,
            IList<string> attrInitialized, int? pageSize, int? currentPage) where T : class
        {
            //pega o database context
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            //Inicia o stringBuilder com o sql que sera utilizado na consulta
            StringBuilder criteria = _sqlQueryUtil.CreateSearchCriteria<T>(aliasToT);

            //preenche as restrições/joins/ordenacao
            _sqlQueryUtil.FillSearchCriteria(
                aliasJoin, aliasJoinTyped, fieldsFilter, fieldsFilterWithRestriction, fieldsOrder, criteria);

            //obtem a lista de parametros da query
            List<DbParameter> parameters = new List<DbParameter>();
            _sqlQueryUtil.GetQueryParameters(fieldsFilter, fieldsFilterWithRestriction, parameters, parameterType);

            //pega o set que será consultado
            DbSet<T> dbSet = (DbSet<T>)dbc.Set<T>();

            // informa as propriedades que serão inicializadas
            EntityUtil.InitializeProperties(attrInitialized, dbSet);

            // prepara o sqlQuery com os parametros
            DbSqlQuery<T> sqlQuery = dbSet.SqlQuery(criteria.ToString(), parameters.ToArray());

            // executa a consutla
            IList<T> list = _sqlQueryUtil.ExecuteQuery<T>(pageSize, currentPage, sqlQuery);

            // retorna list
            return list;
        }
        
        public IList<T> FindByFilter<T>(IDictionary<string, object> fieldsFilter) where T : class 
        {
            return this.FindByFilter<T>(null, null, null, fieldsFilter, null, null, null, null, null);
        }

        /// <summary>
        /// Retorna o total de objetos do tipo T que seriam retornados em uma consulta com o filtro da função lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int CountByFilter<T>(Func<T, bool> predicate) where T : class
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            return dbc.Set<T>().Where(predicate).Count();
        }

        /// <summary>
        /// Retorna o total de objetos do tipo T que seriam retornados em uma consulta de acordo com as anotações no objeto filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int CountByFilter<T>(object filter) where T : class
        {
            //pega o database context
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            //Inicia o stringBuilder com o sql que sera utilizado na consulta
            StringBuilder criteria = _sqlQueryUtil.CreateSearchCriteria<T>(ClassAttributeUtil.MainAlias(filter.GetType()));

            //preenche as restrições/joins/ordenacao
            _sqlQueryUtil.FillSearchCriteria(filter, criteria, true);

            //obtem a lista de parametros da query
            List<DbParameter> parameters = new List<DbParameter>();
            _sqlQueryUtil.GetQueryParameters(filter, parameters, parameterType);

            // prepara o sqlQuery com os parametros
            DbSqlQuery<T> sqlQuery = ((DbSet<T>)dbc.Set<T>()).SqlQuery(criteria.ToString(), parameters.ToArray());

            // retorna o count
            return sqlQuery.Count();
        }

        /// <summary>
        /// Retorna o total de objetos do tipo T que seriam retornados em uma consulta de acordo com os dicionarios de filtros
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasToT">Alias para T</param>
        /// <param name="aliasJoin">Alias para inner joins</param>
        /// <param name="aliasJoinTyped">Alias para os joins definidos</param>
        /// <param name="fieldsFilter">Propriedades comparadas com equal aos valores <"propriedade", valor> </param>
        /// <param name="fieldsFilterWithRestriction">Propriedades comparadas com a restricao definida em relacao aos valores 
        /// <"propriedade", <Restricao, Valor>></param>
        /// <returns></returns>
        public int CountByFilter<T>(string aliasToT,
            IDictionary<string, string> aliasJoin,
            IDictionary<string, IDictionary<string, Enums.JoinType>> aliasJoinTyped,
            IDictionary<string, object> fieldsFilter,
            IDictionary<string, IDictionary<Enums.Restriction, object>> fieldsFilterWithRestriction) where T : class
        {
            //pega o database context
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            //Inicia o stringBuilder com o sql que sera utilizado na consulta
            StringBuilder criteria = _sqlQueryUtil.CreateSearchCriteria<T>(aliasToT);

            //preenche as restrições/joins/ordenacao
            _sqlQueryUtil.FillSearchCriteria(
                aliasJoin, aliasJoinTyped, fieldsFilter, fieldsFilterWithRestriction, null, criteria);

            //obtem a lista de parametros da query
            List<DbParameter> parameters = new List<DbParameter>();
            _sqlQueryUtil.GetQueryParameters(fieldsFilter, fieldsFilterWithRestriction, parameters, parameterType);

            // prepara o sqlQuery com os parametros
            DbSqlQuery<T> sqlQuery = ((DbSet<T>)dbc.Set<T>()).SqlQuery(criteria.ToString(), parameters.ToArray());

            // retorna o count
            return sqlQuery.Count();
        }

        /// <summary>
        /// Implementacao da interface IDisposable
        /// </summary>
        public void Dispose()
        {
            ConnDB dbc = (ConnDB)EntityUtil.GetCurrentContext(contextType);

            dbc.Dispose();
        }
    }
}
