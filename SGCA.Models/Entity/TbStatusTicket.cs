using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbStatusTicket {
        public TbStatusTicket() { }
        public virtual int CodigoStatusTicket { get; set; }
        public virtual string Descricao { get; set; }
        //public virtual Processo TbProcesso { get; set; }
    }
}
