using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbStatusProcesso {
        public TbStatusProcesso() { }
        public virtual int CodigoStatusProcesso { get; set; }
        public virtual string Descricao { get; set; }
        //public virtual Processo TbProcesso { get; set; }
    }
}
