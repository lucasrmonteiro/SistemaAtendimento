using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.DTO
{
    public class DTO_Alterar_Grid_Mobilidade
    {
        public int cod_nota { get; set; }
        public int status_edit { get; set; }
        public int grupo_edit { get; set; }
        public int resp_edit { get; set; }
        public string Observacoes { get; set; }
    }
}