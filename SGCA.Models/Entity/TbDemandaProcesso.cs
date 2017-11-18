using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbDemandaProcesso {
        public TbDemandaProcesso() { }
        public virtual int CodigoDemandaProcesso { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Processo TbProcesso { get; set; }
    }
}
