using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                //Make a database connection object.
                clsDataAccess db = new clsDataAccess();

                //Call the GetAllItems to generate a string for the SQL statement.
                string sSQL = clsItemsSQL.GetAllItems();

                //Used to capture the number of rows.
                int numRows = 0;

                //Execute the SQL statement and save it as a DataSet.
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref numRows);

                //Grab the first table from the resulting DataSet.
                var tableszero = ds.Tables[0];

                //Create a list to hold all of the Item objects.
                List<Item> itemList = new List<Item>();

                //Loop through the table and add the Items in it to the itemList.
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

                //Return the list of items retrieved from the database.
                return itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method used to Update an Item in the database.
        /// </summary>
        /// <param name="itemCode">ItemCode for the item to be updated.</param>
        /// <param name="itemDesc">New Description for the item.</param>
        /// <param name="itemCost">New Cost for the item.</param>
        /// <returns>Returns the number of rows updated.</returns>
        public static int UpdateItem(string itemCode, string itemDesc, double itemCost)
        {
            try
            {
                //Create a database connection.
                clsDataAccess db = new clsDataAccess();

                //Call the UpdateItem Method to generate a SQL statement with the passed parameters.
                string sSQL = clsItemsSQL.UpdateItem(itemCode, itemDesc, itemCost);

                //Execute the SQL statement and retrieve the number of rows affected by the update. 
                int numRows = db.ExecuteNonQuery(sSQL);

                //Return the number of rows affected.
                return numRows;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Mehtod called to add a new Item into the database. 
        /// </summary>
        /// <param name="itemCode">ItemCode for the new Item.</param>
        /// <param name="itemDesc">Description for the new Item.</param>
        /// <param name="itemCost">Cost for the new Item.</param>
        /// <returns>Returns the number of rows added.</returns>
        public static int AddItem(string itemCode, string itemDesc, double itemCost)
        {
            try
            {
                //Create a database connection.
                clsDataAccess db = new clsDataAccess();

                //Call the AddItem Method to generate a SQL statement with the passed parameters.
                string sSQL = clsItemsSQL.AddItem(itemCode, itemDesc, itemCost);

                //Execute the SQL statement and retrieve the number of rows affected by the insert.
                int numRows = db.ExecuteNonQuery(sSQL);

                //Return the number of rows affected.
                return numRows;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Method called to remove an Item from the database.
        /// </summary>
        /// <param name="itemCode">Item code for the Item to be deleted.</param>
        /// <returns>Returns the number of rows deleted from the database.</returns>
        public static int DeleteItem(string itemCode)
        {
            try
            {
                //Create a database connection.
                clsDataAccess db = new clsDataAccess();

                //Call the DeleteItem Method to generate a SQL statement with the passed parameters.
                string sSQL = clsItemsSQL.DeleteItem(itemCode);

                //Execute the SQL statement and retrieve the number of rows affected by the deletion.
                int numRows = db.ExecuteNonQuery(sSQL);
                
                //Return the number of rows affected.
                return numRows;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Method called to check if an Item already exists with a certain itemCode. Used to insure no duplicate codes are used for Items.
        /// </summary>
        /// <param name="itemList">List of Items to check.</param>
        /// <param name="itemCode">Item code for the new item.</param>
        /// <returns>Returns true if no items exist with that ItemCode.</returns>
        public static Boolean CheckPrimaryKey(ObservableCollection<Item> itemList, string itemCode)
        {
            try
            {
                //For each item in the list
                foreach (Item i in itemList)
                {
                    //Check to see if its item code matches the passed parameter.
                    if (i.ItemCode == itemCode)
                    {
                        //Return false indicating an Item with that ItemCode(Primary Key) already exists.
                        return false;
                    }
                }

                //Returns true if no items currently exist with the desired item code.
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method called to check if an Item is currentely being used by an Invoice.
        /// </summary>
        /// <param name="invoiceList">List of all invoices.</param>
        /// <param name="itemCode">Item code of the Item to check.</param>
        /// <returns></returns>
        public static string CheckItemInvoice(List<Invoice> invoiceList, string itemCode)
        {
            try
            {
                //Generate an empty string.
                string ItemInInvoices = "";

                //Check each invoice to see if an items itemCode in that invoice mathces the passed in itemCode.
                foreach (var invoice in invoiceList)
                {
                    if (invoice.Items.Any(a => a.ItemCode == itemCode))
                    {
                        ItemInInvoices += invoice.InvoiceNumber.ToString() + ", ";

                    }
                }

                //Returns the string of Invoices using that item.
                return ItemInInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
