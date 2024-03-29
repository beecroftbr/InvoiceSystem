﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Search
{
    class clsSearchSQL
    {
        /// <summary>
        /// Gets all invoices
        /// </summary>
        /// <returns></returns>
        public static string GetAllInvoices()
        {
            return "SELECT * FROM Invoices";
        }

        /// <summary>
        /// Get all distinct invoice numbers
        /// </summary>
        /// <returns></returns>
        public static string GetInvoiceNums()
        {
            return "SELECT DISTINCT InvoiceNum FROM Invoices";
        }

        /// <summary>
        /// Get invoice dates
        /// </summary>
        /// <returns></returns>
        public static string GetInvoiceDate()
        {
            return "SELECT DISTINCT InvoiceDate FROM Invoices";
        }

        /// <summary>
        /// Get invoice total charges
        /// </summary>
        /// <returns></returns>
        public static string GetInvoiceTotCharge()
        {
            return "SELECT DISTINCT TotalCost FROM Invoices";
        }


        /// <summary>
        /// Get filtered invoices
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string GetFilteredInvoices(string filter)
        {
            return "SELECT * FROM Invoices WHERE " + filter;
        }

        /// <summary>
        /// Get Invoice by invoice number
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public static string GetInvoiceByNum(string invoiceNum)
        {
            return "SELECT * FROM Invoices WHERE InvoiceNum = " + invoiceNum;
        }

        /// <summary>
        /// Get items for an invoice based on invoice number
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public static string GetItemsByInvoiceNum(string invoiceNum)
        {
            return "SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost, LineItems.LineItemNum FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = " + invoiceNum;
        }
    }
}
