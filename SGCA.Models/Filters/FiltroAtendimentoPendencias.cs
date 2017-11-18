using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroAtendimentoPendencias
    {
        public int CodigoNota { get; set; }

        public DateTime? DataImportacao { get; set; }

        public long NumeroNota { get; set; }

        public string TipoNota { get; set; }

        public DateTime? DataInicioDesejado { get; set; }

        //public DateTime? FimAtendimento { get; set; }

        public DateTime? Instalacao { get; set; }

        public string StatusSistema { get; set; }

        public string StatusUsuario { get; set; }

        public string SegmentoCliente { get; set; }

        public string TextoCodeCodificacao { get; set; }

        public int CodPendencia { get; set; }

        public string DescPendencia { get; set; }

        public string CodAreaDirecionada { get; set; }

        public string DescAreaDirecionada { get; set; }

        public DateTime? DataAtendimento { get; set; }

        public string Responsavel { get; set; }

        public string GrupoAtendimento { get; set; }

        public string Observacoes { get; set; }

    }
}
