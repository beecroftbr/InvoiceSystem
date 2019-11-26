using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Search
{
    /// <summary>
    /// 
    /// </summary>
    class clsSearchLogic
    {
        #region Methods
        /// <summary>
        /// Get all invoice numbers
        /// </summary>
        public static List<string> GetCboNum()
        {
            try
            {
                List<string> numList = new List<string>();
                int numRows = 0;
                clsDataAccess db;

                db = new clsDataAccess();
                DataSet ds = db.ExecuteSQLStatement(clsSearchSQL.GetInvoiceNums(), ref numRows);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    numList.Add(ds.Tables[0].Rows[i][0].ToString());
                }

                return numList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }

        /// <summary>
        /// Get all dates from invoices
        /// </summary>
        public static List<string> GetCboDate()
        {
            try
            {
                List<string> dateList = new List<string>();
                int numRows = 0;
                clsDataAccess db;

                db = new clsDataAccess();
                DataSet ds = db.ExecuteSQLStatement(clsSearchSQL.GetInvoiceDate(), ref numRows);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dateList.Add(ds.Tables[0].Rows[i][0].ToString());
                }

                return dateList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }

        /// <summary>
        /// Get all total charges
        /// </summary>
        public static List<string> GetCboTotCharge()
        {
            try
            {
                List<string> totChargeList = new List<string>();
                int numRows = 0;
                clsDataAccess db;

                db = new clsDataAccess();
                DataSet ds = db.ExecuteSQLStatement(clsSearchSQL.GetInvoiceTotCharge(), ref numRows);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    totChargeList.Add(ds.Tables[0].Rows[i][0].ToString());
                }

                return totChargeList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }

        /// <summary>
        /// Get all invoices for datagrid
        /// </summary>
        /// <returns></returns>
        public static List<Invoice> GetDGInvoices()
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                int numRows = 0;
                DataSet ds = db.ExecuteSQLStatement(clsSearchSQL.GetAllInvoices(), ref numRows);
                List<Invoice> invoiceList = new List<Invoice>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    invoiceList.Add(new Invoice()
                    {
                        InvoiceNumber = int.TryParse(ds.Tables[0].Rows[i].ItemArray[0].ToString(), out int inNumber) ? inNumber : 0,
                        InvoiceDate = DateTime.Parse(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                        TotalCost = double.TryParse(ds.Tables[0].Rows[i].ItemArray[2].ToString(), out double totalCost) ? totalCost : 0,
                    });
                }
                return invoiceList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }

        /// <summary>
        /// Get filtered invoices
        /// </summary>
        /// <returns></returns>
        public static List<Invoice> GetFilteredInvoices(string filter)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                int numRows = 0;
                DataSet ds = db.ExecuteSQLStatement(clsSearchSQL.GetFilteredInvoices(filter), ref numRows);
                List<Invoice> invoiceList = new List<Invoice>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    invoiceList.Add(new Invoice()
                    {
                        InvoiceNumber = int.TryParse(ds.Tables[0].Rows[i].ItemArray[0].ToString(), out int inNumber) ? inNumber : 0,
                        InvoiceDate = DateTime.Parse(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                        TotalCost = double.TryParse(ds.Tables[0].Rows[i].ItemArray[2].ToString(), out double totalCost) ? totalCost : 0,
                    });
                }
                return invoiceList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }

        /// <summary>
        /// Get current Invoice by selected invoice number
        /// </summary>
        /// <returns></returns>
        public static Invoice GetCurrInvoice(int invoiceNum)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                int numRows = 0;
                DataSet ds = db.ExecuteSQLStatement(clsSearchSQL.GetInvoiceByNum(invoiceNum), ref numRows);

                return new Invoice()
                {
                    InvoiceNumber = int.TryParse(ds.Tables[0].Rows[0].ItemArray[0].ToString(), out int inNumber) ? inNumber : 0,
                    InvoiceDate = DateTime.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString()),
                    TotalCost = double.TryParse(ds.Tables[0].Rows[0].ItemArray[2].ToString(), out double totalCost) ? totalCost : 0,
                    Items = GetItems(inNumber),
                };

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
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
                string sSQL = clsSearchSQL.GetItemsByInvoiceNumber(invoiceNumber);
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
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }

        }
        #endregion
    }
}
