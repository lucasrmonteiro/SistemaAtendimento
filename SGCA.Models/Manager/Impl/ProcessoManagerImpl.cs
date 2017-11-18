using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGCA.Models.Entity;
using SGCA.Models.Enums;
using SGCA.Models.Filters;
using SGCA.Models.Manager.Base.Impl;
using NetUtil.Util.Enums;
using SGCA.Models.Util;
using SGCA.Models.DAO;
using NHibernate;
using NetUtil.Util.Hibernate;
using NHibernate.Criterion;
using System.Collections;
using System.IO;
using SGCA.Models.Manager.Exceptions;
using System.Globalization;
using System.Configuration;
using System.Text;
using SGCA.Models.Properties;
using NetUtil.Util.Helper;

namespace SGCA.Models.Manager.Impl
{
    public class ProcessoManagerImpl : BaseManagerImpl<Processo>, IProcessoManager
    {
        #region Constantes
        private static string EXTENSAO_ARQUIVOS = "*.log";

        private static string PATH_RAIZ = ConfigurationManager.AppSettings[Constantes.APP_CONFIG_PASTA_RAIZ];
        private static string PATH_SUCESSO = PATH_RAIZ + Constantes.PASTA_SUCESSO;
        private static string PATH_FALHA = PATH_RAIZ + Constantes.PASTA_FALHA;
        #endregion

        public ProcessoManagerImpl(IGenericDAO dao)
        {
            this._dao = dao;
            if (!Directory.Exists(PATH_RAIZ)) Directory.CreateDirectory(PATH_RAIZ);
            if (!Directory.Exists(PATH_SUCESSO)) Directory.CreateDirectory(PATH_SUCESSO);
            if (!Directory.Exists(PATH_FALHA)) Directory.CreateDirectory(PATH_FALHA);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool VerificaSeIdExisteNaBase(int id)
        {
            Convert.ToString(id);
            return _dao.FindEntityByFilter<Processo>(new Dictionary<string, object>() { { "Id_processo", id } }, null) == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Processo GetProcessoPeloId(int id)
        {
            Convert.ToString(id);
            return _dao.FindEntityByFilter<Processo>(new Dictionary<string, object>() { { "Id_processo", id } }, null);
        }

        /// <summary>
        ///     Método que busca um processo no banco usando o 'numero_processo'.
        /// </summary>
        /// <param name="numero_processo">
        ///     Int para busca do processo no banco.
        /// </param>
        /// <returns>
        ///     Se encontrar, retorna o primeiro 'Usuario' encontrado.
        ///     Caso contrário, retorna 'null'.
        /// </returns>
        public Processo BuscaProcessoPorNumero(int numero_processo)
        {
            //Cria um filtro de campos
            IDictionary<string, object> fieldsFilter = new Dictionary<string, object>();

            //Adiciona o campo 'Num_processo' ao filtro
            fieldsFilter.Add("Num_processo", numero_processo);

            //Busca usuários no banco, filtrando por 'Dsc_login'
            IList<Processo> processos = _dao.FindByFilter<Processo>(fieldsFilter, null);

            return processos.Count.Equals(0) ? null : processos[0];
        }

        /// <summary>
        ///     Atualiza data do processo no banco.
        ///     Busca processo na base utilizando o numero_processo
        ///     e depois atualiza a data.
        /// </summary>
        /// <param name="numero_processo">
        ///     Int com o numero_processo do processo a ser buscado.
        /// </param>
        /// <param name="data">
        ///     Datetime com a nova data para ser atualizada no banco.
        /// </param>
        /// <returns>
        ///     Sucesso: Objeto Usuario.
        ///     Falha: null.
        /// </returns>
        public Processo AtualizaProcesso(int numero_processo, DateTime NovaDataExtracao)
        {
            Processo processo = BuscaProcessoPorNumero(numero_processo);

            if (processo != null)
            {
                processo.DataExtracao = NovaDataExtracao;

                _dao.Alterar(processo); 
            }

            return processo;
        }



        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroProcesso filtro)
        {
            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, object> fieldsFilter = new Dictionary<string, object>();

            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction = new Dictionary<string, IDictionary<Restriction, object>>();

            if (filtro.Id_processo != 0)
            {
                IDictionary<Restriction, object> filtroIdProcesso = new Dictionary<Restriction, object>();
                filtroIdProcesso.Add(Restriction.Eq, filtro.Id_processo);
                fieldsFilterWithRestriction.Add("Dsc_cpf", filtroIdProcesso);
            }

            if (filtro.Num_processo != 0)
            {
                IDictionary<Restriction, object> filtroNumProcesso = new Dictionary<Restriction, object>();
                filtroNumProcesso.Add(Restriction.LikeRight, filtro.Num_processo);
                fieldsFilterWithRestriction.Add("Num_processo", filtroNumProcesso);
            }

            return fieldsFilterWithRestriction;
        }


        /// <summary>
        ///     Através do objeto filtro, cria objeto 'Criteria'
        ///     para filtrar resultados da consulta.
        /// </summary>
        /// <param name="filtro">
        ///     Objeto com valores preenchidos para montagem do filtro
        ///     de resultados da busca.
        /// </param>
        /// <returns>
        ///     Objeto 'Criteria' preenchido de acordo com parâmetros da busca.
        ///     Se não houver nenhum valor definido, retorna 'null'.
        /// </returns>
        public ICriteria MontaCriteriasValidos(FiltroProcesso filtro)
        {
            ISession sessao = HibernateUtil.GetCurrentSession();
            ICriteria criteria = sessao.CreateCriteria<Processo>();

            bool noFilter = true;

            if (filtro.Id_processo != 0)
            {
                // Adiciona restrição de "Id_solicitacao"
                criteria.Add(Expression.Eq("Id_processo",
                                            filtro.Id_processo));
                noFilter = false;
            }

            if (filtro.Dat_extracao == null)
            {
                // Adiciona restrição a data de extração
                criteria.Add(Expression.Eq("Dat_extracao",
                                            filtro.Dat_extracao));
                noFilter = false;
                
            }

            if (noFilter)
            {
                criteria = null;
                HibernateUtil.CloseSession();
            }

            return criteria;
        }

        public void InsereProcessoNaBase(Processo processo)
        {
            _dao.Incluir<Processo>(processo);
        }


        public void SaveOrUpdate(Processo processo)
        {
            _dao.SaveOrUpdate<Processo>(processo);
        }


        public void Delete(Processo processo)
        {
            Processo p = new Processo();
            p = _dao.FindByPK<Processo>(processo.CodigoProcesso);
            _dao.Excluir<Processo>(p);
        }

        public Processo FindByPk(int pk)
        {
            return _dao.FindByPK<Processo>(pk);
        }

        public IList<Processo> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter)
        {
            return _dao.FindByFilter<Processo>(fieldsFilter, null);
        }

