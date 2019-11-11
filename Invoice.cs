using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem
{
    /// <summary>
    /// Class that represents a single invoice.
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// The invoice number.
        /// </summary>
        public int InvoiceNumber { get; set; }
        /// <summary>
        /// The date of the invoice.
        /// </summary>
        public DateTime InvoiceDate { get; set; }
        /// <summary>
        /// The total cost of this invoice.
        /// </summary>
        public double TotalCost { get; set; }

        /// <summary>
        /// The list of items that belong to this invoice.
        /// </summary>
        public List<Item> Items { get; set; }

        /// <summary>
        /// The items that this invoice initialized with.
        /// Used for change tracking.  Do not change this collection
        /// after initialization.
        /// </summary>
        private List<Item> InitializedItems { get; set; }


        /// <summary>
        /// Override for the ToString method that outputs the invoice data in a more readable format.
        /// </summary>
        /// <returns>Outputs invoice data in the format ID: [invoice number] | Date: [invoice date] | Cost: [total cost]</returns>
        public override string ToString()
        {
            try
            {
                return "ID: " + InvoiceNumber + " | Date: " + InvoiceDate.ToShortDateString() + " | Cost: " + Formatting.FormatCurrency(TotalCost);
            }

            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
    }
}