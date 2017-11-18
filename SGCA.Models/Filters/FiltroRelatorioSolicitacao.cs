using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroRelatorioSolicitacao
    {
        public int Id_solicitacao { get; set; }

        public int TipoSolicitacao { get; set; }

        public int Produto { get; set; }

        public String DatSolicitacao_de { get; set; }

        public String DatSolicitacao_ate { get; set; }

        public int StatusSolicitacao { get; set; }

        public int Classificacao { get; set; }

        public DateTime DataDe
        {
            get
            {
                if (!String.IsNullOrEmpty(DatSolicitacao_de))
                {
                    var splitDate = DatSolicitacao_de.Split(new char[] { '/' }, 3);
                    return new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]), 00, 00, 00);
                }
                else
                {
                    return new DateTime();
                }
            }
        }

        public DateTime DataAte
        {
            get
            {
                if (!String.IsNullOrEmpty(DatSolicitacao_ate))
                {
                    var splitDate = DatSolicitacao_ate.Split(new char[] { '/' }, 3);
                    return new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]), 23, 59, 59);
                }
                else
                {
                    return new DateTime();
                }
            }
        }
    }
}