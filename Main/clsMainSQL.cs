using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
    }
}
