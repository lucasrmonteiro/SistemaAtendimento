using NetUtil.Util.Filter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;

namespace NetUtil.Util.Entity
{
    public sealed class EntityUtil
    {
        private const string DATA_BASE_CONTEXT_KEY = "DataBaseContext";

        public static object GetCurrentContext(Type contextType)
        {
            object dbCurrentContext = null;

            // se utiliza contexto web pega a sessao do context web
            try
            {
                HttpContext httpContext = HttpContext.Current;
                dbCurrentContext = httpContext.Items[DATA_BASE_CONTEXT_KEY];

                //caso contexto do banco seja null
                if (dbCurrentContext == null)
                {
                    dbCurrentContext = Activator.CreateInstance(contextType);
                    httpContext.Items.Add(DATA_BASE_CONTEXT_KEY, dbCurrentContext);
                }//end if
            }
            // se nao tem contexto web cai no catch
            catch (Exception)
            {
                dbCurrentContext = Activator.CreateInstance(contextType);
            } // end catch

            return dbCurrentContext;
        }

        public static void InitializeProperties<T>(IList<string> attrInitialized, IQueryable<T> dbSet) where T : class
        {
            if (attrInitialized != null && dbSet != null)
            {
                foreach (string property in attrInitialized)
                {
                    dbSet = dbSet.Include(property).AsQueryable();
                }
            }
        }
    }
}
