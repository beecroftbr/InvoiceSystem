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
                string sSQL = "UPDATE Invoices SET TotalCost = " + cost + " WHERE InvoiceNum = " + invoiceID;
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
                string sSQL = "DELETE From LineItems WHERE InvoiceNum = " + invoiceID;
                db.ExecuteNonQuery(sSQL);
                // Next, delete the Invoice proper.
                sSQL = "DELETE From Invoices WHERE InvoiceNum = " + invoiceID;
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public static List<Invoice> GetInvoices(bool addAdditionHeader = false)
        {

            clsDataAccess db = new clsDataAccess();
            string sSQL = "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
            int numRows = 0;
            DataSet ds = db.ExecuteSQLStatement(sSQL, ref numRows);
            var tableszero = ds.Tables[0];
            List<Invoice> invoiceList = new List<Invoice>();
            // Add a 'header' invoice, which is just a dummy invoice
            // for certain combo boxes.
            // By default, this does not run.
            if (addAdditionHeader)
            {
                invoiceList.Add(new Invoice()
                {
                    InvoiceNumber = -200,
                    TotalCost = 0
                });
            }
            for (int i = 0; i < tableszero.Rows.Count; i++)
            {
                var rowEntry = tableszero.Rows[i];
                invoiceList.Add(new Invoice()
                {
                    InvoiceNumber = int.TryParse(rowEntry.ItemArray[0].ToString(), out int inNumber) ? inNumber : 0,
                    InvoiceDate = DateTime.Parse(rowEntry.ItemArray[1].ToString()),
                    TotalCost = double.TryParse(rowEntry.ItemArray[2].ToString(), out double totalCost) ? totalCost : 0
                });
            }
            return invoiceList;
        }
    }
}
