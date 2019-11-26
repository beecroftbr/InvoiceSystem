using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Search
{
    class clsSearchSQL
    {
        /// <summary>
        /// Gets all invoices
        /// </summary>
        /// <returns></returns>
        public static string GetAllInvoices()
        {
            return "SELECT * FROM Invoices";
        }

        /// <summary>
        /// Get all distinct invoice numbers
        /// </summary>
        /// <returns></returns>
        public static string GetInvoiceNums()
        {
            return "SELECT DISTINCT InvoiceNum FROM Invoices";
        }

        /// <summary>
        /// Get invoice dates
        /// </summary>
        /// <returns></returns>
        public static string GetInvoiceDate()
        {
            return "SELECT DISTINCT InvoiceDate FROM Invoices";
        }

        /// <summary>
        /// Get invoice total charges
        /// </summary>
        /// <returns></returns>
        public static string GetInvoiceTotCharge()
        {
            return "SELECT DISTINCT TotalCost FROM Invoices";
        }


        /// <summary>
        /// Get filtered invoices
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string GetFilteredInvoices(string filter)
        {
            return "SELECT * FROM Invoices WHERE " + filter;
        }

        public static string GetInvoiceByNum(int invoiceNum)
        {
            return "SELECT * FROM Invoices WHERE InvoiceNum = " + invoiceNum;
        }

        /// <summary>
        /// Gets the SQL for getting all item details that are associated with a given invoice number.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number to query by.</param>
        /// <returns>The SQL statement for getting all item details that are associated with a given invoice number.</returns>
        public static string GetItemsByInvoiceNumber(int invoiceNumber)
        {
            return "SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost, LineItems.LineItemNum " +
                    "FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = " + invoiceNumber;
        }
    }
}