        public bool ImportarLegado()
        {
            bool sucessoTotal = true;
            //Captura todos os arquivos legados na pasta(.log)
            var arquivoImportacao = Directory.GetFiles(PATH_RAIZ, EXTENSAO_ARQUIVOS);
            bool sucesso = true;
            foreach (var arq in arquivoImportacao)
            {   
                try
                {
                    LeArquivoLegado(arq);
                }
                catch (Exception ex)
                {
                    sucesso = false;
                    EventViewerHelper.LogException(arq + Environment.NewLine + ex.Message, this.GetType().Name, "LeArquivoLegado");
                }
                finally
                {
                    FileUtil.MoverArquivo(Path.GetFileName(arq), PATH_RAIZ, sucesso? PATH_SUCESSO: PATH_FALHA);
                    sucessoTotal = sucessoTotal && sucesso;
                }
            }

            return sucessoTotal;
        }

        private void LeArquivoLegado(string arq)
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(File.OpenRead(arq));
                string cabecalho = reader.ReadLine();
                int count = 1;
                while (!reader.EndOfStream)
                {
                    string[] linha = reader.ReadLine().Split(Constantes.CSV_CHAR_SPLIT);
                    count++;
                    Ticket ticket;
                    try
                    {
                        ticket = LeLinhaLegado(linha);
                        AtualizaTicketLegado(ticket);
                    }
                    catch (LinhaArquivoInvalidoException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(String.Format(Resources.mensagem_erro_linha_arquivo,count,Path.GetFileName(arq)));
                        sb.AppendLine(String.Empty);
                        sb.AppendLine(ex.Message);
                        EventViewerHelper.LogBusinessFailure(sb.ToString(), this.GetType().Name, "AtualizaTicket");
                    }
                }
            }
            finally
            {
               if (reader != null)
	            {
		            reader.Dispose();
	            }     
            }
        }

        private Ticket LeLinhaLegado(string[] linha)
        {
            Ticket ticket = new Ticket();

            try
            {
                //format||outofbound exception
                ticket.NumeroTicket = Convert.ToInt64(linha[(int)EnumLayoutLog.ID]);

                ticket.RetornoLegado = DateTime.Now;

                //outofbound exception
                ticket.StatusRetorno = linha[(int)EnumLayoutLog.StatusProcessamentoLegado];
                ticket.MensagemRetorno = linha[(int)EnumLayoutLog.MensagemErro];
            }
            catch (Exception ex)
            {
                throw new LinhaArquivoInvalidoException(ex);
            }
            return ticket;
        }

        private void AtualizaTicketLegado(Ticket linha)
        {
            Ticket ticketBase = this._dao.FindByFilter<Ticket>
                    (t => t.NumeroTicket == linha.NumeroTicket).FirstOrDefault();

            if (ticketBase != null)
            {
                ticketBase.RetornoLegado = DateTime.Now;
                ticketBase.StatusRetorno = linha.StatusRetorno;
                ticketBase.MensagemRetorno = linha.MensagemRetorno;
                _dao.Alterar(ticketBase);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format
                    (Resources.mensagem_info_tickect_nao_encontrado, linha.NumeroTicket));
                throw new LinhaArquivoInvalidoException(sb.ToString());
            }
        }
            
    }
}
