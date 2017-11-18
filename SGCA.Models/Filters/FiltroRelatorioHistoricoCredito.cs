using SGCA.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Filters
{
    public class FiltroRelatorioHistoricoCredito
    {
        //primeira coluna
        public int? Codigo { get; set; }

        public String Segmento { get; set; }

        public int? Status { get; set; }

        //segunda coluna
        public string Raiz { get; set; }

        public string Pendencia { get; set; }
        
        public string Empresa { get; set; }        

        public virtual Decimal? Valor
        {
            get
            {
                if (!String.IsNullOrEmpty(StringValorAtual))
                {
                    return NumberFormatHelper.ConvertStringToDecimal(StringValorAtual);
                }
                else
                {
                    return null;
                }
            }
            set { }
        }

        public virtual String StringValorAtual { get; set; }

        public String StringDataLancamentoFrom { get; set; }

        public String StringDataLancamentoUntil { get; set; }

        public String StringDataPagamentoFrom { get; set; }

        public String StringDataPagamentoUntil { get; set; }

        public DateTime? DataLancamentoFrom 
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataLancamentoFrom);
            }
        }

        public DateTime? DataLancamentoUntil
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataLancamentoUntil);
            }
        }

        public DateTime? DataPagamentoFrom
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataPagamentoFrom);
            }
        }

        public DateTime? DataPagamentoUntil
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataPagamentoUntil);
            }
        }
              
    }
}