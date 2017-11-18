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

namespace SGCA.Models.Manager.Impl
{
    public class NotaManagerImpl : BaseManagerImpl<Nota>, INotaManager
    {
        #region Constantes
        private static string EXTENSAO_ARQUIVOS = "*.log";
        private static char CSV_CHAR_SPLIT = ';';

        private static string PATH_RAIZ = ConfigurationManager.AppSettings[Constantes.APP_CONFIG_PASTA_RAIZ];
        private static string PATH_SUCESSO = PATH_RAIZ + Constantes.PASTA_SUCESSO;
        private static string PATH_FALHA = PATH_RAIZ + Constantes.PASTA_FALHA;
        #endregion

        public NotaManagerImpl(IGenericDAO dao)
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
            return _dao.FindEntityByFilter<Nota>(new Dictionary<string, object>() { { "CodigoNota", id } }, null) == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Nota GetNotaPeloId(int id)
        {
            Convert.ToString(id);
            return _dao.FindEntityByFilter<Nota>(new Dictionary<string, object>() { { "CodigoNota", id } }, null);
        }

        /// <summary>
        ///     Método que busca uma nota no banco usando o 'numero_nota'.
        /// </summary>
        /// <param name="numero_nota">
        ///     Int para busca da nota no banco.
        /// </param>
        /// <returns>
        ///     Se encontrar, retorna a primeira 'Nota' encontrado.
        ///     Caso contrário, retorna 'null'.
        /// </returns>
        public Nota BuscaNotaPorNumero(int numero_nota)
        {
            //Cria um filtro de campos
            IDictionary<string, object> fieldsFilter = new Dictionary<string, object>();

            //Adiciona o campo 'NumeroNota' ao filtro
            fieldsFilter.Add("NumeroNota", numero_nota);

            //Busca usuários no banco, filtrando por 'NumeroNota'
            IList<Nota> notas = _dao.FindByFilter<Nota>(fieldsFilter, null);

            return notas.Count.Equals(0) ? null : notas[0];
        }



        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroNota filtro)
        {
            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, object> fieldsFilter = new Dictionary<string, object>();

            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction = new Dictionary<string, IDictionary<Restriction, object>>();

            if (filtro.CodigoNota != 0)
            {
                IDictionary<Restriction, object> filtroIdNota = new Dictionary<Restriction, object>();
                filtroIdNota.Add(Restriction.Eq, filtro.CodigoNota);
                fieldsFilterWithRestriction.Add("CodigoNota", filtroIdNota);
            }

            if (filtro.NumeroNota != 0)
            {
                IDictionary<Restriction, object> filtroNumNota = new Dictionary<Restriction, object>();
                filtroNumNota.Add(Restriction.LikeRight, filtro.NumeroNota);
                fieldsFilterWithRestriction.Add("NumeroNota", filtroNumNota);
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
        public ICriteria MontaCriteriasValidos(FiltroNota filtro)
        {
            ISession sessao = HibernateUtil.GetCurrentSession();
            ICriteria criteria = sessao.CreateCriteria<Nota>();

            bool noFilter = true;

            if (filtro.CodigoNota != 0)
            {
                // Adiciona restrição de "Id_nota"
                criteria.Add(Expression.Eq("CodigoNota",
                                            filtro.CodigoNota));
                noFilter = false;
            }

            if (noFilter)
            {
                criteria = null;
                HibernateUtil.CloseSession();
            }

            return criteria;
        }

        public void InsereNotaNaBase(Nota nota)
        {
            _dao.Incluir<Nota>(nota);
        }


        public void SaveOrUpdate(Nota nota)
        {
            _dao.SaveOrUpdate<Nota>(nota);
        }


        public void Delete(Nota nota)
        {
            Nota p = new Nota();
            p = _dao.FindByPK<Nota>(nota.CodigoNota);
            _dao.Excluir<Nota>(p);
        }

        public Nota FindByPk(int pk)
        {
            return _dao.FindByPK<Nota>(pk);
        }

        public IList<Nota> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter)
        {
            return _dao.FindByFilter<Nota>(fieldsFilter, null);
        }
            
    }
}
