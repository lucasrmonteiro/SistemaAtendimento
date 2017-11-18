using NetUtil.Util.DTO;
using NetUtil.Util.Spring;
using Resources;
using SGCA.Controllers.Base;
using SGCA.Models.Entity;
using SGCA.Models.Manager;
using SGCA.Models.Manager.Exceptions;
using SGCA.Models.Manager.Impl;
using SGCA.Models.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGCA.Controllers
{
    public class ImportacaoController : BaseController
    {
        private IImportacaoManager _importacaoManager = ServiceLocator.GetObject<ImportacaoManagerImpl>();

        private static int PAGE_SIZE = Convert.ToInt16(ConfigurationManager.AppSettings[Constantes.APP_CONFIG_TAMANHO_PAGINA]);
        private int atualizacaoPagina = Convert.ToInt16(ConfigurationManager.AppSettings[Constantes.APP_CONFIG_ATUALIZACAO_PAGINA_IMPORTACAO]);

        /// <summary>
        /// Abre a pagina inicial de importacao
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportacaoArquivo()
        {
            try
            {
                ViewBag.TempoAtualizacao = atualizacaoPagina;
                ViewBag.TamanhoPagina = PAGE_SIZE;
                ViewBag.ListaCombo = _importacaoManager.GetArquivosImportacao();
                return View("ImportacaoArquivo", new Importacao());
            }
            catch (Exception ex)
            {
                return View(Constantes.VIEW_ERRO, ex);
            }
        }

        /// <summary>
        /// Retorna a lista da combo de arquivos da tela
        /// </summary>
        /// <returns></returns>
        public ActionResult ListaCombo()
        {
            try
            {
                IList<string> lista = _importacaoManager.GetArquivosImportacao();

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View(Constantes.VIEW_ERRO, ex);
            }
        }
        
        /// <summary>
        /// Método que faz a consulta paginada.
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public ActionResult AjaxGetJsonData(int draw, int start, int length)
        {
            try
            {
                var currentPage = (start / length) + 1;
                int idUsuario = GetSessaoDoUsuario().Usuario.Id_usuario;

                DataTableData dados = _importacaoManager.GetImportacoes(length, currentPage, idUsuario);
                dados.draw = draw;
                return Json(dados, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View(Constantes.VIEW_ERRO, ex);
            }
        }

        /// <summary>
        /// Adiciona o arquivo para importacao e importa
        /// </summary>
        /// <param name="importacao"></param>
        /// <returns></returns>
        public ActionResult AdicionarImportacao(Importacao importacao)
        {
            try
            {
                importacao.Analista = GetSessaoDoUsuario().Usuario;
                this._importacaoManager.AdicionaExecutaImportacao(importacao);
                ViewBag.MensagemSucesso = ViewMessagesResource.mensagem_sucesso_adicionar_executar_importacao;
            }
            catch (ManagerException ex)
            {
                ViewBag.MensagemErro = ex.Message;
            }
            catch (Exception ex)
            {
                return View(Constantes.VIEW_ERRO, ex);
            }

            return ImportacaoArquivo();
        }
    }
}
