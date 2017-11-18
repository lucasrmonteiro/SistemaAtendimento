using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Ticket
    {
        #region Propriedades

        public virtual int CodigoTicket { get; set; }
        public virtual TbStatusTicket TbStatusTicket { get; set; }
        public virtual Demanda TbDemanda { get; set; }
        public virtual FluxoAtendimento TbFluxoAtendimento { get; set; }
        public virtual TbTipoSolicitacao TbTipoSolicitacao { get; set; }
        public virtual long? NumeroTicket { get; set; }
        public virtual string SlaCliente { get; set; }
        public virtual DateTime? DataCriacao { get; set; }
        public virtual DateTime? DataExtracao { get; set; }
        public virtual DateTime? DataEncerramento { get; set; }
        public virtual string TipoSolicitacao { get; set; }
        public virtual bool AtendimentoOutraArea { get; set; }
        public virtual bool? AtendimentoAutomatico { get; set; }
        public virtual string Atividade { get; set; }
        public virtual string Mensagens { get; set; }
        public virtual DateTime? DataSalvo { get; set; }
        public virtual DateTime? EnvioLegado { get; set; }
        public virtual DateTime? RetornoLegado { get; set; }
        public virtual DateTime? ContadorProcesso { get; set; }
        public virtual DateTime? ProcessoEncerrado { get; set; }
        public virtual string MensagemRetorno { get; set; }
        public virtual string StatusRetorno { get; set; }
        public virtual string JustificativaPriorizacao { get; set; }
        public virtual string Farol { get; set; }

        public virtual IList<Nota> Notas { get; set; }

        #endregion

        #region Construtor

        public Ticket()
        {
            Notas = new List<Nota>();
        }

        #endregion

        #region Metodos


        

        #endregion
    }
}