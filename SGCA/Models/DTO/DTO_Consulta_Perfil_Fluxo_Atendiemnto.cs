using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.DTO
{
    public class DTO_Consulta_Perfil_Fluxo_Atendiemnto
    {
        public int id_perfil { get; set; }
        public int id_fluxo_Atendimento { get; set; }
        public string desc_perfil { get; set; }
        public string desc_fluxo_Atendimento { get; set; }
        public string nome_usuario { get; set; }
    }
}