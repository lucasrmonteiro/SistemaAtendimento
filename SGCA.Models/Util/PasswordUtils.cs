using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SGCA.Models.Util
{
    public class PasswordUtils
    {

        public static char[] caracteresValidosSenha = {  
	        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',  
	        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',  
	        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',  
	        '$', '%', '@','#'  
	        };

        static char[] caracteresValidosLetrasMin = { 'a', 'b', 'c', 'd', 'e', 'f', 'g',
			'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z' };

        static char[] caracteresValidosLetrasMai = { 'A', 'B', 'C', 'D', 'E', 'F', 'G',
			'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
			'U', 'V', 'W', 'X', 'Y', 'Z' };

        static char[] caracteresValidosNum = { '1', '2', '3', '4', '5', '6', '7', '8',
			'9', '0' };

        static char[] caracteresValidosEsp = { '$', '%', '@', '#' };

 
        /// <summary>
        ///     Metodo responsavel por gerar uma nova senha para usuário. 
        /// </summary>
        /// <returns></returns>
        public static String GerarSenha()
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 13; i++)
            {
                sb.Append(caracteresValidosSenha[r.Next(caracteresValidosSenha.Length)]);
            }

            return sb.ToString();
        }
    }
}