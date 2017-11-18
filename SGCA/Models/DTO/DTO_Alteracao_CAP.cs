using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.DTO
{
    public class DTO_Alteracao_CAP
    {
        public int id_ticket { get; set; }
        public int id_nescessidade { get; set; }
        public string acoes { get; set; }
        public string observacoes { get; set; }
        public int id_ponto_focal { get; set; }
        public int id_etapa { get; set; }
        public string detalhe_acoes { get; set; }
        public string mensagem { get; set; }
        public string status_notas { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}