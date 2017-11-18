using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroSolicitacaoBaixaCobranca
    {
        /// <summary>
        /// 
        /// </summary>
        public int CodSolicitacaoBaixaCobranca { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String DataSolicitacao { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StatusSolicitacao { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String RaizGrupo { get; set; }
    }
}