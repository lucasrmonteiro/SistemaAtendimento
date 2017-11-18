using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Helper {
    public abstract class StringUtil {
        /// <summary>
        /// Converter string com a primeira em maiscula e restante em minusculo
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public static string ToFirstUpper(String strString) {
            string strResult = "";
            if (strString.Length > 0) {
                strResult += strString.Substring(0, 1).ToUpper();
                strResult += strString.Substring(1, strString.Length - 1).ToLower();
            } // end if

            return strResult;
        }

        /// <summary>
        /// Format value de acordo com a cultura informada
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatCurrency(decimal value, string cultureInfo) {
            // Cria culture no formato pt-BR
            CultureInfo culture = new CultureInfo(cultureInfo);

            // Retorna valor formatado
            return value.ToString("c", culture);
        }

        /// <summary>
        /// Format number de acordo com a cultura informada
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatNumber(decimal value, string cultureInfo) {
            // Cria culture no formato pt-BR
            CultureInfo culture = new CultureInfo(cultureInfo);

            // Retorna valor formatado
            return value.ToString("N2", culture);
        }

       /// <summary>
        /// Format Integer de acordo com a cultura informada
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatInt(decimal value, string cultureInfo) {
            // Cria culture no formato pt-BR
            CultureInfo culture = new CultureInfo(cultureInfo);

            // Retorna valor formatado
            return ((int)value).ToString("d", culture);
        }
    }
}
