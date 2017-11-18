using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate;
using System.Web;
using NHibernate.Tool.hbm2ddl;

namespace NetUtil.Util.Hibernate {
    public sealed class HibernateUtil {
        private const string CurrentSessionKey = "nhibernate.current_session";
        private static readonly ISessionFactory sessionFactory;

        // Atributes class
        private static Configuration cfg;

        /// <summary>
        /// Bloco static para inicializar SessionFactory
        /// </summary>
        static HibernateUtil() {
            cfg = new Configuration().Configure();
            sessionFactory = cfg.BuildSessionFactory();
        }

        /// <summary>
        /// Retorna session
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentSession()
        {
            ISession currentSession = null;

            // se utiliza contexto web pega a sessao do context web
            try
            {
                HttpContext context = HttpContext.Current;
                currentSession = context.Items[CurrentSessionKey] as ISession;

                // Caso currentSession seja null
                if (currentSession == null)
                {
                    currentSession = sessionFactory.OpenSession(new NHSQLInterceptor()); //sessionFactory.OpenSession();
                    context.Items[CurrentSessionKey] = currentSession;
                } // end if
            }
            // se nao tem contexto web cai no catch
            catch
            {
                currentSession = sessionFactory.OpenSession(new NHSQLInterceptor()); //sessionFactory.OpenSession();
            } // end catch

            // Retorna session
            return currentSession;
        }

        /// <summary>
        /// Retorna session
        /// </summary>
        /// <returns></returns>
        public static ISession OpenSession() {
            // Retorna session
            return sessionFactory.OpenSession(new NHSQLInterceptor());
        }

        /// <summary>
        /// Fecha a sessao e remove do pool caso esteja utilizando httpContext
        /// </summary>
        /// <param name="session"></param>
        public static void CloseSession(ISession session) {
            //fecha a session se diferente de null
            if (session != null)
            {
                session.Close();

                //caso esteja utilizando HttpContext tenta remover do pool
                try
                {
                    HttpContext context = HttpContext.Current;
                    if (context.Items.Contains(CurrentSessionKey))
                    {
                        context.Items.Remove(CurrentSessionKey);
                    }
                }
                //se cair no catch nao tem context 
                catch { }
            } // end if
        }

        /// <summary>
        /// Fecha session utilizando o contexto para encontrar a session aberta
        /// </summary>
        public static void CloseSession() {
            ISession currentSession = null;
            //tenta obter a sessao corrente do contexto
            try
            {
                HttpContext context = HttpContext.Current;
                currentSession = context.Items[CurrentSessionKey] as ISession;
                //remove do pool
                context.Items.Remove(CurrentSessionKey);
            }
            // se cair aqui noa tem contexto
            catch { }

            // Caso session seja diferente de null
            if (currentSession != null) {
                // fecha a sessao
                currentSession.Close();
            } // end if
        }
		
        /// <summary>
        /// Fecha session factory
        /// </summary>
        public static void CloseSessionFactory() {
            if (sessionFactory != null) {
                sessionFactory.Close();
            } // end if
        }

        /// <summary>
        /// Generate Schema
        /// </summary>
        public static void GeraSchema() {
            new SchemaUpdate(cfg).Execute(true, true);// SchemaExport(cfg).Create(true, true);
        }

        /// <summary>
        /// Inicializa as propriedades, de cada entidade da lista de objetos,
        /// que nao foram carregadas pois a consulta foi do tipo lazy
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="list"></param>
        public static void InitializeProperties(IList<string> properties, IList<object> list)
        {
            // Inicializa atributos
            if (list != null && properties != null)
            {
                foreach (Object o in list)
                {
                    InitializeProperties(properties, o);
                } // end for
            } // end if
        }


        /// <summary>
        /// Inicializa as propriedades, do objeto fornecido,
        /// que nao foram carregadas pois a consulta foi do tipo lazy
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="list"></param>
        public static void InitializeProperties(IList<string> properties, object entity)
        {
            // Initialize attributes
            if (entity != null && properties != null)
            {
                foreach (string atributo in properties)
                {
                    NHibernateUtil.Initialize(entity.GetType().GetProperty(atributo.ToString()).GetValue(entity, null));
                } // end for
            } // end if
        }
    }
}
