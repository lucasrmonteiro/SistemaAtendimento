using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbMobilidade {
        public virtual int CodigoMobilidade { get; set; }
        public virtual Nota TbNota { get; set; }
        public virtual string StatusOs { get; set; }
        public virtual string SubcategoriaOs { get; set; }
        public virtual int? ZonaAtendimento { get; set; }
        public virtual string RegistroGasista { get; set; }
        public virtual string NomeGasista { get; set; }
        public virtual string Viatura { get; set; }
        public virtual string DescricaoMaterial { get; set; }
        public virtual int? Quantidade { get; set; }
        public virtual decimal? Valor { get; set; }
    }
}
