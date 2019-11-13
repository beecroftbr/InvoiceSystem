using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;

namespace InvoiceSystem.Main
{
    static class clsMainSQL
    {
        /// <summary>
        /// Gets the SQL for deleting line items, given an invoice ID.
        /// </summary>
        /// <param name="invoiceID">The invoice ID corresponding to the line items to delete.</param>
        /// <returns>The SQL statement for deleting all line items that match the invoiceID.</returns>
        public static string DeleteLineItemsByInvoiceID(int invoiceID)
        {
            return "DELETE From LineItems WHERE InvoiceNum = " + invoiceID;
        }

        /// <summary>
        /// Gets the SQL for deleting invoices, given their ID.
        /// </summary>
        /// <param name="invoiceID">The invoice ID corresponding to the invoice to delete.</param>
        /// <returns>The SQL statement for deleting the invoice matching a given ID.</returns>
        public static string DeleteInvoiceByID(int invoiceID)
        {
            return "DELETE From Invoices WHERE InvoiceNum = " + invoiceID;
        }

        /// <summary>
        /// Gets the SQL for setting a cost in Invoices to a particular cost, matching a given invoice ID.
        /// </summary>
        /// <param name="cost">The cost to set the invoice's cost to.</param>
        /// <param name="id">The ID of the invoice to set the cost for.</param>
        /// <returns>The SQL statement for setting a cost in Invoices to a particular cost, matching a given invoice ID.</returns>
        public static string SetCostByID(double cost, int id)
        {
            return "UPDATE Invoices SET TotalCost = " + cost + " WHERE InvoiceNum = " + id;
        }

        /// <summary>
        /// Gets the SQL for getting all invoices in the system.
        /// </summary>
        /// <returns>The SQL statement for getting all invoices in the system.</returns>
        public static string GetInvoices()
        {
            return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
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

        /// <summary>
        /// Gets the SQL for getting all items in the database.
        /// </summary>
        /// <returns>The SQL statement that fetches all items in the databse.</returns>
        public static string GetAllItems()
        {
            return "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc";
        }

        /// <summary>
        /// Gets the SQL for inserting a new Invoice object to the database.
        /// </summary>
        /// <param name="invoiceToSave">The Invoice to save to the database.</param>
        /// <returns>The SQL statement for inserting a new Invoice object to the database.</returns>
        public static string AddNewInvoice(Invoice invoiceToSave)
        {
            return "INSERT INTO Invoices(InvoiceDate, TotalCost) VALUES(" + Formatting.GetFormattedAccessDateTime(invoiceToSave.InvoiceDate) + ", " + invoiceToSave.TotalCost + ")";
        }

        /// <summary>
        /// Gets the SQL for inserting a line item, based on an invoice and an item belonging to that invoice.
        /// </summary>
        /// <param name="invoiceToInsert">The invoice associated with the line item.</param>
        /// <param name="itemToInsert">The item to be associated with the line item.</param>
        /// <returns></returns>
        public static string AddLineItemForInvoice(Invoice invoiceToInsert, Item itemToInsert)
        {
            return "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values (" + invoiceToInsert.InvoiceNumber + ", " + itemToInsert.LineItemNumber + ", '" + itemToInsert.ItemCode + "')";
        }
    }
}