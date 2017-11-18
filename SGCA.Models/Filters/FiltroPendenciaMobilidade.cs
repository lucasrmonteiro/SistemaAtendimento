using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroPendenciaMobilidade
    {

        public long NumeroNota { get; set; }

        public string TipoNota { get; set; }

        public DateTime? InicioAtedimento { get; set; }

        public DateTime? FimAtendimento { get; set; }

        public DateTime? DataInstalacao { get; set; }

        public int Id_usuario { get; set; }

        public string Dsc_descricao { get; set; }

        public string SegmentoCliente { get; set; }

        public string TextoCodeCodificacao { get; set; }

        public string Descricao { get; set; }

        public int CodigoPendenciaNota { get; set; }

        public int CodigoArea { get; set; }

        public string IdentificacaoArea { get; set; }

        //public string Descricao { get; set; }

        public string Observacao { get; set; }

        public int CodigoGrupo { get; set; }

        public string Dsc_grupo { get; set; }


    }
}
