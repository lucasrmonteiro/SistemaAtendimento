using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroProcesso
    {

        public int Id_processo { get; set; }

        public int Id_usuario_responsavel { get; set; }

        public int Id_demanda_processo { get; set; }

        public int Id_tipo_solicitacao { get; set; }

        public int Id_status_processo { get; set; }

        public int Id_area_processo { get; set; }

        public int Id_fluxo_atendimento { get; set; }

        public int Num_processo { get; set; }

        public String Dsc_sla_cliente { get; set; }

        public String Dsc_observacao { get; set; }

        public DateTime? Dat_criacao { get; set; }

        public DateTime? Dat_extracao { get; set; }

        public DateTime? Dat_encerramento { get; set; }

        public Boolean Bit_atendimento_outra_area { get; set; }

        public Boolean Bit_atendimento_automatico { get; set; }

    }
}
