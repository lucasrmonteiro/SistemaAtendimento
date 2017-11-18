using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroProcessamentoBaixa
    {

        
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessageResourceType=typeof
            (Resources),ErrorMessageResourceName = 
            "common_error_um_campo_necessario_para_consulta_baixa_processamento")]
        public String StringDataEnvioDe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessageResourceType = typeof
            (Resources), ErrorMessageResourceName =
            "common_error_um_campo_necessario_para_consulta_baixa_processamento")]
        public String StringDataEnvioAte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DataEnvioDe 
        { 
            get
            {
                if (!String.IsNullOrEmpty(StringDataEnvioDe))
                {
                    var splitDate = StringDataEnvioDe.Split(new char[] { '/' }, 3);
                    return new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]),00,00,00);
                }
                else
                {
                    return new DateTime();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DataEnvioAte 
        {
            get
            {
                if (!String.IsNullOrEmpty(StringDataEnvioAte))
                {
                    var splitDate = StringDataEnvioAte.Split(new char[] { '/' }, 3);
                    return new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]),23,59,59);
                }
                else
                {
                    return new DateTime();
                }
            }
        }

    }
}