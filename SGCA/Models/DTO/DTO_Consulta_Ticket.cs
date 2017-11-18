using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.DTO
{
    public class DTO_Consulta_Ticket
    {
        public int? cod_ticket { get; set; }
        public string responsavel { get; set; }
        public DateTime? dt_criacao { get; set; }
        public DateTime? Dt_extracao { get; set; }
        public DateTime? Dt_encerramento { get; set; }
        public int? id_demanda { get; set; }
        public int? id_solicitacao { get; set; }
        public int? id_status { get; set; }
    }
}