using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SGCA.Controllers.Base;
using SGCA.Models.Entity;
using System.Collections;
using SGCA.Models.Filters;
using NetUtil.Util.Enums;
using SGCA.Models.DTO;
using NetUtil.Util.Hibernate;
using NHibernate.Linq;
using System.Web.Routing;
using Jitbit.Utils;
using System.Configuration;
using SGCA.Models.Manager;
using NetUtil.Util.Spring;
using SGCA.Models.Util;
using System.IO;

namespace SGCA.Controllers
{
    public class ProcessoController : BaseController
    {
        #region Managers

        //protected IPerfilManager _perfilManager = ServiceLocator.GetObject<IPerfilManager>();

        protected IProcessoManager _processoManager = ServiceLocator.GetObject<IProcessoManager>();

        #endregion Managers

        /// <summary>
        /// Método que retorna a página de gerar atualização de sistemas legados
        /// </summary>
        /// <returns></returns>
        public ActionResult GeraAtualizacao()
        {
            ViewBag.ControllerAction = "Processo";
            ViewBag.UrlRequestAction = "GeraAtualizacao";
            ViewBag.UrlRequestController = "Processo";
            return View();
        }

        /// <summary>
        /// Método que retorna a página de atendimento de processos
        /// </summary>
        /// <returns></returns>
        public ActionResult AtendimentoProcesso()
        {
            //Adicicona a lista de perfis na visao
            //ViewBag.BagPerfis = _perfilManager.FindAll();
            ViewBag.ControllerAction = "Processo";
            ViewBag.UrlRequestAction = "AtendimentoProcesso";
            ViewBag.UrlRequestController = "Processo";
            return View();
        }

        /// <summary>
        /// Método que retorna a página de consulta de processos
        /// </summary>
        /// <returns></returns>
        public ActionResult ConsultaProcesso()
        {
            //Adicicona a lista de perfis na visao
            //ViewBag.BagPerfis = _perfilManager.FindAll();
            ViewBag.ControllerAction = "Processo";
            ViewBag.UrlRequestAction = "AtendimentoProcesso";
            ViewBag.UrlRequestController = "Processo";
            return View();
        }

        /// <summary>
        /// Método que filtra os usuarios
        /// </summary>
        /// <param name="filtro">Filtro da tela ConsultaProcesso</param>
        /// <returns>
        ///     Retorna uma parcial com os usuarios filtrados
        /// </returns>
        [HttpPost]
        public PartialViewResult FiltraProcessos(FiltroProcesso filtro)
        {
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilter = _processoManager.AdicionaFiltros(filtro);
            IList<Processo> processos = _processoManager.FindByFilter(fieldsFilter);
            return PartialView("_ProcessosFiltrados", processos);
        }

        public JsonResult GeraCSV()
        {
            try
            {
                var session = HibernateUtil.GetCurrentSession();

                var query = from p in session.Query<Ticket>()
                            where p.DataExtracao == null
                            select p;

                if (query.Any())
                {
                    var myExport = new CsvExport();

                    foreach (var item in query)
                    {
                        myExport.AddRow();
                        myExport["COD_TICKET;COD_STATUS_TICKET"] = item.CodigoTicket + ";" + item.TbStatusTicket.CodigoStatusTicket;

                    }

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), Path.GetFileName("EXTRACAO-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".out"));

                    myExport.ExportToFile(path);

                    var json = new DTO_JSON_Result()
                    {
                        status = 1,
                        msg = "{OK}"
                    };

                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var json = new DTO_JSON_Result()
                    {
                        status = 1,
                        msg = "{VAZIO}"
                    };

                    return Json(json, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                var json = new DTO_JSON_Result()
                {
                    status = 0,
                    msg = ex.Message.ToString()
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Método que faz a importação dos arquivos legados
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportarLegado()
        {
            try
            {
                if (this._processoManager.ImportarLegado()) 
                {
                    ViewBag.MensagemSucesso = Resources.ViewMessagesResource.mensagem_sucesso_importar_legado;
                }
                else 
                {
                    ViewBag.MensagemErro = Resources.ViewMessagesResource.mensagem_erro_importar_legado;
                }
            }
            catch (Exception ex)
            {
                return View(Constantes.VIEW_ERRO, ex);
            }
            return GeraAtualizacao();
        }
    }
    
}
