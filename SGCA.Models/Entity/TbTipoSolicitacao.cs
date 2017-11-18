using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbTipoSolicitacao {
        public TbTipoSolicitacao() { }
        public virtual int CodigoTipoSolicitacao { get; set; }
        public virtual string Descricao { get; set; }
        //public virtual Processo TbProcesso { get; set; }
    }
}
