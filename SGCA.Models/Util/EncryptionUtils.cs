using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace SGCA.Models.Util
{
    public class EncryptionUtils
    {
        /// <summary>
        ///     Recebe um string (encoding UTF8)
        ///     e faz o hash pelo método SHA256. 
        /// </summary>
        /// <param name="s">
        ///     String a passar pela função de hash.
        ///     </param>
        /// <returns>
        ///     String resultante da função de hash SHA256.
        /// </returns>
        public static string SHA256Hash(string s)
        {
            //StringBuilder para facilitar a manipulação da string.
            StringBuilder sBuilder = new StringBuilder();

            //Instanciação do objeto que executa a função de hash.
            SHA256 shaHash = SHA256Managed.Create();
            
            //Auxiliar de encodingpara manipular a string
            //e computar a hash. (UTF8)
            Encoding enc = Encoding.UTF8;

            //Array de bytes retornado pela função de hash.
            //A função de hash só recebe um array de bytes,
            //que aqui é fornecido por 'enc'.
            byte[] bytes = shaHash.ComputeHash(enc.GetBytes(s));

            //Loop para "montar" a string de volta
            foreach (byte b in bytes)
            {
                sBuilder.Append(b.ToString("x2"));
            }

            //Retorno da string "bem" formatada.
            return sBuilder.ToString();
        }
    }
}