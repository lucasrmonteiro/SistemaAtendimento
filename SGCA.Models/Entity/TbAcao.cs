using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbAcao {
        public virtual int CodigoAcao { get; set; }

        public virtual TbPontoFocal TbPontoFocal { get; set; }
        public virtual int CodigoTicket { get; set; }
        public virtual int? CodigoEtapa { get; set; }
        public virtual int? CodigoCategoria { get; set; }
        public virtual int? CodigoStatusAcaoSap { get; set; }
        public virtual string Observacao { get; set; }
        public virtual string Mensagem { get; set; }
        public virtual string DetalheAcao { get; set; }
        public virtual string PathUpload { get; set; }
    }
}
