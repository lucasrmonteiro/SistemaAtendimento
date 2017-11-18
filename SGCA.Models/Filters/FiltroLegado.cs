using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroLegado
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string PAR1_t00_quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string PAR1_t00_valor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string PAR1_t08_quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string PAR1_t08_valor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PAR2_t00_quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PAR2_t00_valor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PAR2_t13_quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PAR2_t13_valor { get; set; }


        public bool SomentePrimeiroParPreenchido()
        {
            return String.IsNullOrEmpty(PAR2_t00_quantidade) && String.IsNullOrEmpty(PAR2_t00_valor)
                                && String.IsNullOrEmpty(PAR2_t13_quantidade) && String.IsNullOrEmpty(PAR2_t13_valor);
        }

        public bool SegundoParPreenchido()
        {
            return !String.IsNullOrEmpty(PAR2_t00_quantidade) && !String.IsNullOrEmpty(PAR2_t00_valor)
                                && !String.IsNullOrEmpty(PAR2_t13_quantidade) && !String.IsNullOrEmpty(PAR2_t13_valor);
        }

        public bool SegundoParPreenchidoCorretamente()
        {
            bool umPreenchido = false;            
            

            if (!String.IsNullOrWhiteSpace(PAR2_t00_quantidade))
            {
                umPreenchido = true;
            }

            if (!String.IsNullOrWhiteSpace(PAR2_t00_valor))
            {
                umPreenchido = true;
            }
            else if (umPreenchido)
            {
                return false;
            }

            if (!String.IsNullOrWhiteSpace(PAR2_t13_quantidade))
            {
                umPreenchido = true;
            }
            else if (umPreenchido)
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(PAR2_t13_valor) && umPreenchido)
            {
                return false;                   
            }

            return true;
        }

        public bool ExisteAlguemNull()
        {
            return String.IsNullOrEmpty(PAR2_t00_quantidade) || String.IsNullOrEmpty(PAR2_t00_valor)
                                || String.IsNullOrEmpty(PAR2_t13_quantidade) || String.IsNullOrEmpty(PAR2_t13_valor);
        }
    }
}