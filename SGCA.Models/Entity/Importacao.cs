using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    public class Importacao
    {
        public virtual int IdImportacao { get; set; }
        public virtual string Arquivo { get; set; }
        public virtual DateTime? DataImportacao { get; set; }
        public virtual int QtdRegistroNovo { get; set; }
        public virtual int QtdRegistroAtualizado { get; set; }
        public virtual int QtdRegistroFalha { get; set; }
        public virtual Usuario Analista { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime DataAdicao { get; set; }
    }
}