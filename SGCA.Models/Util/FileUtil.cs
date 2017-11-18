using NetUtil.Util.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Util
{
    public class FileUtil
    {


        /// <summary>
        /// Move o arquivo da pasta origem para a pasta destino
        /// </summary>
        /// <param name="importacao"></param>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public static void MoverArquivo(string arquivo, string origem, string destino)
        {
            try
            {
                string origemCompleto = Path.Combine(origem, arquivo);
                string destinoCompleto = Path.Combine(destino, arquivo);
                File.Copy(origemCompleto, destinoCompleto);
                File.Delete(origemCompleto);
            }
            catch (Exception ex)
            {
                EventViewerHelper.LogException
                    (arquivo + Environment.NewLine + 
                    origem + Environment.NewLine + 
                    destino + Environment.NewLine + 
                    ex);
            }
        }
    }
}
