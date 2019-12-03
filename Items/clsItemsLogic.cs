using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Items
{
    class clsItemsLogic
    {

        /// <summary>
        /// Gets a list of all Items in the database.
        /// </summary>
        /// <returns>A List of all Items in the database.</returns>
        public static List<Item> GetAllItems()
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsItemsSQL.GetAllItems();
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

        public static int UpdateItem(string itemCode, string itemDesc, double itemCost)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsItemsSQL.UpdateItem(itemCode, itemDesc, itemCost);
                int numRows = db.ExecuteNonQuery(sSQL);

                return numRows;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        public static int AddItem(string itemCode, string itemDesc, double itemCost)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                string sSQL = clsItemsSQL.AddItem(itemCode, itemDesc, itemCost);
                int numRows = db.ExecuteNonQuery(sSQL);

                return numRows;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        public static Boolean CheckPrimaryKey(List<Invoice> invoiceList, string itemCode)
        {
            foreach (var invoice in invoiceList)
            {
                if (invoice.Items.Any(a => a.ItemCode == itemCode))
                {
                   
                    return false;
                }
            }
            return true;
        }
    }
}
