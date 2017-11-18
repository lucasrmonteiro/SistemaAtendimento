using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Entity
{
    public class Atendimento
    {

        public Atendimento()
        {
            
        }

        public virtual int Id_Atendimento { get; set; }
        public virtual string Responsavel { get; set; }
        public virtual string SLA_Cliente { get; set; }
        public virtual string Aging { get; set; }
        public virtual string Atividade { get; set; }
        public virtual string Bandeira { get; set; }
        public virtual FluxoAtendimento Fluxo { get; set; }
        public virtual DateTime? Dt_criacao { get; set; }
        public virtual DateTime? Dt_extracao { get; set; }
        public virtual DateTime? Dt_encerramento { get; set; }
        public virtual DateTime? Dt_exportacao { get; set; }
        public virtual DateTime? Dt_importacao { get; set; }
        public virtual IList<Ticket> Tickets { get; set; }
        public virtual IList<Nota> Notas { get; set; }
        public virtual Demanda Demanda { get; set; }
        public virtual TbTipoSolicitacao Solicitacao { get; set; }
        public virtual TbStatusTicket Status { get; set; }
        public virtual IList<TbTicketHistorico> Historico { get; set; }
        public virtual IList<TbMobilidade> Mobilidade { get; set; }
        public virtual TbPontoFocal PontoFocal { get; set; }
        public virtual TbEtepa Etapa { get; set; }
        public virtual TbCategoria Categoria { get; set; }
        public virtual TbAcao Acao { get; set; }
        public string Acoes { get; set; }
        public string Anotacoes { get; set; }
        public int id_responsavel { get; set; }

    }
}
