using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGCA.Models.Helpers;

namespace SGCA.Models.Filters
{
    public class FiltroBuscaCredito
    {
        public int? Id_credito { get; set; }

        public virtual Decimal? Num_valorAtual
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

        public String Dsc_raiz { get; set; }

        public String Dsc_pendencia { get; set; }

        public DateTime? Dat_dataLancamentoDe 
        { 
            get 
            {
               return DateHelper.GetDateTimeFromString(StringDataLancamentoFrom);
            } 
        }

        public DateTime? Dat_dataLancamentoAte
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataLancamentoUntil);
            }
        }

        public DateTime? Dat_dataPagamentoDe
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataPagamentoFrom);
            }
        }

        public DateTime? Dat_dataPagamentoAte
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataPagamentoUntil);
            }
        }

        public String StringDataLancamentoFrom { get; set; }

        public String StringDataLancamentoUntil { get; set; }

        public String StringDataPagamentoFrom { get; set; }

        public String StringDataPagamentoUntil { get; set; }

        public String StringDataPagamento { get; set; }

        public DateTime? Dat_dataPagamento
        {
            get
            {
                return DateHelper.GetDateTimeFromString(StringDataPagamento);
            }
        }

        public String Dsc_segmento { get; set; }

        public String Dsc_empresa { get; set; }

        public String Dsc_status { get; set; }

    }
}