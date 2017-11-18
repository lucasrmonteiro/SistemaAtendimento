using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Nota
    {
        #region Propriedades

        public virtual int CodigoNota { get; set; }
        public virtual Area TbArea { get; set; }
        public virtual TbGrupo TbGrupo { get; set; }
        public virtual Usuario TbUsuario { get; set; }
        public virtual TbStatusNota TbStatusNota { get; set; }
        public virtual long NumeroNota { get; set; }
        public virtual string PendenciaNs { get; set; }
        public virtual string TipoNota { get; set; }
        public virtual string NotaRegularizacao { get; set; }
        public virtual string StatusNotaSap { get; set; }
        public virtual string StatusNotaUsuario { get; set; }
        public virtual long? IdInstalacao { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Observacao { get; set; }
        public virtual string Anotacoes { get; set; }
        public virtual DateTime? InicioAtendimento { get; set; }
        public virtual DateTime? FimAtendimento { get; set; }
        public virtual DateTime? DataInstalacao { get; set; }
        public virtual string Mensagem { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual Mobilidade Mobilidade { get; set; }

        public virtual PendenciaNota PendenciaNota { get; set; }

public virtual TbTipoSolicitacao TbTipoSolicitacao { get; set; }

        #endregion Propriedades
    }
}