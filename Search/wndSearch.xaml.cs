using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InvoiceSystem.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        #region Attributes
        /// <summary>
        /// Search logic class
        /// </summary>
        clsSearchLogic searchLogic;
        #endregion

        #region Constructor
        /// <summary>
        /// wndSearch constructor
        /// </summary>
        public wndSearch()
        {
            try
            {
                InitializeComponent();
                searchLogic = new clsSearchLogic();

                FillInvoiceDataGrid();

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates Invoice dataGrid
        /// </summary>
        private void FillInvoiceDataGrid()
        {
            try
            {
                //Fill Invoice dataGrid
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }
        #endregion

        #region ErrorHandling
        /// <summary>
        /// If user slected an invoice 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectInvoice_click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Closes search window
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }

        /// <summary>
        /// Reseting Invoice dataGrid, cboInvoiceNum, cboInvoiceDate, and cboInvoiceTotCharge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reseting selected from combo boxes
                cboInvoiceNum.SelectedIndex = -1;
                cboInvoiceDate.SelectedIndex = -1;
                cboInvoiceTotCharge.SelectedIndex = -1;

                //Update Invoice datagrid
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }

        /// <summary>
        /// User selected a invoice number to be used to specify which invoices to display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboInvoiceNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }

        /// <summary>
        /// User selected a invoice date to be used to specify which invoices to display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboInvoiceDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }

        /// <summary>
        /// User selected a invoice total charge to be used to specify which invoices to display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboInvoiceTotCharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }
        #endregion


    }
}
