using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using SGCA.Models.Filters;
using NetUtil.Util.Enums;
using SGCA.Models.Entity;
using SGCA.Models.Manager.Base;
using NetUtil.Util.DTO;

namespace SGCA.Models.Manager
{
    public interface IAtendimentoPendenciasManager : IBaseManager<AtendimentoPendencias>
    {

        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroAtendimentoPendencias filtro);

        IList<string> GetArquivosAtendimentoPendencias();

        List<AtendimentoPendencias> GetAtendimentoPendencias();

        AtendimentoPendencias GetPorNumeroNota(int numeroNota);

        void InserePendenciaNaBase(AtendimentoPendencias atendimentoPendencias);

        void SaveOrUpdate(AtendimentoPendencias atendimentoPendencias);

        void Delete(AtendimentoPendencias ap);

        AtendimentoPendencias FindByPk(int atendimentoPendencias);

        IList<AtendimentoPendencias> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter);
    }
}
