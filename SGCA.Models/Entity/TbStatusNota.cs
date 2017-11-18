using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbStatusNota {
        public TbStatusNota() { }
        public virtual int CodigoStatusNota { get; set; }
        public virtual string Descricao { get; set; }
        public virtual int CodigoGrupo { get; set; }
    }
}
