using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGCA.Controllers.Base;
using SGCA.Models.Validators;
using SGCA.Models.Entity;
using SGCA.Models.Manager;
using NetUtil.Util.Spring;
using System.Collections;
using SGCA.Models.Enums;
using SGCA.Models.Filters;
using Resources;
using SGCA.Models.Util;
using NetUtil.Util.Enums;
using SGCA.Models.DTO;
using NetUtil.Util.Hibernate;
using NHibernate.Linq;
using System.Web.Routing;
using System.Configuration;
using System.IO;
using NHibernate;
using System.Security.Policy;
using System.Web.UI;
using System.Net;

namespace SGCA.Controllers
{
    public class AtendimentoPriorizacaoController : BaseController
    {
        public ActionResult ListaProcessos()
        {
            try
            {
                ViewData["disablecontrols"] = false;

                PopulateViewbags();
                var model = PopulateModel(0);

                return View("Priorizacao", model);
            }
            catch (Exception)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }

        private static Atendimento PopulateModel(int id_ticket)
        {
            
            var model = new Atendimento();
            //Teste (Entender)



            var session = HibernateUtil.OpenSession();

            var CodigoStatusTicket = (from p in session.Query<TbStatusTicket>()
                                      where p.Descricao == "Encerrado"
                                  select p.CodigoStatusTicket).FirstOrDefault();

            if (id_ticket > 0) 
            {
                var teste = (from p in session.Query<Ticket>()
                             where p.CodigoTicket == id_ticket
                             && p.TbStatusTicket.CodigoStatusTicket != CodigoStatusTicket
                             select p).ToList();
                
                if (teste.Count() == 0) 
                {
                    return model;
                }

                model.SLA_Cliente = (from p in session.Query<Ticket>()
                                     where p.CodigoTicket == id_ticket
                                     select p.SlaCliente).FirstOrDefault();

                var data_cracao = (from p in session.Query<Ticket>()
                                     where p.CodigoTicket == id_ticket
                                     select p.DataCriacao).FirstOrDefault();

                var subtract = (DateTime.Now - data_cracao.Value);

                model.Aging = subtract.Days.ToString() + " dias";
                
                model.Atividade = (from p in session.Query<Ticket>()
                                   where p.CodigoTicket == id_ticket
                                   select p.Atividade).FirstOrDefault();

                model.Bandeira = getFarolTicket((from p in session.Query<Ticket>()
                                   where p.CodigoTicket == id_ticket
                                   select p).FirstOrDefault());

                model.Dt_exportacao = (from p in session.Query<Ticket>()
                                       where p.CodigoTicket == id_ticket
                                       select p.DataEncerramento).FirstOrDefault();
                
                model.Dt_importacao = (from p in session.Query<Ticket>()
                                       where p.CodigoTicket == id_ticket
                                       select p.DataExtracao).FirstOrDefault();

                model.Id_Atendimento = id_ticket;
                model.Fluxo = (from p in session.Query<FluxoAtendimento>()
                               join c in session.Query<Ticket>()
                               on p.Codigo equals c.TbFluxoAtendimento.Codigo
                               where c.CodigoTicket == id_ticket
                               select p).FirstOrDefault();

                model.Tickets = (from p in session.Query<Ticket>()
                                 where p.CodigoTicket == id_ticket
                                   select p).ToList();

                model.Dt_criacao = model.Tickets[0].DataCriacao;
                model.Dt_extracao = model.Tickets[0].DataExtracao;
                model.Dt_encerramento = model.Tickets[0].DataEncerramento;
                model.Status = model.Tickets[0].TbStatusTicket;
                model.Solicitacao = model.Tickets[0].TbTipoSolicitacao;
                model.Tickets[0].Farol = getFarolTicket(model.Tickets[0]);
                //model.Responsavel = model.Tickets[0];
                model.Responsavel = (from p in session.Query<Nota>()
                                     where p.Ticket.CodigoTicket == id_ticket
                                     select p.TbUsuario.Dsc_nome).FirstOrDefault();
                model.id_responsavel = (from p in session.Query<Nota>()
                                     where p.Ticket.CodigoTicket == id_ticket
                                     select p.TbUsuario.Id_usuario).FirstOrDefault();
                model.Demanda = model.Tickets[0].TbDemanda;
                model.Notas = (from p in session.Query<Nota>()
                                 where p.Ticket.CodigoTicket == id_ticket
                                 select p).ToList();

                model.Historico = (from p in session.Query<TbTicketHistorico>()
                                   where p.TbTicket.CodigoTicket == id_ticket
                                   select p).ToList();

                var acao = (from p in session.Query<TbAcao>()
                                   where p.CodigoTicket == id_ticket
                                   select p);

                if (acao.Any())
                {
                    model.Acao = acao.FirstOrDefault();

                }
                
                var list_modalidade = new List<TbMobilidade>();

                foreach (var item in model.Notas) 
                {
                    var mob = from p in session.Query<TbMobilidade>()
                              where p.TbNota.CodigoNota == item.CodigoNota
                              select p;

                    if (mob.Any()) 
                    {
                        list_modalidade.Add(mob.FirstOrDefault());
                    }
                }

                model.Mobilidade = list_modalidade;
            }
            else 
            {
                model.Fluxo = (from p in session.Query<FluxoAtendimento>()
                               select p).FirstOrDefault();

                model.Dt_criacao = null;
                model.Dt_extracao = null;
                model.Dt_encerramento = null;
                model.Dt_exportacao = DateTime.Now;
                model.Dt_importacao = DateTime.Now;

                model.Tickets = (from p in session.Query<Ticket>()
                                 where p.TbStatusTicket.CodigoStatusTicket != CodigoStatusTicket
                                   select p).ToList();

                foreach (var item in model.Tickets)
                {
                  item.Farol = getFarolTicket(item);
                }

            }

            HibernateUtil.CloseSession();
            return model;
        }


