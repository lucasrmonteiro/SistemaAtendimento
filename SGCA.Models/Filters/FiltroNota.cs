using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroNota
    {

        public int CodigoNota { get; set; }

        public int CodigoArea { get; set; }

        public int CodigoGrupo { get; set; }

        public int CodigoUsuarioResponsavel { get; set; }

        public int CodigoTicket { get; set; }

        public int CodigoStatusNota { get; set; }

        public long NumeroNota { get; set; }

        public string PendeciaNS { get; set; }

        public string TipoNota { get; set; }

        public string NotaRegularizacao { get; set; }

        public string StatusNotaSap { get; set; }

        public string StatusNotaUsuario { get; set; }

        public long IdInstalacao { get; set; }

        public string Cidade { get; set; }

        public string Bairro { get; set; }

        public string Endereco { get; set; }

        public string Observacao { get; set; }

        public string Anotacoes { get; set; }

        public DateTime? InicioAtedimento { get; set; }

        public DateTime? FimAtendimento { get; set; }

        public DateTime? DataInstalacao { get; set; }

        public string Mensagem { get; set; }
    }
}
