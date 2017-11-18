using NetUtil.Util.DTO;
using SGCA.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGCA.Tests.Mock
{
    public class GenericMockDAO : IGenericDAO
    {
        public virtual object Incluir<T>(T entity)
        {
            return entity;
        }

        public virtual void Incluir<T, K>(T entity, K pk)
        {
        }

        public virtual void Alterar(object entity)
        {
        }

        public virtual void Excluir<T>(T entity)
        {
        }

        public virtual void ExecuteQuery(object table, object parameter, string query)
        {
        }

        public virtual void SaveOrUpdate<T>(T entity)
        {
        }

        public virtual object SaveOrUpdate<T>(string propertySearch, object valuePropertySearch, T entity)
        {
            return entity;
        }

        public virtual T FindByPK<T>(object pk)
        {
            return Activator.CreateInstance<T>();
        }

        public virtual T FindByPK<T>(object pk, IList<string> atributosAInicializar)
        {
            return Activator.CreateInstance<T>();
        }

        public virtual IList<T> FindAll<T>(IList<string> atributosAInicializar)
        {
            return new List<T>();
        }

        public virtual IList<T> FindAll<T>()
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int? pageSize, int? currentPage) where T : class
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(object filter)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(NetUtil.Util.Filter.AbstractPagingFilter filter)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(object filter, int? pageSize, int? currentPage)
        {
            return new List<T>();
        }

        public virtual int CountByFilter<T>(object filter)
        {
            return 0;
        }

        public virtual int CountByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<string, NetUtil.Util.Enums.JoinType>> aliasJoinTyped, IDictionary<string, object> fieldsFilter, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction)
        {
            return 0;
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<string, NetUtil.Util.Enums.JoinType>> aliasJoinTyped, IDictionary<string, object> fieldsFilter, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder, IList<string> attrInitialized, int? pageSize, int? currentPage)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, IDictionary<string, NetUtil.Util.Enums.JoinType>> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, IDictionary<string, NetUtil.Util.Enums.JoinType>> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, int? pageSize, int? currentPage)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, int? pageSize, int? currentPage)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(IDictionary<string, object> fieldsFilter, IList<string> attrInitialized)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, IList<string> attrInitialized)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, IDictionary<string, NetUtil.Util.Enums.JoinType>> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, IList<string> attrInitialized)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, IList<string> attrInitialized)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, IDictionary<string, NetUtil.Util.Enums.JoinType>> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder, IList<string> attrInitialized)
        {
            return new List<T>();
        }

        public virtual IList<T> FindByFilter<T>(string aliasToT, IDictionary<string, string> aliasJoin, IDictionary<string, IDictionary<NetUtil.Util.Enums.Restriction, object>> fieldsFilterWithRestriction, IDictionary<string, NetUtil.Util.Enums.Order> fieldsOrder, IList<string> attrInitialized)
        {
            return new List<T>();
        }

        public virtual T FindEntityByFilter<T>(IDictionary<string, object> fieldsFilter, IList<string> attrInitialized)
        {
            return Activator.CreateInstance<T>();
        }

        public virtual bool ExistsEntity<T>(string propertyName, string value)
        {
            return false;
        }

        public virtual bool ExistsEntity<T>(IDictionary<string, object> propertiesName)
        {
            return false;
        }

        public virtual bool ExistsEntityOnUpdate<T>(object pkValue, string propertyCompare, IDictionary<string, object> propertiesSelectCompare, IDictionary<string, object> propertiesSelectIn)
        {
            return false;
        }


        public int CountByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            return 10;
        }

        public DataTableData FindDataTableByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int start, int length) where T : class
        {
            return new DataTableData();
        }
    }
}