        public ActionResult Download(int id, string name)
        {
            var session = HibernateUtil.OpenSession();

            string fileName = name;

            var uploads = (from p in session.Query<TbAcao>()
                        where p.CodigoTicket == id
                        select p.PathUpload).FirstOrDefault();

            if (uploads != null)
            {

                string folder = Server.MapPath("~/App_Data/uploads");
                //HttpContext.Response.AddHeader("content-dispostion", "attachment; filename=" + fileName);
                return File(new FileStream(folder + "/" + fileName, FileMode.Open), "content-dispostion", fileName);
            }

            throw new ArgumentException("Invalid file name of file not exist");
        }


        private void PopulateViewbags()
        {
            var session = HibernateUtil.OpenSession();

            var demanda = (from p in session.Query<Demanda>()
                           select p).ToList();
            ViewBag.Vdemanda = demanda;

            var solicitacao = (from p in session.Query<TbTipoSolicitacao>()
                               select p).ToList();
            ViewBag.Vsolictacao = solicitacao;

            var status = (from p in session.Query<TbStatusTicket>()
                          select p).ToList();
            ViewBag.Vstatus = status;

            var fluxo = (from p in session.Query<FluxoAtendimento>()
                          select p).ToList();

            ViewBag.Vfluxo = fluxo;

            var ponto_focal = (from p in session.Query<TbPontoFocal>()
                         select p).ToList();

            ViewBag.Vponto_focal = ponto_focal;

            var etapa = (from p in session.Query<TbEtepa>()
                               select p).ToList();

            ViewBag.Vetapa = etapa;

            var categoria = (from p in session.Query<TbCategoria>()
                         select p).ToList();

            ViewBag.Vcategoria = categoria;

            var status_nota = (from p in session.Query<TbStatusNota>()
                             select p).ToList();

            ViewBag.Vstatus_nota = status_nota;

            var usuario = (from p in session.Query<Usuario>()
                           where p.FlAtivo == true
                               select p).ToList();

            ViewBag.Vusuario = usuario;

            HibernateUtil.CloseSession();
        }

        public ActionResult ConsultaProcesso(int id) 
        {
            try
            {
                ViewData["disablecontrols"] = true;

                PopulateViewbags();
                var model = PopulateModel(id);

                return View("Manutencao", model);
            }
            catch (Exception)
            {
                return View(Constantes.VIEW_ERRO);
            }

        }

