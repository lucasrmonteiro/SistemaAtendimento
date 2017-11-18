using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGCA.Models.Helpers
{
    public class FileHelper : Controller
    {
        //
        /// <summary>
        /// Método que deleta um arquivo do servidor
        /// </summary>
        /// <param name="localArquivo">local do arquivo</param>
        public static string DeletarArquivo(string localArquivo)
        {
            try
            {
                // Verifica se o arquivo existe
                if (System.IO.File.Exists(localArquivo))
                {
                    // Deleta o arquivo do local fisico
                    System.IO.File.Delete(localArquivo);
                }

                return "";
            }
            catch (Exception ex)
            {
               return Resources.mensagem_erro_excluir_anexo + ex.Message;
            }

        }

        public static long RetornaTamanhoArquivo(string localArquivo)
        {
            try
            {
                long tamanhoArquivo = 0;
                System.IO.FileStream fs = new System.IO.FileStream(localArquivo, System.IO.FileMode.Open);
    
                  // Verifica se o arquivo existe
                if (System.IO.File.Exists(localArquivo))
                {
                    // Verifica se o arquivo existe
                    tamanhoArquivo = fs.Length;
                }

                return tamanhoArquivo;
            }
            catch (Exception)
            {
                return 0;
            }

        }

    }
}
