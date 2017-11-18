using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class AtendimentoPendencias
    {

        public virtual int CodigoNota { get; set; }

        //public virtual DateTime? DataImportacao { get; set; }

        public virtual long NumeroNota { get; set; }

        public virtual string TipoNota { get; set; }

        public virtual DateTime? DataInicioDesejado { get; set; }

        //public virtual DateTime? FimAtendimento { get; set; }

        public virtual DateTime? Instalacao { get; set; }

        public virtual string StatusSistema { get; set; }

        public virtual string StatusUsuario { get; set; }

        public virtual string SegmentoCliente { get; set; }

        public virtual string TextoCodCodif { get; set; }

        public virtual int CodPendencia { get; set; }

        public virtual string DescPendencia { get; set; }

        public virtual string CodAreaDirecionada { get; set; }

        public virtual string DescAreaDirecionada { get; set; }

        public virtual DateTime? DataAtendimento { get; set; }

        public virtual string Responsavel { get; set; }

        public virtual string GrupoAtendimento { get; set; }

        public virtual string Observacoes { get; set; }
        public IList<VwPendeciasA104> Pendencias104 { get; set; }

    }

}
