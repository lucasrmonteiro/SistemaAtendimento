using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Helpers
{
    public class ControllerHelper
    {

        /// <summary>
        /// Método responsavel por verificar se pelo menos uma propriedade do objeto passado esta preenchida
        /// </summary>
        /// <param name="objeto">filtro cujas propriedades serao verificadas</param>
        /// <returns>true : caso alguma propriedade nao esteja nula ou vazia</returns>
        public static bool IsPreenchido<T>(T objeto)
        {
            foreach (var item in typeof(T).GetProperties())
            {
                object valor = item.GetValue(objeto);

                if ((valor != null) && !String.IsNullOrWhiteSpace(valor.ToString()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Método responsavel por verificar se todas as propriedade do objeto passado estao preenchidas
        /// </summary>
        /// <param name="objeto">objeto cujas propriedades serao verificadas</param>
        /// <param name="camposNaoPreenchidos">'.ToString()' das propriedades que nao estiverem preenchidas separados por 'Environment.NewLine' </param>
        /// <returns>false : caso alguma propriedade esteja nula ou vazia</returns>
        public static bool IsTodoPreenchido<T>(T objeto, ref String camposNaoPreenchidos)
        {
            foreach (var item in typeof(T).GetProperties())
            {
                object valor = item.GetValue(objeto);

                if ((valor == null) || String.IsNullOrWhiteSpace(valor.ToString()))
                {
                    camposNaoPreenchidos = String.Concat(camposNaoPreenchidos, item.ToString(), Environment.NewLine);
                }
            }

            return String.IsNullOrWhiteSpace(camposNaoPreenchidos);
        }

    }
}