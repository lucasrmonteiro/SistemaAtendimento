using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using System.Collections;

namespace NetUtil.Util.Spring {
    public class ServiceLocator {
        /// <summary>
        /// Construtor private para não permitir criar instância
        /// 
        /// </summary>
        private ServiceLocator() {

        }

        /// <summary>
        /// Retorna context
        /// 
        /// </summary>
        /// <returns></returns>
        private static IApplicationContext CreateContainerUsingXML() {
            return new XmlApplicationContext("~/application-context.xml");
        }

        /// <summary>
        /// Retorna object configurado no arquivo do Spring
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>(bool singleton) {
            // Recupera context
            IApplicationContext ctx = ServiceLocator.CreateContainerUsingXML();
            IDictionary obectsOfType = (IDictionary)ctx.GetObjectsOfType(typeof(T)); 
            
            // Verifica se foi informado somente uma instância
            if (singleton && obectsOfType.Count != 1) {
                throw new ApplicationException(string.Format("Esperado somente uma instância de {0} mas foram encontradas {1)",
                    typeof(T).FullName, obectsOfType.Count));
            } // end if
            
            // Seta default
            T retVal = default(T); 
            
            // Recupera objeto
            foreach (object key in obectsOfType.Keys) {
                retVal = (T) obectsOfType[key];
            } // end for
            
            // Retorna object
            return retVal;
        }

        /// <summary>
        /// Retorna object configurado no arquivo do Spring
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>() {
            // Retorna object
            return GetObject<T>(false);
        }
    }
}