        [HttpPost]
        public PartialViewResult ListaProcessosFiltro(DTO_Consulta_Ticket param)
        {
            try
            {
                ViewData["disablecontrols"] = false;

                PopulateViewbags();
                
                var model = PopulateModel(param.cod_ticket ?? 0);

                //to do nao definido
                //if (!string.IsNullOrEmpty(param.responsavel.ToString()))
                //{
                //    //model.Tickets = model.Tickets.Where(x=> x.res)
                //}
                var list = new List<Ticket>();
                if (model.Tickets == null)
                {
                    model.Tickets = list;
                    return PartialView("_GridTickets", model);
                }
                else 
                {
                    list = model.Tickets.ToList();
                }
                if (param.dt_criacao != null) 
                {
                    list.RemoveAll(x => x.DataCriacao != param.dt_criacao);
                  
                }

                if (param.Dt_extracao != null)
                {
                    list.RemoveAll(x => x.DataExtracao != param.Dt_extracao);
                }

                if (param.Dt_encerramento != null)
                {
                    list.RemoveAll(x => x.DataEncerramento != param.Dt_encerramento);
                }

                if (param.id_demanda != null)
                {
                    list.RemoveAll(x => x.TbDemanda.Codigo != param.id_demanda);
                }

                if (param.id_solicitacao != null)
                {
                    list.RemoveAll(x => x.TbTipoSolicitacao.CodigoTipoSolicitacao != param.id_solicitacao);
                }

                if (param.id_status != null)
                {
                    list.RemoveAll(x => x.TbStatusTicket.CodigoStatusTicket != param.id_status);
                }

                model.Tickets = list;

                return PartialView("_TabelasConsultaTicket", model);
            }
            catch (Exception)
            {
                //return View(Constantes.VIEW_ERRO);
                throw;
            }
        }


        public ActionResult salvaAlteracoesSAP(DTO_Alteracao_SAP obj)
        {
            try
            {
                var session = HibernateUtil.OpenSession();

                var valida_acao = (from p in session.Query<TbAcao>()
                    where p.CodigoTicket == obj.id_ticket
                    select p);

                if (!valida_acao.Any())
                {
                    InserirAcaoSAP(obj, session);
                }
                else
                {
                    AtualizaAcaoSAP(obj, session, valida_acao);
                }

   
                HibernateUtil.CloseSession();

                ViewData["disablecontrols"] = true;

                PopulateViewbags();
                var model = PopulateModel(obj.id_ticket);

                return View("Manutencao", model);
            }
            catch (Exception)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }

        [HttpPost]
        public ActionResult SalvarSessaoTicket(DTO_Ticket_Salvar obj)
        {
            try
            {
                var session = HibernateUtil.OpenSession();

                var vet = obj.status_notas.Split('|').ToList();

                vet = vet.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                var notas = (from p in session.Query<Nota>()
                             where p.Ticket.CodigoTicket == obj.id_ticket
                             select p);
                
                var ticket = (from p in session.Query<Ticket>()
                             where p.CodigoTicket == obj.id_ticket
                             select p).FirstOrDefault();
                
                using (ITransaction transaction = session.BeginTransaction())
                {
                    int i = 0;
                    foreach (var item in notas)
                    {
                        int _t = Convert.ToInt32(vet[i]);
                        var status = (from p in session.Query<TbStatusNota>()
                                      where p.CodigoStatusNota == _t
                                      select p).FirstOrDefault();
                        item.TbStatusNota = status;
                        session.SaveOrUpdate(item);
                        i++;
                    }

                    var dt_inicial = new DateTime?();
                    var _dt_inicial = (from p in session.Query<TbTicketHistorico>()
                                      where p.TbTicket.CodigoTicket == obj.id_ticket
                                      select p);
                    if (_dt_inicial.Any()) { dt_inicial = _dt_inicial.First().DataInicio; } else { dt_inicial = DateTime.Now; }

                    var historico = new TbTicketHistorico()
                    {
                        TbTicket = ticket,
                        TbStatusTicket = ticket.TbStatusTicket,
                        DataInicio = dt_inicial,
                        DataFim = DateTime.Now,
                        Anotacoes = obj.anotacoes
                    };
                    session.Save(historico);

                    transaction.Commit();
                }
              
                ViewData["disablecontrols"] = true;

                PopulateViewbags();
                var model = PopulateModel(obj.id_ticket);
                session.Close();
                return View("Manutencao", model);
            }
            catch (Exception)
            {
                return View(Constantes.VIEW_ERRO);
            }

        }

