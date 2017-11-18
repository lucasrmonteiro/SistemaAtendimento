using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.DTO
{
    public class DTO_Ticket_Salvar
    {
        public int id_ticket { get; set; }
        public string status_notas { get; set; }
        public string anotacoes { get; set; }
    }
}