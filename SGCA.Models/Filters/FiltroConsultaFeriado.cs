using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SGCA.Models.Validators;
using System.ComponentModel;

namespace SGCA.Models.Filters
{
    public class FiltroConsultaFeriado
    {
        public String StringDataIniFeriado { get; set; }

        [Description("Dat_ini_feriado")]
        public DateTime? Dat_ini_feriado
        {
            get
            {
                if (!String.IsNullOrEmpty(StringDataIniFeriado))
                {
                    var splitDate = StringDataIniFeriado.Split(new char[] { '/' }, 3);
                    return new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]), 00, 00, 00);
                }
                else
                {
                    return new DateTime();
                }
            }
        }

        [Description("Dsc_feriado")]        
        public string Dsc_feriado { get; set; }

        [Description("Num_qtd_dias")]        
        public int? Num_qtd_dias { get; set; }

        [Description("Num_feriado_fixo")]
        public int? Num_feriado_fixo { get; set; }
    }
}