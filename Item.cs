using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem
{
    /// <summary>
    /// Class that represents a single item entry.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Line item number, if this item is part of an invoice.
        /// </summary>
        public int LineItemNumber { get; set; }
        /// <summary>
        /// The item code for this item.
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// The description of this item.
        /// </summary>
        public string ItemDesc { get; set; }
        /// <summary>
        /// The cost of this item.
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Override for the ToString method that outputs the item data in a more readable format.
        /// </summary>
        /// <returns>Outputs item data in the format Item: [item description] | Cost: [cost]</returns>
        public override string ToString()
        {
            try
            {
                return "Item: " + ItemDesc + " | Cost: " + Formatting.FormatCurrency(Cost);
            }

            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
    }
}