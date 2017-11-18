using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbTicketHistorico {
        public virtual int IdTicketHistorico { get; set; }
        public virtual Ticket TbTicket { get; set; }
        public virtual TbStatusTicket TbStatusTicket { get; set; }
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual string Anotacoes { get; set; }
    }
}
