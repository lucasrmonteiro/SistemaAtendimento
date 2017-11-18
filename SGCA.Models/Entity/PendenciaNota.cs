using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class PendenciaNota
    {
        #region Propriedades

        public virtual int CodigoPendenciaNota { get; set; }
        public virtual string SegmentoCliente { get; set; }
        public virtual DateTime? inicioDesejado { get; set; }
        public virtual string CentrabRespon { get; set; }
        public virtual int NumeroApartamento { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string CodigoPendencia { get; set; }
        public virtual string DescricaoPendencia { get; set; }
        public virtual string TextoCodeCodificacao { get; set; }
        public virtual DateTime? DataCriacaoA104 { get; set; }
        public virtual string MensagemErro { get; set; }
        public virtual DateTime? DataEncerramentoSAP { get; set; }
        public virtual Nota Nota { get; set; }
       
        #endregion Propriedades

        #region Construtor

        public PendenciaNota()
        {

        }

        public PendenciaNota(Nota nota)
        {
            Nota = nota;
        }

        #endregion
    }
}


