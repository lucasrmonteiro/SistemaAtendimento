using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Collections;
using SGCA.Models.Enums;
using SGCA.Models.Filters;
using SGCA.Controllers.Base;
using SGCA.Models.Validators;
using SGCA.Models.Entity;
using SGCA.Models.Manager;
using SGCA.Models.Util;
using SGCA.Models.DTO;
using SGCA.Models.Manager.Impl;
using NetUtil.Util.Enums;
using NetUtil.Util.Hibernate;
using NetUtil.Util.Spring;
using NetUtil.Util.DTO;
using NHibernate.Linq;
using System.Web.Routing;
using System.Configuration;
using Resources;
using NHibernate;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace SGCA.Controllers
{
    public class AtendimentoPendenciasController : BaseController
    {
        private IAtendimentoPendenciasManager _atendimentoPendenciasManager = ServiceLocator.GetObject<AtendimentoPendenciasManagerImpl>();

        private static int PAGE_SIZE = Convert.ToInt16(ConfigurationManager.AppSettings[Constantes.APP_CONFIG_TAMANHO_PAGINA]);

        #region Managers

        //protected IGrupoManager _grupoManager = ServiceLocator.GetObject<IGrupoManager>();

        #endregion Managers

        public ActionResult ListaPendencias()
        {
            try
            {
                List<AtendimentoPendencias> model = _atendimentoPendenciasManager.GetAtendimentoPendencias();
                var session = HibernateUtil.OpenSession();
                var grupo = (from p in session.Query<TbGrupo>()
                             select p).ToList();

                var status = (from p in session.Query<TbStatusNota>()
                             select p).ToList();



                ViewBag.Vgrupo = new SelectList(grupo, "CodigoGrupo", "Descricao");
                ViewBag.Vstatus = new SelectList(status, "CodigoStatusNota", "Descricao");
                var usuarios = (from p in session.Query<Usuario>()
                                select p).ToList();
                ViewBag.Vusuarios = new SelectList(usuarios, "Id_usuario", "Dsc_nome");
                var pendencia104 = (from p in session.Query<VwPendeciasA104>()
                select p).ToList();
                
                model.ForEach(x => x.Pendencias104 = pendencia104);
                

                return View("Pendencias", model);
            }
            catch (Exception e)
            {
                return View(Constantes.VIEW_ERRO);
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
                List<AtendimentoPendencias> model = _atendimentoPendenciasManager.GetAtendimentoPendencias();
                return View(model);
            }
            catch (Exception ex)
            {
                return View(Constantes.VIEW_ERRO, ex);
            }
        }

        public ActionResult PegaConteudoModal(int numeroNota)
        {
            AtendimentoPendencias ap = _atendimentoPendenciasManager.GetPorNumeroNota(numeroNota);
            return PartialView("_ModalEdicaoPendencias", ap);
        }

        public ActionResult AlteraPendencia(AtendimentoPendencias ap)
        {

            return Json(0);
        }

        private static IList<AtendimentoPendencias> ListaMock()
        {
            AtendimentoPendencias gridAtendimento = new AtendimentoPendencias();

            gridAtendimento.CodigoNota = gridAtendimento.CodigoNota;
            //gridAtendimento.DataImportacao = gridAtendimento.DataImportacao;
            gridAtendimento.NumeroNota = gridAtendimento.NumeroNota;
            gridAtendimento.TipoNota = gridAtendimento.TipoNota;
            gridAtendimento.DataInicioDesejado = gridAtendimento.DataInicioDesejado;
            gridAtendimento.Instalacao = gridAtendimento.Instalacao;
            gridAtendimento.StatusSistema = gridAtendimento.StatusSistema;
            gridAtendimento.StatusUsuario = gridAtendimento.StatusUsuario;
            gridAtendimento.SegmentoCliente = gridAtendimento.SegmentoCliente;
            gridAtendimento.TextoCodCodif = gridAtendimento.TextoCodCodif;
            gridAtendimento.CodPendencia = gridAtendimento.CodPendencia;
            gridAtendimento.DescPendencia = gridAtendimento.DescPendencia;
            gridAtendimento.CodAreaDirecionada = gridAtendimento.CodAreaDirecionada;
            gridAtendimento.DescAreaDirecionada = gridAtendimento.DescAreaDirecionada;
            gridAtendimento.DataAtendimento = gridAtendimento.DataAtendimento;
            gridAtendimento.Responsavel = gridAtendimento.Responsavel;
            gridAtendimento.GrupoAtendimento = gridAtendimento.GrupoAtendimento;
            gridAtendimento.Observacoes = gridAtendimento.Observacoes;

            IList<AtendimentoPendencias> model = new List<AtendimentoPendencias>();

            model.Add(gridAtendimento);
            return model;
        }

        [HttpPost]
        public ActionResult AlterarGridMobilidade(DTO_Alterar_Grid_Mobilidade obj)
        {
            try
            {
                List<AtendimentoPendencias> model = _atendimentoPendenciasManager.GetAtendimentoPendencias();
                var session = HibernateUtil.OpenSession();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    var nota = (from p in session.Query<Nota>()
                                 where p.CodigoNota == obj.cod_nota
                                 select p).FirstOrDefault();

                    var Usuario = (from p in session.Query<Usuario>()
                                   where p.Id_usuario == obj.resp_edit
                                   select p).FirstOrDefault();


                    var grupo_nota = (from p in session.Query<TbGrupo>()
                                      where p.CodigoGrupo == obj.grupo_edit
                                      select p).FirstOrDefault();

                    var status_nota = (from p in session.Query<TbStatusNota>()
                                      where p.CodigoStatusNota == obj.status_edit
                                       select p).FirstOrDefault();


                    nota.Observacao = obj.Observacoes;
                    nota.TbGrupo = grupo_nota;
                    nota.TbStatusNota = status_nota;
                    nota.TbUsuario = Usuario;

                    session.Save(nota);
                    transaction.Commit();
                }


                var grupo = (from p in session.Query<TbGrupo>()
                             select p).ToList();

                var status = (from p in session.Query<TbStatusNota>()
                              select p).ToList();
                var usuarios = (from p in session.Query<Usuario>()
                                select p).ToList();
                ViewBag.Vusuarios = new SelectList(usuarios, "Id_usuario", "Dsc_nome");

                ViewBag.Vgrupo = new SelectList(grupo, "CodigoGrupo", "Descricao");
                ViewBag.Vstatus = new SelectList(status, "CodigoStatusNota", "Descricao");
                var pendencia104 = (from p in session.Query<VwPendeciasA104>()
                                    select p).ToList();

                model.ForEach(x => x.Pendencias104 = pendencia104);
                return View("Pendencias", model);
            }
            catch (Exception e)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }

        public ActionResult ExportData()
        {
            GridView gv = new GridView();
            gv.DataSource = _atendimentoPendenciasManager.GetAtendimentoPendencias().ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Grid.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("ListaPendencias");
        }

        public ActionResult ExportData1()
        {
            GridView gv = new GridView();
            var session = HibernateUtil.OpenSession();
            gv.DataSource = (from p in session.Query<VwPendeciasA104>()
                             select p).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Grid.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("ListaPendencias");
        }

        [HttpPost]
        public ActionResult AlterarGridAcao104(DTO_Alterar_Grid_104 obj)
        {
            try
            {
                List<AtendimentoPendencias> model = _atendimentoPendenciasManager.GetAtendimentoPendencias();
                var session = HibernateUtil.OpenSession();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    var nota = (from p in session.Query<Nota>()
                                where p.CodigoNota == obj.cod_nota
                                select p).FirstOrDefault();

                    var Usuario = (from p in session.Query<Usuario>()
                                   where p.Id_usuario == obj.resp_edit_104
                                select p).FirstOrDefault();

                    var grupo_nota = (from p in session.Query<TbGrupo>()
                                      where p.CodigoGrupo == obj.grupo_edit
                                      select p).FirstOrDefault();

                    var status_nota = (from p in session.Query<TbStatusNota>()
                                       where p.CodigoStatusNota == obj.status_edit
                                       select p).FirstOrDefault();

                    var pendencia = (from p in session.Query<PendenciaNota>()
                                     where p.Nota.CodigoNota == obj.cod_nota
                                     select p);
                    
                    if (pendencia.Any())
                    {
                        var oDados = pendencia.FirstOrDefault();
                        oDados.CentrabRespon = obj.CentrabRespon;
                        session.Save(oDados);
                    }

                    nota.TbUsuario = Usuario;
                    nota.TbGrupo = grupo_nota;
                    nota.TbStatusNota = status_nota;

                    session.Save(nota);
                    transaction.Commit();
                }


                var grupo = (from p in session.Query<TbGrupo>()
                             select p).ToList();

                var status = (from p in session.Query<TbStatusNota>()
                              select p).ToList();

                ViewBag.Vgrupo = new SelectList(grupo, "CodigoGrupo", "Descricao");
                ViewBag.Vstatus = new SelectList(status, "CodigoStatusNota", "Descricao");
                var usuarios = (from p in session.Query<Usuario>()
                                select p).ToList();
                ViewBag.Vusuarios = new SelectList(usuarios, "Id_usuario", "Dsc_nome");
                var pendencia104 = (from p in session.Query<VwPendeciasA104>()
                                    select p).ToList();

                model.ForEach(x => x.Pendencias104 = pendencia104);
                
                return View("Pendencias", model);
            }
            catch (Exception e)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }


        public ActionResult BuscaPendenciasMobilidade(string SearchKey)
        {
            try
            {
                var _code_nota = Convert.ToInt32(SearchKey);
                List<AtendimentoPendencias> model = _atendimentoPendenciasManager.GetAtendimentoPendencias();
                var session = HibernateUtil.OpenSession();

                var grupo = (from p in session.Query<TbGrupo>()
                             select p).ToList();

                var status = (from p in session.Query<TbStatusNota>()
                              select p).ToList();

                ViewBag.Vgrupo = new SelectList(grupo, "CodigoGrupo", "Descricao");
                ViewBag.Vstatus = new SelectList(status, "CodigoStatusNota", "Descricao");
                var usuarios = (from p in session.Query<Usuario>()
                                select p).ToList();
                ViewBag.Vusuarios = new SelectList(usuarios, "Id_usuario", "Dsc_nome");
                var pendencia104 = (from p in session.Query<VwPendeciasA104>()
                                    select p).ToList();

                model = model.Where(x => x.CodigoNota == _code_nota).ToList();

                model.ForEach(x => x.Pendencias104 = pendencia104);

                return View("Pendencias", model);
            }
            catch (Exception e)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }

        public ActionResult BuscaPendenciasA104(string SearchKey)
        {
            try
            {
                var _code_nota = Convert.ToInt32(SearchKey);
                List<AtendimentoPendencias> model = _atendimentoPendenciasManager.GetAtendimentoPendencias();
                var session = HibernateUtil.OpenSession();

                var grupo = (from p in session.Query<TbGrupo>()
                             select p).ToList();

                var status = (from p in session.Query<TbStatusNota>()
                              select p).ToList();

                ViewBag.Vgrupo = new SelectList(grupo, "CodigoGrupo", "Descricao");
                ViewBag.Vstatus = new SelectList(status, "CodigoStatusNota", "Descricao");
                var usuarios = (from p in session.Query<Usuario>()
                                select p).ToList();
                ViewBag.Vusuarios = new SelectList(usuarios, "Id_usuario", "Dsc_nome");
                var pendencia104 = (from p in session.Query<VwPendeciasA104>()
                                    where p.CodigoNota == _code_nota
                                    select p).ToList();

                model.ForEach(x => x.Pendencias104 = pendencia104);

                return View("Pendencias", model);
            }
            catch (Exception e)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }

    }
}
