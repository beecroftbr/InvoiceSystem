using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Main
{
    class clsMainLogic
    {
        /// <summary>
        /// Given an invoice ID and a cost, set the invoice's cost to
        /// the number defined by cost.
        /// </summary>
        /// <param name="invoiceID"></param>
        /// <param name="cost"></param>
        public static void SetCostByID(int invoiceID, int cost)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsMainSQL.SetCostByID(invoiceID, cost);
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Delete the invoice corresponding to the given invoice ID.
        /// </summary>
        /// <param name="invoiceID">The ID corresponding with the invoice to delete.</param>
        public static void DeleteInvoice(int invoiceID)
        {
            try
            {
                // First, delete the line items associated with the Invoice.
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsMainSQL.DeleteLineItemsByInvoiceID(invoiceID);
                db.ExecuteNonQuery(sSQL);
                // Next, delete the Invoice proper.
                sSQL = clsMainSQL.DeleteInvoiceByID(invoiceID);
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of all invoices.
        /// </summary>
        /// <returns>A List of all Invoices.</returns>
        public static List<Invoice> GetInvoices()
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsMainSQL.GetInvoices();
                int numRows = 0;
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref numRows);
                var tableszero = ds.Tables[0];
                List<Invoice> invoiceList = new List<Invoice>();
                for (int i = 0; i < tableszero.Rows.Count; i++)
                {
                    var rowEntry = tableszero.Rows[i];
                    invoiceList.Add(new Invoice()
                    {
                        InvoiceNumber = int.TryParse(rowEntry.ItemArray[0].ToString(), out int inNumber) ? inNumber : 0,
                        InvoiceDate = DateTime.Parse(rowEntry.ItemArray[1].ToString()),
                        TotalCost = double.TryParse(rowEntry.ItemArray[2].ToString(), out double totalCost) ? totalCost : 0,
                        Items = GetItems(inNumber),
                    });
                }
                return invoiceList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets all items that belong to a given invoice ID.
        /// </summary>
        /// <param name="invoiceNumber">The ID of the invoice to search by.</param>
        /// <returns>A List of Items belonging to the given invoice ID.</returns>
        public static List<Item> GetItems(int invoiceNumber)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsMainSQL.GetItemsByInvoiceNumber(invoiceNumber);
                int numRows = 0;
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref numRows);
                var tableszero = ds.Tables[0];
                List<Item> itemList = new List<Item>();
                for (int i = 0; i < tableszero.Rows.Count; i++)
                {
                    var rowEntry = tableszero.Rows[i];
                    itemList.Add(new Item()
                    {
                        ItemCode = rowEntry.ItemArray[0].ToString(),
                        ItemDesc = rowEntry.ItemArray[1].ToString(),
                        Cost = double.TryParse(rowEntry.ItemArray[2].ToString(), out double totalCost) ? totalCost : 0,
                        LineItemNumber = int.TryParse(rowEntry.ItemArray[3].ToString(), out int liNumber) ? liNumber : 0
                    });
                }
                return itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Gets a list of all Items in the database.
        /// </summary>
        /// <returns>A List of all Items in the database.</returns>
        public static List<Item> GetAllItems()
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsMainSQL.GetAllItems();
                int numRows = 0;
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref numRows);
                var tableszero = ds.Tables[0];
                List<Item> itemList = new List<Item>();
                for (int i = 0; i < tableszero.Rows.Count; i++)
                {
                    var rowEntry = tableszero.Rows[i];
                    itemList.Add(new Item()
                    {
                        ItemCode = rowEntry.ItemArray[0].ToString(),
                        ItemDesc = rowEntry.ItemArray[1].ToString(),
                        Cost = double.TryParse(rowEntry.ItemArray[2].ToString(), out double totalCost) ? totalCost : 0
                    });
                }
                return itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
