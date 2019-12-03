using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Items
{
    class clsItemsSQL
    {

        /// <summary>
        /// Gets the SQL for Deleting an item, given its ItemCode.
        /// </summary>
        /// <param name="itemCode">The Item Code for the item to be deleted.</param>
        /// <returns>A string for the SQL code to delete the item based on the itemCode.</returns>
        public static string DeleteItem(String itemCode)
        {
            return "DELETE FROM ItemDesc WHERE ItemCode = " + itemCode;
        }

        /// <summary>
        /// Gets the SQL for updating an item, given its ItemCode.
        /// </summary>
        /// <param name="itemCode">The ItemCode for the Item to be updated.</param>
        /// <param name="itemDesc">The new ItemDesc for the Item.</param>
        /// <param name="itemCost">The new Cost for the Item.</param>
        /// <returns>A string for the SQL code to update the item.</returns>
        public static string UpdateItem(String itemCode, String itemDesc, double itemCost)
        {
            return "UPDATE ItemDesc SET ItemDesc = '" + itemDesc + "', Cost = " + itemCost + " WHERE itemCode = '" + itemCode +"'";

        }

        /// <summary>
        /// Gets the SQL for adding a new item into the database.
        /// </summary>
        /// <param name="itemCode">The ItemCode for the new Item.</param>
        /// <param name="itemDesc">The ItemDesc for the new Item.</param>
        /// <param name="itemCost">The Cost for the new Item.</param>
        /// <returns>A string for the SQL code to add a new Item to the database with the desired attributes.</returns>
        public static string AddItem(String itemCode, String itemDesc, double itemCost)
        {
            return "INSERT INTO ItemDesc VALUES ('" + itemCode + "', '" + itemDesc + "', " + itemCost + ")";
        }

        /// <summary>
        /// Gets the SQL for returning all Items from the database.
        /// </summary>
        /// <returns>The SQL code for getting a list of all the Items in the database.</returns>
        public static string GetAllItems()
        {
            return "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc";
        }


    }
}
