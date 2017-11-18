using System;
using System.Text;
using System.Collections.Generic;



namespace SGCA.Models.Entity
{
    
    public class TbNotaHistorico {
        public virtual long CodigoNotaHistorico { get; set; }
        public virtual Ticket TbTicket { get; set; }
        public virtual TbGrupo TbGrupo { get; set; }
        public virtual Area TbArea { get; set; }
        public virtual Usuario TbUsuario { get; set; }
        public virtual TbStatusNota TbStatusNota { get; set; }
        public virtual int? NumeroNota { get; set; }
        public virtual string PendenciaNs { get; set; }
        public virtual string TipoNota { get; set; }
        public virtual string NotaRegularizacao { get; set; }
        public virtual string StatusNotaSap { get; set; }
        public virtual string StatusNotaUsuario { get; set; }
        public virtual int? IdInstalacao { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Observacao { get; set; }
        public virtual string Anotacoes { get; set; }
        public virtual DateTime? InicioAtendimento { get; set; }
        public virtual DateTime? FimAtendimento { get; set; }
        public virtual string Acao { get; set; }
        public virtual DateTime DataEvento { get; set; }
    }
}
