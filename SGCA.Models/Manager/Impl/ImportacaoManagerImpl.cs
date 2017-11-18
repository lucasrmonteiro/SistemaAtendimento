using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGCA.Models.Entity;
using SGCA.Models.Manager.Base;
using SGCA.Models.DAO;
using SGCA.Models.Manager.Base.Impl;
using System.IO;
using SGCA.Models.Manager.Exceptions;
using SGCA.Models.Enums;
using SGCA.Models.Helpers;
using SGCA.Models.Util;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Text;
using NetUtil.Util.Helper;
using SGCA.Models.Properties;
using NetUtil.Util.DTO;

namespace SGCA.Models.Manager.Impl
{
    public class ImportacaoManagerImpl : BaseManagerImpl<Importacao>, IImportacaoManager
    {
        //TODO: verificar onde deve ser o diretorio ou onde vai estar configurado
        private static char UNDERSCORE_CHAR_SPLIT = '_';
        private static char DOT_CHAR_SPLIT = '.';

        private static int TOTAL_FILE_COLUMNS = 49;
        private static int EXPIRACAO_TIMESTAMP = 1;

        private static string EXTENSAO_ARQUIVOS = "*.csv";
        private static string DATE_TIME_FORMAT = "yyyyMMddHHmm";
        private static string PATH_RAIZ = ConfigurationManager.AppSettings[Constantes.APP_CONFIG_PASTA_RAIZ];
        private static string PATH_AGUARDANDO = PATH_RAIZ + Constantes.PASTA_AGUARDANDO;
        private static string PATH_SUCESSO = PATH_RAIZ + Constantes.PASTA_SUCESSO;
        private static string PATH_FALHA = PATH_RAIZ + Constantes.PASTA_FALHA;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="dao"></param>
        public ImportacaoManagerImpl(IGenericDAO dao)
        {
            this._dao = dao;

            if (!Directory.Exists(PATH_RAIZ)) Directory.CreateDirectory(PATH_RAIZ);
            if (!Directory.Exists(PATH_AGUARDANDO)) Directory.CreateDirectory(PATH_AGUARDANDO);
            if (!Directory.Exists(PATH_SUCESSO)) Directory.CreateDirectory(PATH_SUCESSO);
            if (!Directory.Exists(PATH_FALHA)) Directory.CreateDirectory(PATH_FALHA);
        }

        /// <summary>
        /// Retorna a lista de importaç~oes que foram adicionadas 
        /// para ser processada ou que ja foram processadas
        /// </summary>
        /// <returns></returns>
        public DataTableData GetImportacoes(int pageSize, int currentPage, int idUsuario)
        {
            return this._dao.FindDataTableByFilter<Importacao>(i => i.IdImportacao != -1 && i.Analista.Id_usuario == idUsuario, pageSize, currentPage);
        }

        /// <summary>
        /// Retorna uma lista com nomes dos arquivos encontrados na pasta raiz
        /// </summary>
        /// <returns></returns>
        public IList<string> GetArquivosImportacao()
        {
            IList<string> lista = new List<string>();

            string[] files = Directory.GetFiles(PATH_RAIZ, EXTENSAO_ARQUIVOS);

            foreach (string file in files)
            {
                lista.Add(Path.GetFileName(file));
            }

            return lista;
        }

        /// <summary>
        /// Adiciona e executa a importação dos dados
        /// </summary>
        /// <param name="importacao"></param>
        public void AdicionaExecutaImportacao(Importacao importacao)
        {
            AdicionaParaImportacao(importacao, true);
        }

        /// <summary>
        /// Executa a importacao de todos os arquivos que estao na pasta raiz, 
        /// ou que ja estejam no banco com status aguardando
        /// </summary>
        public void ImportaArquivos()
        {
            AdicionaArquivosRestantesDaRaizParaImportacao();

            IList<Importacao> importacoes = this._dao.FindByFilter<Importacao>
                (i => i.Status.Equals(EnumStatusImportacao.AGUARDANDO));

            foreach (var item in importacoes)
            {
                ImportaArquivo(item);
            }
        }

