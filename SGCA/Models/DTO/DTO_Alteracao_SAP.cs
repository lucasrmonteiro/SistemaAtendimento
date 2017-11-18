using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.DTO
{
    public class DTO_Alteracao_SAP
    {
        public int id_ticket { get; set; }
        public int id_status { get; set; }
        public int id_categoria { get; set; }
        public string notas { get; set; }
        public string status_notas { get; set; }
    }
}