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
    public interface INotaManager : IBaseManager<Nota>
    {
        /// <summary>
        ///     Método que verifica se o id da nota existe na base
        /// </summary>
        /// <param name="id">Id do nota</param>
        /// <returns>
        ///     True - Se o id existir
        ///     False - Se o id não existir
        /// </returns>
        bool VerificaSeIdExisteNaBase(int id);

        /// <summary>
        ///     Método que retorna a nota com o id passado por parâmetro.
        /// </summary>
        /// <param name="id">Id da nota</param>
        /// <returns>
        ///     Nota com o id passado por parâmetro
        /// </returns>
        Nota GetNotaPeloId(int id);

        /// <summary>
        ///     Método que busca uma nota no banco usando o 'numero_nota'.
        /// </summary>
        /// <param name="numero_nota">
        ///     Int para busca da nota no banco.
        /// </param>
        /// <returns>
        ///     Se encontrar, retorna o primeira 'Nota' encontrado.
        ///     Caso contrário, retorna 'null'.
        /// </returns>
        Nota BuscaNotaPorNumero(int numero_nota);

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
        ICriteria MontaCriteriasValidos(FiltroNota filtro);

        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroNota filtro);

        //IList<Nota> FindAll();

        void InsereNotaNaBase(Nota nota);

        void SaveOrUpdate(Nota nota);

        void Delete(Nota p);

        Nota FindByPk(int nota);

        IList<Nota> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter);
    }
}
