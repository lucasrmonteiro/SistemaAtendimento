using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Helpers
{
    public class DateHelper
    {

        public static int GetDiffDays(DateTime initialDate, DateTime finalDate)
        {
            int days = 0;
            int daysCount = 0;
            days = initialDate.Subtract(finalDate).Days;

            //Módulo
            if (days < 0)
                days = days * -1;

            for (int i = 1; i <= days; i++)
            {
                initialDate = initialDate.AddDays(1);
                //Conta apenas dias da semana.
                if (initialDate.DayOfWeek != DayOfWeek.Sunday &&
                    initialDate.DayOfWeek != DayOfWeek.Saturday)
                    daysCount++;
            }
            return daysCount;
        }

        public static DateTime? GetDateTimeFromString(string sDate)
        {
            if (!String.IsNullOrWhiteSpace(sDate))
            {
                var splitDate = sDate.Split(new char[] { '/' }, 3);
                return new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]), 00, 00, 00);
            }
            else
            {
                return null;
            }
        }

        public static DateTime GetDateTimeFromStringNotNullable(string sDate)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(sDate))
                {
                    var splitDate = sDate.Split(new char[] { '/' }, 3);
                    return new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]), 00, 00, 00);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}