        /// <summary>
        /// Adiciona os arquivos da pasta raiz para o banco com status aguardando 
        /// e caso timestamp esteja invalido o status sera de timestamp expirado
        /// </summary>
        private void AdicionaArquivosRestantesDaRaizParaImportacao()
        {
            Usuario sistema = this._dao.FindByFilter<Usuario>
                (u => u.Dsc_nome.Equals(Constantes.USUARIO_SISTEMA_BATCH)).First();

            IList<string> arquivosNaRaiz = GetArquivosImportacao();

            Importacao importacao;
            foreach (string nomeArquivo in arquivosNaRaiz)
            {
                importacao = new Importacao();
                importacao.Arquivo = nomeArquivo;
                importacao.Analista = sistema;

                try
                {
                    AdicionaSemExecutarImportacao(importacao);
                }
                catch (Exception ex)
                {
                    StringBuilder message = new StringBuilder();
                    message.AppendLine(String.Format(Resources.mensagem_erro_indexar_arquivo, importacao.Arquivo));
                    message.AppendLine(String.Empty);
                    message.AppendLine(ex.Message);

                    string currentClass = this.GetType().ToString();
                    string method = "AdicionaSemExecutarImportacao";

                    EventViewerHelper.LogBusinessFailure(message.ToString(), currentClass, method);
                }
            }
        }

        /// <summary>
        /// Adiciona e nao executa a importação dos dados
        /// </summary>
        /// <param name="importacao"></param>
        private void AdicionaSemExecutarImportacao(Importacao importacao)
        {
            AdicionaParaImportacao(importacao, false);
        }

