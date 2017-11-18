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
using NetUtil.Util.Enums;
using SGCA.Models.Filters;
using NHibernate;
using NetUtil.Util.Hibernate;
using NHibernate.Transform;

namespace SGCA.Models.Manager.Impl
{
    public class AtendimentoPendenciasManagerImpl : BaseManagerImpl<AtendimentoPendencias>, IAtendimentoPendenciasManager
    {

        private static string EXTENSAO_ARQUIVOS = "*.xlsx";
        
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="dao"></param>
        public AtendimentoPendenciasManagerImpl(IGenericDAO dao)
        {
            this._dao = dao;
        }

        /// <summary>
        /// Retorna a lista de pendências que foram adicionadas 
        /// para ser processada ou que ja foram processadas
        /// </summary>
        /// <returns></returns>
        public List<AtendimentoPendencias> GetAtendimentoPendencias()
        {
            //return this._dao.FindAll<AtendimentoPendencias>;

            //ISession session = HibernateUtil.GetCurrentSession();

            //string sql = @"SELECT * from [sgca].[dbo].[ViewMobilidadePendencias] ";
                            
            //sql += this.MontaWhereConsultaRelatorioCredito(filtro);

            //ISQLQuery query = session.CreateSQLQuery(sql);

            //query.SetResultTransformer(Transformers.AliasToBean(typeof(AtendimentoPendencias)));

            //this.SetQueryParameters(ref query, filtro);

            //List<AtendimentoPendencias> results = query.List<AtendimentoPendencias>().ToList(); 


            IList<AtendimentoPendencias> results = this._dao.FindAll<AtendimentoPendencias>();

            return results.ToList();

        }

        /// <summary>
        /// Retorna uma lista com nomes dos arquivos encontrados na pasta raiz
        /// </summary>
        /// <returns></returns>
        public IList<string> GetArquivosAtendimentoPendencias()
        {
            IList<string> lista = new List<string>();

            string[] files = Directory.GetFiles(EXTENSAO_ARQUIVOS);

            foreach (string file in files)
            {
                lista.Add(Path.GetFileName(file));
            }

            return lista;
        }


        public AtendimentoPendencias GetPorNumeroNota(int numeroNota)
        {
            //return new AtendimentoPendencias();
            return _dao.FindByFilter<AtendimentoPendencias>(a => a.NumeroNota == numeroNota).FirstOrDefault();
        }


        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroAtendimentoPendencias filtro)
        {
            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, object> fieldsFilter = new Dictionary<string, object>();

            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction = new Dictionary<string, IDictionary<Restriction, object>>();

            if (filtro.CodigoNota != 0)
            {
                IDictionary<Restriction, object> filtroCodigoNota = new Dictionary<Restriction, object>();
                filtroCodigoNota.Add(Restriction.Eq, filtro.CodigoNota);
                fieldsFilterWithRestriction.Add("CodigoNota", filtroCodigoNota);
            }

            if (filtro.NumeroNota != 0)
            {
                IDictionary<Restriction, object> filtroNumNota = new Dictionary<Restriction, object>();
                filtroNumNota.Add(Restriction.LikeRight, filtro.NumeroNota);
                fieldsFilterWithRestriction.Add("NumeroNota", filtroNumNota);
            }

            return fieldsFilterWithRestriction;
        }

        public void InserePendenciaNaBase(AtendimentoPendencias atendimentoPendencias)
        {
            _dao.Incluir<AtendimentoPendencias>(atendimentoPendencias);
        }


        public void SaveOrUpdate(AtendimentoPendencias atendimentoPendencias)
        {
            _dao.SaveOrUpdate<AtendimentoPendencias>(atendimentoPendencias);
        }


        public void Delete(AtendimentoPendencias atendimentoPendencias)
        {
            AtendimentoPendencias ap = new AtendimentoPendencias();
            ap = _dao.FindByPK<AtendimentoPendencias>(atendimentoPendencias.CodigoNota);
            _dao.Excluir<AtendimentoPendencias>(ap);
        }

        public AtendimentoPendencias FindByPk(int pk)
        {
            return _dao.FindByPK<AtendimentoPendencias>(pk);
        }

        public IList<AtendimentoPendencias> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter)
        {
            return _dao.FindByFilter<AtendimentoPendencias>(fieldsFilter, null);
        }
    }
}
