using SGCA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroCreditoPerformance
    {
        public DateTime? DataPagamentoFrom { get; set; }

        public DateTime? DataPagamentoUntil { get; set; }

        public DateTime? DataLancamentoFrom { get; set; }

        public DateTime? DataLancamentoUntil { get; set; }

        public string Empresa { get; set; }

        public string Segmento { get; set; }

        public string Raiz { get; set; }

        public string Pendencia { get; set; }

        public int StatusCredito { get; set; }
    }
}