        private static void AtualizaAcaoSAP(DTO_Alteracao_SAP obj, NHibernate.ISession session, IQueryable<TbAcao> valida_acao)
        {
            var oDados = valida_acao.FirstOrDefault();

            var ticket = (from p in session.Query<Ticket>()
                          where p.CodigoTicket == obj.id_ticket
                          select p).FirstOrDefault();

            using (ITransaction transaction = session.BeginTransaction())
            {
                ticket.TbStatusTicket.CodigoStatusTicket = obj.id_status;
                ticket.Mensagens = obj.notas;

                oDados.CodigoTicket = obj.id_ticket;
                oDados.CodigoStatusAcaoSap = obj.id_status;
                oDados.CodigoCategoria = obj.id_categoria;
                oDados.Mensagem = obj.notas;

                session.Save(oDados);
                session.Save(ticket);

                transaction.Commit();
            }
        }

        private static void InserirAcaoSAP(DTO_Alteracao_SAP obj, NHibernate.ISession session)
        {
            var ticket = (from p in session.Query<Ticket>()
                          where p.CodigoTicket == obj.id_ticket
                          select p).FirstOrDefault();

            ticket.TbStatusTicket.CodigoStatusTicket = obj.id_status;
            ticket.Mensagens = obj.notas;

            var acao = new TbAcao()
            {
                CodigoTicket = obj.id_ticket,
                CodigoStatusAcaoSap = obj.id_status,
                CodigoCategoria = obj.id_categoria,
                Mensagem = obj.notas
            };

            session.Save(acao);
            session.Save(ticket);
        }

        [HttpPost]
        public ActionResult salvaAlteracoesCAP(DTO_Alteracao_CAP obj)
        {
            try
            {
                var session = HibernateUtil.OpenSession();

                var valida_acao = (from p in session.Query<TbAcao>()
                                   where p.CodigoTicket == obj.id_ticket
                                   select p);

                if (!valida_acao.Any())
                {

                    InserirNovaAcaoCAP(obj, session);
                }
                else
                {
                    AtualizaAcaoCAP(obj, session, valida_acao);
                }

                HibernateUtil.CloseSession();

                ViewData["disablecontrols"] = true;

                PopulateViewbags();
                var model = PopulateModel(obj.id_ticket);

                return View("Manutencao", model);
            }
            catch (Exception)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }

        private void AtualizaAcaoCAP(DTO_Alteracao_CAP obj, NHibernate.ISession session, IQueryable<TbAcao> valida_acao)
        {
            var oDados = valida_acao.FirstOrDefault();

            var ticket = (from p in session.Query<Ticket>()
                          where p.CodigoTicket == obj.id_ticket
                          select p).FirstOrDefault();


            var ponto_focal = (from p in session.Query<TbPontoFocal>()
                               where p.IdPontoFocal == obj.id_ponto_focal
                               select p).FirstOrDefault();
            using (ITransaction transaction = session.BeginTransaction())
            {
                bool outra_area = false;
                if (obj.id_nescessidade == 1) { outra_area = true; }
                ticket.AtendimentoOutraArea = outra_area;

                ticket.Mensagens = obj.mensagem;
                string savedFileName = Path.Combine(Server.MapPath("~/App_Data/uploads"), Path.GetFileName(obj.file.FileName));

                oDados.CodigoTicket = obj.id_ticket;
                oDados.Mensagem = obj.mensagem;
                oDados.Observacao = obj.observacoes;
                oDados.TbPontoFocal = ponto_focal;
                oDados.DetalheAcao = obj.detalhe_acoes;
                if (obj.id_etapa != 0)
                {
                    oDados.CodigoEtapa = obj.id_etapa;
                }

                oDados.PathUpload = obj.file.FileName;
                session.Save(oDados);
                //FileUpload
                obj.file.SaveAs(savedFileName); // Save the file
                transaction.Commit();
            }


        }