        /// <summary>
        /// Adiciona e/ou executa a importacao de um arquivo no sistema
        /// valida o timestamp
        /// valida o layout
        /// </summary>
        /// <param name="importacao"></param>
        /// <param name="executaImportacao"></param>
        private void AdicionaParaImportacao(Importacao importacao, bool executaImportacao)
        {
            if (ValidaTimeStamp(importacao))
            {
                try
                {
                    if (VerificaLayoutArquivo(importacao))
                    {
                        AdicionaImportacaoEMoveArquivo(importacao);

                        if (executaImportacao)
                        {
                            ExecutaImportacao(importacao);
                        }
                    }
                    else
                    {
                        throw new LayoutArquivoInvalidoException();
                    }
                }
                catch (FileNotFoundException)
                {
                    Importacao importacaoDoBanco = this._dao.FindByFilter<Importacao>
                        (i => i.Arquivo.Equals(importacao.Arquivo)).First();
                    if (importacaoDoBanco != null)
                    {
                        throw new ImportacaoIndexadaPeloSistemaException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new TimeStampInvalidoException();
            }
        }

        /// <summary>
        /// Inicia uma thread para executar a importacao de um arquivo
        /// </summary>
        /// <param name="importacao"></param>
        private void ExecutaImportacao(Importacao importacao)
        {
            Thread thread = new Thread(() => ImportaArquivo(importacao));
            thread.Start();
        }

        /// <summary>
        /// Adiciona uma importacao com status aguardando e move o arquivo para a pasta de aguardando
        /// </summary>
        /// <param name="importacao"></param>
        private void AdicionaImportacaoEMoveArquivo(Importacao importacao)
        {
            importacao.DataAdicao = DateTime.Now;

            importacao.Status = EnumStatusImportacao.AGUARDANDO.ToString();

            FileUtil.MoverArquivo(importacao.Arquivo, PATH_RAIZ, PATH_AGUARDANDO);

            this._dao.Incluir(importacao);
        }

        /// <summary>
        /// Valida o timestamp dos arquivos
        /// </summary>
        /// <param name="importacao"></param>
        /// <returns></returns>
        private bool ValidaTimeStamp(Importacao importacao)
        {
            bool retorno = true;

            try
            {
                //Pega o TimeStamp no moe do arquivo, hora de geracao do arquivo
                string[] nomeArquivoSplit = importacao.Arquivo.Split(UNDERSCORE_CHAR_SPLIT);

                string dataArquivo = nomeArquivoSplit.Last().Split(DOT_CHAR_SPLIT).First();

                DateTime timeStampImport = DateTime.ParseExact(dataArquivo, DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                //subtrai a hora atual com a hora do arquivo gerado
                TimeSpan validaHoraImport = DateTime.Now.Subtract(timeStampImport);

                //Arquivo é váilido se tempo for menor que uma hora da criaçao
                if (validaHoraImport.Hours >= EXPIRACAO_TIMESTAMP)
                {
                    retorno = false;
                }
            }
            catch (FormatException)
            {
                throw new NomeclaturaInvalidaException();
            }
            return retorno;
        }

        /// <summary>
        /// Verifica o layout do arquivo
        /// 
        /// "se possui o número de colunas esperado"
        /// </summary>
        /// <param name="importacao"></param>
        /// <returns></returns>
        private bool VerificaLayoutArquivo(Importacao importacao)
        {
            string localizacaoArquivo = Path.Combine(PATH_RAIZ, importacao.Arquivo);

            var reader = new StreamReader(File.OpenRead(localizacaoArquivo));

            var line = reader.ReadLine();

            reader.Dispose();

            return line.Split(Constantes.CSV_CHAR_SPLIT).Count() == TOTAL_FILE_COLUMNS;
        }

        /// <summary>
        /// Responsável pela execução da importação, abre o arquivo, le as linhas
        /// e no final determina se teve sucesso ou alguma falha
        /// </summary>
        /// <param name="importacao"></param>
        private void ImportaArquivo(Importacao importacao)
        {
            importacao.DataImportacao = DateTime.Now;

            string localizacaoArquivo = Path.Combine(PATH_AGUARDANDO, importacao.Arquivo);
            //verificar se o arquivo esta na pasta arquivo
            if (File.Exists(localizacaoArquivo))
            {
                importacao.Status = EnumStatusImportacao.EM_EXECUCAO.ToString();
                this._dao.Alterar(importacao);

                bool sucesso = LeArquivo(importacao, localizacaoArquivo);

                string destino;
                if (sucesso)
                {
                    importacao.Status = EnumStatusImportacao.SUCESSO.ToString();
                    destino = PATH_SUCESSO;
                }
                else
                {
                    importacao.Status = EnumStatusImportacao.FALHA.ToString();
                    destino = PATH_FALHA;
                }
                FileUtil.MoverArquivo(importacao.Arquivo, PATH_AGUARDANDO, destino);
            }
            else
            {
                importacao.Status = EnumStatusImportacao.ARQUIVO_REMOVIDO.ToString();
            }

            this._dao.Alterar(importacao);
        }

        /// <summary>
        /// Executa a leitura do arquvio
        /// </summary>
        /// <param name="importacao"></param>
        /// <param name="localizacaoArquivo"></param>
        /// <returns></returns>
        private bool LeArquivo(Importacao importacao, string localizacaoArquivo)
        {
            bool sucesso;
            int count = 1;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(File.OpenRead(localizacaoArquivo));
                string cabecalho = reader.ReadLine();
                string[] linhaSplit;
                while (!reader.EndOfStream)
                {
                    try
                    {
                        linhaSplit = reader.ReadLine().Split(Constantes.CSV_CHAR_SPLIT);
                        count++;
                        ImportaLinha(importacao, linhaSplit);
                    }
                    catch (LinhaArquivoInvalidoException ex)
                    {
                        StringBuilder message = new StringBuilder();
                        message.AppendLine(String.Format(Resources.mensagem_erro_linha_arquivo, count, importacao.Arquivo));
                        message.AppendLine(String.Empty);
                        message.AppendLine(ex.Message);
                        string currentClass = this.GetType().ToString();
                        string currentMethod = "ImportaLinha";

                        EventViewerHelper.LogBusinessFailure(message.ToString(), currentClass, currentMethod);
                    }
                }
                sucesso = true;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(String.Format(Resources.mensagem_erro_inseperado_leitura_arquivo, importacao.Arquivo, count));
                message.AppendLine(String.Empty);
                message.AppendLine(ex.ToString());
                string currentClass = this.GetType().ToString();
                string currentMethod = "ImportaLinha";

                EventViewerHelper.LogBusinessFailure(message.ToString(), currentClass, currentMethod);
                sucesso = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return sucesso;
        }

        /// <summary>
        /// Importa uma linha do arquivo
        /// </summary>
        /// <param name="importacao">Objeto de histórico e estatística da importação</param>
        /// <param name="dados">Registro com os dados de Ticket, Nota, Mobilidade e Pendências</param>
        public void ImportaLinha(Importacao importacao, string[] dados)
        {
            if (dados[0] == EnumHelper.GetEnumDescription(EnumGrupoAtendimento.ID01))
                ImportaGrupo1(importacao, dados);
            else
                ImportaGrupo2e3(importacao, dados);
        }

        private void ImportaGrupo1(Importacao importacao, string[] dados)
        {
            Int32 parseInt;
            Int64 parseLong;
            Nota nota = null;
            Int64 numeroNota;
            DateTime parseData;
            Int64 numeroTicket;
            Decimal parseDecimal;
            Boolean update = false;
            StringBuilder erroMessage = new StringBuilder();
            StringBuilder alertaMessage = new StringBuilder();

            if (!Int64.TryParse(dados[(int)EnumIndiceTicket.NUMERO_TICKET], out numeroTicket))
            {
                erroMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndiceTicket.NUMERO_TICKET, EnumIndiceTicket.NUMERO_TICKET.ToString()));
            }

            if (!Int64.TryParse(dados[(int)EnumIndiceNota.NUMERO_NOTA], out numeroNota))
            {
                erroMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndiceNota.NUMERO_NOTA, EnumIndiceNota.NUMERO_NOTA.ToString()));
            }

