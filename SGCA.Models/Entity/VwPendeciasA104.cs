using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class VwPendeciasA104 {
        public virtual int CodigoNota { get; set; }

        public virtual DateTime? DataImportacao { get; set; }
        public virtual string SegmentoCliente { get; set; }
        public virtual long NumeroNota { get; set; }
        public virtual string TipoNota { get; set; }
        public virtual DateTime? InicioAtendimento { get; set; }
        public virtual DateTime? DataInstalacao { get; set; }
        public virtual string StatusUsuario { get; set; }
        public virtual string StatusSistema { get; set; }
        public virtual string TextoCodeCod { get; set; }
        public virtual string Rua { get; set; }
        public virtual int? NumeroApartamento { get; set; }
        public virtual string Local { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string CodigoPendecia { get; set; }
        public virtual string DescricaoPendencia { get; set; }
        public virtual int CodigoArea { get; set; }
        public virtual string DescricaoArea { get; set; }
        public virtual DateTime? DataCriacaoA104 { get; set; }
        public virtual string MensagemErro { get; set; }
        public virtual string CentrabRespon { get; set; }
        public virtual int CodigoStatusNota { get; set; }
        public virtual DateTime? DataAtendimentoNota { get; set; }
        public virtual string Responsavel { get; set; }
        public virtual string DescricaoGrupo { get; set; }
    }
}
