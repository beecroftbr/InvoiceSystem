using System;
using System.Windows;

namespace InvoiceSystem
{
    /// <summary>
    /// Generic error handler class that takes care of top-level exception handling.
    /// </summary>
    class ErrorHandler
    {
        public static void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage, "An error has occurred.");
            }
            catch(Exception e)
            {
                System.IO.File.AppendAllText("Error_" + System.DateTime.Today.ToShortDateString().Replace(@"/", "-") + ".txt",
                    Environment.NewLine + Environment.NewLine + "[ Error Timestamp: " + System.DateTime.Now.ToLongTimeString()
                    + " ]" + Environment.NewLine + e.Message);
            }
        }
    }
}