            Ticket ticket = _dao.FindByFilter<Ticket>(t => t.NumeroTicket == numeroTicket).FirstOrDefault();

            if (ticket != null)
            {
                update = true;
                nota = ticket.Notas.FirstOrDefault(n => n.NumeroNota == numeroNota);
            }
            else
            {
                ticket = new Ticket();
            }

            if (nota == null)
            {
                ticket.Notas.Add(new Nota());
                nota = ticket.Notas[ticket.Notas.Count - 1];
                nota.Mobilidade = new Mobilidade(nota);
            }
            else
            {
                nota.Mobilidade = _dao.FindByFilter<Mobilidade>(m => m.Nota.CodigoNota == nota.CodigoNota).FirstOrDefault();
            }

            //Dados do Ticket
            ticket.NumeroTicket = numeroTicket;
            ticket.Atividade = dados[(int)EnumIndiceTicket.ATIVIDADE];
            ticket.TbDemanda = ImportacaoHelper.IdentificaDemanda(dados[(int)EnumIndiceTicket.DEMANDA], _dao);
            ticket.TbFluxoAtendimento = ImportacaoHelper.IdentificaFluxoAtendimento(dados[(int)EnumIndiceTicket.FLUXO_ATENDIMENTO], _dao);
            ticket.TbStatusTicket = new TbStatusTicket() { CodigoStatusTicket = (int)EnumStatusTicket.ABERTO, Descricao = EnumHelper.GetEnumDescription(EnumStatusTicket.ABERTO) };

            //Valor fixo pois o campo Tipo solicitação não veio na atualização do arquivo inbox.
            ticket.TbTipoSolicitacao = _dao.FindByFilter<TbTipoSolicitacao>(t => t.CodigoTipoSolicitacao == (int)EnumTipoSolicitacao.OUTROS).FirstOrDefault();

