using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    public class PendenciasMobilidade
    {
        public virtual int CodigoNota { get; set; }

        public virtual long NumeroNota { get; set; }

        public virtual string TipoNota { get; set; }

        public virtual DateTime? InicioAtendimento { get; set; }

        public virtual DateTime? FimAtendimento { get; set; }

        public virtual DateTime? DataInstalacao { get; set; }

        public virtual string StatusNotaSap { get; set; }

        public virtual string StatusNotaUsuario { get; set; }

        public virtual string SegmentoCliente { get; set; }

        public virtual string TextoCodeCodificacao { get; set; }

        public virtual string CodigoPendencia { get; set; }

        public virtual string DescricaoPendencia { get; set; }

        public virtual string IdentificacaoArea { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string Dsc_descricao { get; set; }

        //Combo Data Atendimento
        //public virtual DateTime? InicioAtendimento { get; set; }

        public virtual string Dsc_nome { get; set; }

        //Descrição Grupo
        //public virtual string Descricao { get; set; }

        public virtual string Mensagem { get; set; }
    }
}
