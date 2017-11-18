using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using SGCA.Models.Enums;
using SGCA.Models.DAO;
using SGCA.Models.Entity;

namespace SGCA.Models.Helpers
{
    public class ImportacaoHelper
    {
        public static Area IdentificaArea(string area, string descricaoArea, IGenericDAO dao) 
        {
            string[] areaIdentificaor = area.Split('_');

            if (areaIdentificaor.Length > 1)
            {
                area = areaIdentificaor[0].ToUpper().Trim();
                descricaoArea = areaIdentificaor[1].Trim();
            }
            else
            { 
                area = area.ToUpper().Trim();
                descricaoArea = descricaoArea.Trim();
            }

            Area areaNota = dao.FindAll<Area>().FirstOrDefault(a => a.IdentificacaoArea.ToUpper() == area);

            if (areaNota == null)
            {
                int codigo = (int)dao.Incluir<Area>(new Area() { IdentificacaoArea = area, DescricaoArea = descricaoArea });

                areaNota = dao.FindByPK<Area>(codigo);
            }

            return areaNota;
        }

        public static Demanda IdentificaDemanda(string demanda, IGenericDAO dao)
        {
            Demanda objDemanda = dao.FindAll<Demanda>().FirstOrDefault(d => d.DescricaoDemanda.ToUpper() == demanda.ToUpper().Trim());

            return objDemanda;
        }

        public static TbStatusNota IdentificaStatusNota(string status, IGenericDAO dao)
        {
            status = status.ToUpper().Trim();

            switch (status)
            {
                case "COMPLETED": status = "FINALIZADO"; break;
                case "INTERRUPTED": status = "CANCELADO"; break;
                default: break;
            }

            TbStatusNota statusNota = dao.FindAll<TbStatusNota>().FirstOrDefault(s => s.Descricao.ToUpper() == status);

            if (statusNota == null)
            {
                statusNota = dao.FindAll<TbStatusNota>().FirstOrDefault(s => s.Descricao.ToUpper() == "ABERTO");
            }

            return statusNota;
        }

        public static FluxoAtendimento IdentificaFluxoAtendimento(string fluxo, IGenericDAO dao)
        {
            FluxoAtendimento fluxoDemanda = dao.FindAll<FluxoAtendimento>().FirstOrDefault(f => f.DescricaoFluxoAtendimento.ToUpper() == fluxo.ToUpper().Trim());

            if (fluxoDemanda == null)
            {
                int codigo = (int)dao.Incluir<FluxoAtendimento>(new FluxoAtendimento() { DescricaoFluxoAtendimento = fluxo });

                fluxoDemanda = dao.FindByPK<FluxoAtendimento>(codigo);
            }

            return fluxoDemanda;
        }

        public static TbGrupo IdentificaGrupo(EnumGrupoAtendimento grupo, IGenericDAO dao)
        {
            return dao.FindAll<TbGrupo>().FirstOrDefault(g => g.CodigoGrupo == (int)grupo);
        }
    }
}