            if (ticket.TbDemanda == null)
            {
                erroMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndiceTicket.DEMANDA, EnumIndiceTicket.DEMANDA.ToString()));
            }

            if (DateTime.TryParse(dados[(int)EnumIndiceTicket.DATA_CRIACAO], out parseData))
            {
                ticket.DataCriacao = parseData;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndiceTicket.DATA_CRIACAO, EnumIndiceTicket.DATA_CRIACAO.ToString()));
            }

            if (DateTime.TryParse(dados[(int)EnumIndiceTicket.DATA_ENCERRAMENTO], out parseData))
            {
                ticket.DataEncerramento = parseData;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndiceTicket.DATA_ENCERRAMENTO, EnumIndiceTicket.DATA_ENCERRAMENTO.ToString()));
            }

            //Dados da Nota
            nota.NumeroNota = numeroNota;
            nota.Mensagem = dados[(int)EnumIndiceNota.MENSAGENS];
            nota.TipoNota = dados[(int)EnumIndiceNota.TIPO_NOTA];
            nota.Observacao = dados[(int)EnumIndiceNota.OBSERVACOES];
            nota.PendenciaNs = dados[(int)EnumIndiceNota.PENDENCIA_NS];
            nota.StatusNotaSap = dados[(int)EnumIndiceNota.STATUS_NOTA_SAP];
            nota.StatusNotaUsuario = dados[(int)EnumIndiceNota.STATUS_NOTA_USUARIO];
            nota.TbGrupo = ImportacaoHelper.IdentificaGrupo(EnumGrupoAtendimento.ID01, _dao);
            nota.TbArea = ImportacaoHelper.IdentificaArea(dados[(int)EnumIndiceNota.AREA_NOTA], string.Empty, _dao);
            nota.TbStatusNota = ImportacaoHelper.IdentificaStatusNota(dados[(int)EnumIndiceNota.STATUS_NOTA], _dao);

            if (Int64.TryParse(dados[(int)EnumIndiceNota.ID_INSTALACAO_NOTA], out parseLong))
            {
                nota.IdInstalacao = parseLong;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndiceNota.ID_INSTALACAO_NOTA, EnumIndiceNota.ID_INSTALACAO_NOTA.ToString()));
            }

            //dados de nota que estão definidos no intervalo de dados de consulta de mobilidade
            nota.Bairro = dados[(int)EnumIndiceMobilidade.BAIRRO];
            nota.Cidade = dados[(int)EnumIndiceMobilidade.CIDADE];
            nota.Endereco = dados[(int)EnumIndiceMobilidade.ENDERECO_MOBILIDADE];

            //Dados de consulta da mobilidade
            nota.Mobilidade.Viatura = dados[(int)EnumIndiceMobilidade.VIATURA];
            nota.Mobilidade.StatusOs = dados[(int)EnumIndiceMobilidade.STATUS_OS];
            nota.Mobilidade.NomeGasista = dados[(int)EnumIndiceMobilidade.NOME_GASISTA];
            nota.Mobilidade.SubCategoriaOs = dados[(int)EnumIndiceMobilidade.SUBCATEGORIA_OS];
            nota.Mobilidade.RegistroGasista = dados[(int)EnumIndiceMobilidade.REGISTRO_GASISTA];
            nota.Mobilidade.DescricaoMaterial = dados[(int)EnumIndiceMobilidade.DESCRICAO_MATERIAL];

            if (Int32.TryParse(dados[(int)EnumIndiceMobilidade.QUANTIDADE], out parseInt))
            {
                nota.Mobilidade.Quantidade = parseInt;
            }

            if (Int32.TryParse(dados[(int)EnumIndiceMobilidade.ZONA_ATENDIMENTO], out parseInt))
            {
                nota.Mobilidade.ZonaAtendimento = parseInt;
            }

            if (Decimal.TryParse(dados[(int)EnumIndiceMobilidade.VALOR], out parseDecimal))
            {
                nota.Mobilidade.Valor = parseDecimal;
            }

            if (!ValidaTicketEnviadoParaLEgado(ticket))
            {
                erroMessage.AppendLine(String.Format(Resources.mensagem_erro_ticket_em_atendimento, ticket.NumeroTicket));
            }

            DistribuiItensParaAtendimento(EnumGrupoAtendimento.ID01, ticket, update);

            AtualizaImportacaoESalvaLinha(importacao, alertaMessage, erroMessage, ticket, update);
        }

        private void ImportaGrupo2e3(Importacao importacao, string[] dados)
        {
            Int32 parseInt;
            Nota nota = null;
            Int64 numeroNota;
            DateTime parseData;
            Boolean update = false;
            EnumGrupoAtendimento grupo;
            StringBuilder erroMessage = new StringBuilder();
            StringBuilder alertaMessage = new StringBuilder();

            if (!Int64.TryParse(dados[(int)EnumIndicePendenciaNota.NUMERO_NOTA_PENDENCIA], out numeroNota))
            {
                erroMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndicePendenciaNota.NUMERO_NOTA_PENDENCIA, EnumIndicePendenciaNota.NUMERO_NOTA_PENDENCIA.ToString()));
            }

            nota = _dao.FindByFilter<Nota>(n => n.NumeroNota == numeroNota).FirstOrDefault();

            if (nota == null)
            {
                nota = new Nota();
            }
            else
            {
                update = true;
            }

            if (nota.PendenciaNota == null)
            {
                nota.PendenciaNota = new PendenciaNota(nota);
            }

            //Dados de nota que vem no intervalo de pendencia A104/Mobilidade
            nota.NumeroNota = numeroNota;
            nota.Cidade = dados[(int)EnumIndicePendenciaNota.CIDADE_PENDENCIA];
            nota.Endereco = dados[(int)EnumIndicePendenciaNota.ENDERECO_PENDENCIA];
            nota.TipoNota = dados[(int)EnumIndicePendenciaNota.TIPO_NOTA_PENDENCIA];
            nota.StatusNotaUsuario = dados[(int)EnumIndicePendenciaNota.STATUS_NOTA_USUARIO_PENDENCIA];
            nota.TbArea = ImportacaoHelper.IdentificaArea(dados[(int)EnumIndicePendenciaNota.CODIGO_AREA], dados[(int)EnumIndicePendenciaNota.DESCRICAO_AREA], _dao);

            if (dados[0] == EnumHelper.GetEnumDescription(EnumGrupoAtendimento.ID02))
            {
                grupo = EnumGrupoAtendimento.ID02;
                nota.TbGrupo = ImportacaoHelper.IdentificaGrupo(EnumGrupoAtendimento.ID02, _dao);
            }
            else
            {
                grupo = EnumGrupoAtendimento.ID03;
                nota.TbGrupo = ImportacaoHelper.IdentificaGrupo(EnumGrupoAtendimento.ID03, _dao);
            }

            nota.TbStatusNota = _dao.FindByFilter<TbStatusNota>(s => s.CodigoGrupo == (int)grupo && s.Descricao == EnumHelper.GetEnumDescription(EnumStatusNota.ABERTO)).FirstOrDefault();

            if (DateTime.TryParse(dados[(int)EnumIndicePendenciaNota.INSTALACAO_PENDENCIA], out parseData))
            {
                nota.DataInstalacao = parseData;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndicePendenciaNota.INSTALACAO_PENDENCIA, EnumIndicePendenciaNota.INSTALACAO_PENDENCIA.ToString()));
            }

            //Dados de Pendência de Mobilidade/A104
            nota.PendenciaNota.Descricao = dados[(int)EnumIndicePendenciaNota.DESCRICAO];
            nota.PendenciaNota.MensagemErro = dados[(int)EnumIndicePendenciaNota.MENSAGEM_ERRO];
            nota.PendenciaNota.CentrabRespon = dados[(int)EnumIndicePendenciaNota.CENTRAB_RESPON];
            nota.PendenciaNota.CodigoPendencia = dados[(int)EnumIndicePendenciaNota.CODIGO_PENDENCIA];
            nota.PendenciaNota.SegmentoCliente = dados[(int)EnumIndicePendenciaNota.SEGMENTO_CLIENTE];
            nota.PendenciaNota.TextoCodeCodificacao = dados[(int)EnumIndicePendenciaNota.TEXTO_CODIFICACAO];
            nota.PendenciaNota.DescricaoPendencia = dados[(int)EnumIndicePendenciaNota.DESCRICAO_PENDENCIA];

            if (DateTime.TryParse(dados[(int)EnumIndicePendenciaNota.INICIO_DESEJADO], out parseData))
            {
                nota.PendenciaNota.inicioDesejado = parseData;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndicePendenciaNota.INICIO_DESEJADO, EnumIndicePendenciaNota.INICIO_DESEJADO.ToString()));
            }

            if (DateTime.TryParse(dados[(int)EnumIndicePendenciaNota.DATA_CRIACAO_A104], out parseData))
            {
                nota.PendenciaNota.DataCriacaoA104 = parseData;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndicePendenciaNota.DATA_CRIACAO_A104, EnumIndicePendenciaNota.DATA_CRIACAO_A104.ToString()));
            }

            if (DateTime.TryParse(dados[(int)EnumIndicePendenciaNota.DATA_ENCERRAMENTO_SAP], out parseData))
            {
                nota.PendenciaNota.DataEncerramentoSAP = parseData;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndicePendenciaNota.DATA_ENCERRAMENTO_SAP, EnumIndicePendenciaNota.DATA_ENCERRAMENTO_SAP.ToString()));
            }

            if (Int32.TryParse(dados[(int)EnumIndicePendenciaNota.NUMERO_APARTAMENTO], out parseInt))
            {
                nota.PendenciaNota.NumeroApartamento = parseInt;
            }
            else
            {
                alertaMessage.AppendLine(String.Format(Resources.mensagem_erro_coluna_linha, (int)EnumIndicePendenciaNota.NUMERO_APARTAMENTO, EnumIndicePendenciaNota.NUMERO_APARTAMENTO.ToString()));
            }

            DistribuiItensParaAtendimento(grupo, nota, update);

            if (nota.StatusNotaUsuario == null)
            {
                erroMessage.AppendLine("Nenhum usuário encontrado no grupo para distribuição.");
            }

            AtualizaImportacaoESalvaLinha(importacao, alertaMessage, erroMessage, nota, update);
        }

        private void AtualizaImportacaoESalvaLinha(Importacao importacao, StringBuilder alertaMessage, StringBuilder erroMessage, object linha, Boolean update)
        {
            if (erroMessage.Length > 0)
            {
                importacao.QtdRegistroFalha += 1;
                throw new LinhaArquivoInvalidoException(erroMessage.ToString());
            }

            if (update)
            {
                _dao.Alterar(linha);
                importacao.QtdRegistroAtualizado += 1;
            }
            else
            {
                _dao.Incluir(linha);
                importacao.QtdRegistroNovo += 1;
            }

            if (alertaMessage.Length > 0)
            {
                EventViewerHelper.LogBusinessWarning(alertaMessage.ToString(), this.GetType().Name, "ImportaLinha");
            }
        }

        public void DistribuiItensParaAtendimento(EnumGrupoAtendimento grupo, Object itemAtendimento, Boolean update)
        {
            Usuario usuario = null;
            Object itensDesignados;

            if (!update)
            {
                if (grupo == EnumGrupoAtendimento.ID01)
                {
                    itensDesignados = _dao.FindByFilter<Ticket>(t => t.TbStatusTicket.CodigoStatusTicket != (int)EnumStatusTicket.ENCERRADO).ToList();
                }
                else
                {
                    List<Nota> notasDesignadas = _dao.FindByFilter<Nota>(n => n.TbGrupo.CodigoGrupo == (int)grupo).ToList();

                    itensDesignados = notasDesignadas.Where(n => (n.TbStatusNota.Descricao == EnumHelper.GetEnumDescription(EnumStatusNota.ABERTO)
                                                              || n.TbStatusNota.Descricao == EnumHelper.GetEnumDescription(EnumStatusNota.EM_ANDAMENTO))).ToList();
                }

                //pega o usuario do grupo com menos itens em sua responsabilidade
                usuario = CalculaQuantidadesItensUsuario(itensDesignados, grupo);
            }

            if (itemAtendimento as Ticket != null)
            {
                if (usuario == null)
                {
                    usuario = ((Ticket)itemAtendimento).Notas[0].TbUsuario;
                }

                ((Ticket)itemAtendimento).Notas.ToList().ForEach(n => n.TbUsuario = n.TbUsuario ?? usuario);
            }
            else
            {
                if (usuario != null)
                {
                    ((Nota)itemAtendimento).TbUsuario = usuario;
                }
            }
        }

        private Usuario CalculaQuantidadesItensUsuario(Object itensDesignados, EnumGrupoAtendimento grupo)
        {
            Usuario usuario = null;
            List<Usuario> usuarios;

            Dictionary<Usuario, int> contagemItem = new Dictionary<Usuario, int>();
            Grupo grupoAtendimento = _dao.FindByFilter<Grupo>(g => g.Codigo == (int)grupo).First();

            usuarios = _dao.FindAll<Usuario>().ToList().Where(u => u.Grupos.Any(g => g.Codigo == grupoAtendimento.Codigo)).ToList();

            dynamic listaItens;

            if (itensDesignados as List<Ticket> != null)
            {
                listaItens = (List<Ticket>)itensDesignados;

                foreach (Ticket ticket in listaItens)
                {
                    int contagemAtual;

                    if (ticket.Notas[0].TbUsuario != null)
                    {
                        usuario = usuarios.Find(u => u.Id_usuario == ticket.Notas[0].TbUsuario.Id_usuario);

                        if (!contagemItem.ContainsKey(usuario))
                        {
                            contagemItem.Add(usuario, 1);
                        }
                        else
                        {
                            contagemItem.TryGetValue(usuario, out contagemAtual);
                            contagemItem[usuario] = contagemAtual + 1;
                        }
                    }
                }
            }
            else
            {
                listaItens = (List<Nota>)itensDesignados;

                foreach (Nota nota in listaItens)
                {
                    int contagemAtual;

                    if (nota.TbUsuario != null)
                    {
                        usuario = usuarios.Find(u => u.Id_usuario == nota.TbUsuario.Id_usuario);

                        if (!contagemItem.ContainsKey(usuario))
                        {
                            contagemItem.Add(usuario, 1);
                        }
                        else
                        {
                            contagemItem.TryGetValue(usuario, out contagemAtual);
                            contagemItem[usuario] = contagemAtual + 1;
                        }
                    }
                }
            }

            if (usuarios.Count > contagemItem.Keys.Count)
            {
                foreach (Usuario usuarioAtual in usuarios)
                {
                    if (!contagemItem.Keys.Contains(usuarioAtual))
                    {
                        usuario = usuarioAtual;
                        break;
                    }
                }
            }
            else
            {
                if (contagemItem.Count > 0)
                {
                    usuario = contagemItem.OrderByDescending(c => c.Value).First().Key;
                }
            }

            return usuario;
        }

        private bool ValidaTicketEnviadoParaLEgado(Ticket ticket)
        {
            return !(ticket.RetornoLegado == null && (ticket.DataSalvo != null || ticket.EnvioLegado != null));
        }
    }
}