using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SGCA.Models.Helpers
{
    /// <summary>
    /// Classe Helper para formatação e conversão de valores monetários.
    /// </summary>
    public static class NumberFormatHelper
    {
        /// <summary>
        /// Converte um valor string para decimal
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        public static Decimal ConvertStringToDecimal(String numero)
        {
            if (!String.IsNullOrEmpty(numero))
            {
                if (CultureInfo.CurrentCulture.ToString().Equals("en-US"))
                {
                    return Convert.ToDecimal(FormatStringToDecimal(numero));
                }
                else
                {
                    return Convert.ToDecimal(numero);
                }
            }
            else
            {
                return Decimal.Zero;
            }
            
        }

        /// <summary>
        /// Formata o valor string, substituindo virgula por ponto
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        public static String FormatStringToDecimal(String numero)
        {
            var numeroSplit = numero.Split(',');
            if (numeroSplit.Length > 1)
            {
                numeroSplit[0] = numeroSplit[0].Replace('.', ',');
                return numeroSplit[0] + "." + numeroSplit[1];
            }
            else
            {
                return numeroSplit[0];
            }
        }
    }
}