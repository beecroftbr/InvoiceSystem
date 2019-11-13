using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem
{
    /// <summary>
    /// Formatting class that handles common formatting operations.
    /// </summary>
    class Formatting
    {
        public static string FormatCurrency(float dollarAmount)
        {
            return FormatCurrency((double)dollarAmount);
        }
        public static string FormatCurrency(double dollarAmount)
        {
            return dollarAmount.ToString("C2");
        }
        public static string GetFormattedAccessDateTime(DateTime date)
        {
            return "#" + date.Year + "-" + date.Month.ToString("D2") + "-" + date.Day.ToString("D2")
                + " " + date.Hour.ToString("D2") + ":" + date.Minute.ToString("D2") + ":" + date.Second.ToString("D2") + "#";
        }
    }
}
