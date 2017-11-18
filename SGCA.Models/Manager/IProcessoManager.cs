using SGCA.Models.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using SGCA.Models.Filters;
using NetUtil.Util.Enums;
using SGCA.Models.Manager.Base;

namespace SGCA.Models.Manager
{
    public interface IProcessoManager : IBaseManager<Processo>
    {
        /// <summary>
        ///     Método que verifica se o id do processo existe na base
        /// </summary>
        /// <param name="id">Id do processo</param>
        /// <returns>
        ///     True - Se o id existir
        ///     False - Se o id não existir
        /// </returns>
        bool VerificaSeIdExisteNaBase(int id);

        /// <summary>
        ///     Método que retorna o processo com o id passado por parâmetro.
        /// </summary>
        /// <param name="id">Id do processo</param>
        /// <returns>
        ///     Processo com o id passado por parâmetro
        /// </returns>
        Processo GetProcessoPeloId(int id);

        /// <summary>
        ///     Método que busca um processo no banco usando o 'numero_processo'.
        /// </summary>
        /// <param name="numero_processo">
        ///     Int para busca do processo no banco.
        /// </param>
        /// <returns>
        ///     Se encontrar, retorna o primeiro 'Processo' encontrado.
        ///     Caso contrário, retorna 'null'.
        /// </returns>
        Processo BuscaProcessoPorNumero(int numero_processo);

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
        ///     Sucesso: Objeto Processo.
        ///     Falha: null.
        /// </returns>
        //Processo AtualizaProcesso(DateTime dataExtracao, DateTime dataEncerramento);

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
        ICriteria MontaCriteriasValidos(FiltroProcesso filtro);

        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroProcesso filtro);

        //IList<Processo> FindAll();

        void InsereProcessoNaBase(Processo processo);

        void SaveOrUpdate(Processo processo);

        void Delete(Processo p);

        Processo FindByPk(int processo);

        IList<Processo> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter);

        bool ImportarLegado();
    }
}