        private void InserirNovaAcaoCAP(DTO_Alteracao_CAP obj, NHibernate.ISession session)
        {
            var ticket = (from p in session.Query<Ticket>()
                          where p.CodigoTicket == obj.id_ticket
                          select p).FirstOrDefault();


            var ponto_focal = (from p in session.Query<TbPontoFocal>()
                               where p.IdPontoFocal == obj.id_ponto_focal
                               select p).FirstOrDefault();

            using (ITransaction transaction = session.BeginTransaction())
            {
                bool outra_area = false;
                if (obj.id_nescessidade == 1) { outra_area = true; }
                ticket.AtendimentoOutraArea = outra_area;

                ticket.Mensagens = obj.mensagem;
                string savedFileName = Path.Combine(Server.MapPath("~/App_Data/uploads"), Path.GetFileName(obj.file.FileName));

                var acao = new TbAcao()
                {
                    CodigoTicket = obj.id_ticket,
                    Mensagem = obj.mensagem,
                    Observacao = obj.observacoes,
                    DetalheAcao = obj.detalhe_acoes,
                    TbPontoFocal = ponto_focal,
                    CodigoEtapa = obj.id_etapa,
                    PathUpload = obj.file.FileName
                };

                session.Save(acao);
                session.Save(ticket);
                transaction.Commit();
                //FileUpload
                obj.file.SaveAs(savedFileName); // Save the file
            }

        }

        [HttpPost]
        public ActionResult EncerrarTicket(DTO_Ticket_Salvar obj)
        {
            try
            {
                SalvarSessaoTicket(obj);

                var session = HibernateUtil.OpenSession();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    var ticket = (from p in session.Query<Ticket>()
                                  where p.CodigoTicket == obj.id_ticket
                                  select p).FirstOrDefault();

                    var TbStatusTicket = (from p in session.Query<TbStatusTicket>()
                                          where p.Descricao == "Encerrado"
                                  select p).FirstOrDefault();


                    ticket.TbStatusTicket = TbStatusTicket;

                    var dt_inicial = new DateTime?();
                    var _dt_inicial = (from p in session.Query<TbTicketHistorico>()
                                       where p.TbTicket.CodigoTicket == obj.id_ticket
                                       select p);
                    if (_dt_inicial.Any()) { dt_inicial = _dt_inicial.First().DataInicio; } else { dt_inicial = DateTime.Now; }

                    var historico = new TbTicketHistorico()
                    {
                        TbTicket = ticket,
                        TbStatusTicket = ticket.TbStatusTicket,
                        DataInicio = dt_inicial,
                        DataFim = DateTime.Now,
                        Anotacoes = obj.anotacoes
                    };
                    session.Save(historico);

                    transaction.Commit();
                }

                ViewData["disablecontrols"] = true;

                PopulateViewbags();
                var model = PopulateModel(obj.id_ticket);

                return View("Priorizacao", model);
            }
            catch (Exception)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }

        public static string getFarolTicket(Ticket obj)
        {
            var farol = "Erro no Farol";

            var subtract_date = DateTime.Now - obj.DataCriacao;

            if (obj.TbDemanda.Codigo != 1)
            {
                if (subtract_date.Value.TotalHours > 0 && subtract_date.Value.TotalHours <= 24)
                {
                    farol = "Verde";
                }
                else if (subtract_date.Value.TotalHours > 24 && subtract_date.Value.TotalHours <= 48)
                {
                    farol = "Amarelo";
                }
                else if (subtract_date.Value.TotalHours > 48)
                {
                    farol = "Vermelho";
                }
            }
            else
            {
                if (subtract_date.Value.TotalHours > 0 && subtract_date.Value.TotalHours <= 8)
                {
                    farol = "Verde";
                }
                else if (subtract_date.Value.TotalHours > 4 && subtract_date.Value.TotalHours <= 8)
                {
                    farol = "Amarelo";
                }
                else if (subtract_date.Value.TotalHours > 8)
                {
                    farol = "Vermelho";
                }
            }



            return farol;
        }

        [HttpPost]
        public ActionResult TrocarRespNotas(DTO_Trocar_Resp_Notas obj)
        {
            try
            {
                var session = HibernateUtil.OpenSession();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    var notas = (from p in session.Query<Nota>()
                                  where p.Ticket.CodigoTicket == obj.id_ticket
                                  select p).ToList();

                    var usu = (from p in session.Query<Usuario>()
                             where p.Id_usuario == obj.id_resp
                            select p).FirstOrDefault();

                    foreach (var item in notas)
                    {
                        item.TbUsuario = usu;
                        session.SaveOrUpdate(item);
                    }

                    transaction.Commit();
                }

                ViewData["disablecontrols"] = true;

                PopulateViewbags();
                var model = PopulateModel(obj.id_ticket);
                return View("Manutencao", model);
            }
            catch (Exception)
            {
                return View(Constantes.VIEW_ERRO);
            }
        }



    }
}
