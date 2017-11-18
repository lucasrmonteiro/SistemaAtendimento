using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SGCA.Models.Helpers;
using SGCA.Models.Entity;
using SGCA.Models.Enums;
using SGCA.Models.Manager;
using NetUtil.Util.Spring;
using Resources;
using SGCA.Helpers;
using SGCA.Models.Util;
using SGCA.Models.DTO;

namespace SGCA.Controllers.Base
{
    public class BaseController : Controller
    {
        
        private log4net.ILog logger;

        /// <summary>
        /// Carrega o usuario logado no sistema
        /// </summary>
        /// <returns>usuario</returns>
        public SessaoDoUsuario GetSessaoDoUsuario()
        {
            SessaoDoUsuario usuarioLogado;

            try
            {
                usuarioLogado = (SessaoDoUsuario)Session[Constantes.SESSAO_DO_USUARIO];
            }
            catch (Exception)
            {
                usuarioLogado = null;
            }

            return usuarioLogado;
        }

        /// <summary> /// <summary>
        ///     Verifica existencia da planilha no request
        /// </summary>
        /// <returns>
        ///     Se planilhar existir, retorna a planilha
        ///     Se não, retorna null
        /// </returns>
        [NonAction]
        public HttpPostedFileBase VerificaExistenciaDaPlanilhaNoRequest(bool planilhaExcel = false)
        {
            // Verifica se existe algum arquivo no request 
            // Se não existir, retorna null
            if (Request.Files.Count != Constantes.EMPTY)
            {
                //Verifica se existe conteudo no arquivo
                //Se não existir, retorna null
                if (!Request.Files[0].FileName.Equals(String.Empty))
                {
                    if (planilhaExcel)
                    {
                        // Verifica se a extensão do arquivo e diferente de XLS e XLSX
                        if (!Request.Files[0].FileName.Substring(Request.Files[0].FileName.Length - 3, 3).Equals("xls") &&
                            !Request.Files[0].FileName.Substring(Request.Files[0].FileName.Length - 4, 4).Equals("xlsx"))
                        {
                            return null;
                        }
                        else
                        {
                            return Request.Files[0];
                        }
                    }
                    else
                        return Request.Files[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Método responsavel em carregar o assunto do email
        /// </summary>
        /// <param name="tipoEmail">Tipo do email que será enviado</param>
        /// <returns>string</returns>
        public string CarregaAssuntoEmail(int tipoEmail)
        {
            string assunto = "";

            switch (tipoEmail)
            {
                case (int)EnumEmailTipo.ABERTURA_SOLICITACAO:
                    assunto = SGCA.Models.Properties.Resources.mensagem_titulo_abertura_solicitacao;
                    break;
                case (int)EnumEmailTipo.ALTERACAO_SOLICITACAO:
                    assunto = SGCA.Models.Properties.Resources.mensagem_titulo_alteracao_solicitacao;
                    break;
                case (int)EnumEmailTipo.PRIMEIRO_ACESSO:
                    assunto = SGCA.Models.Properties.Resources.mensagem_titulo_acesso;
                    break;
                case (int)EnumEmailTipo.ACESSO_LIBERADO:
                    assunto = SGCA.Models.Properties.Resources.mensagem_titulo_senha_acesso;
                    break;
                case (int)EnumEmailTipo.ESQUECI_SENHA:
                    assunto = SGCA.Models.Properties.Resources.mensagem_titulo_senha_acesso;
                    break;
            }

            return assunto;
        }

        /// <summary>
        ///     Método que adiciona o anexo na pasta Anexos
        /// </summary>
        /// <param name="idSolicitacao">Id da solicitação</param>
        /// <param name="arquivoExcelNoRequest">Arquivo anexado no request</param>
        protected void AdicionaAnexoNaPastaAnexos(int idSolicitacao, HttpPostedFileBase arquivoExcelNoRequest,string local)
        {
            var fileName = idSolicitacao.ToString() + "_" + Path.GetFileName(arquivoExcelNoRequest.FileName);
            var path = Path.Combine(Server.MapPath(local), fileName);
            arquivoExcelNoRequest.SaveAs(path);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaErros"></param>
        public void AdicionaListaDeErrosNoModelState(IDictionary<int, List<char>> listaErros)
        {
            //Adiciona erros no ModelState
            foreach (var erro in listaErros.ToList())
            {
                StringBuilder builder = new StringBuilder();
                char lastItem = erro.Value.ToList().Last();
                foreach (var coluna in erro.Value.ToList())
                {
                    if (coluna.Equals(lastItem))
                    {
                        builder.Append(coluna);
                    }
                    else
                    {
                        builder.Append(coluna + ",");
                    }
                }
                ModelState.AddModelError("", String.Format(ViewMessagesResource.erro_importacao_planilha_dados_invalidos_ou_nao_preenchidos, builder, erro.Key + 1));
            }
        }


        /// <summary>
        ///     Download de um arquivo do ftp
        /// </summary>
        /// <returns>arquivo do ftp</returns>
        [HttpPost]
        protected FileResult DownloadSftp(String arquivo, String pasta, string contentType = "application/vnd.ms-excel")
        {

            var file = SFtpHelper.DownloadFtpFile(SFtpHelper.GetConnectionInfo(), pasta, arquivo, null);
  
            //Retorna o arquivo
            return File(file.Name, contentType, arquivo);
         }


        #region Log4Net

        protected log4net.ILog Logger
        {
            get { return logger != null ? logger : Logger = log4net.LogManager.GetLogger("Logs.txt"); }
            set { logger = value; }
        }

        #endregion




    }
}
