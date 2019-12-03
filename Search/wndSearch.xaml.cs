using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private clsSearchLogic searchLogic;

        /// <summary>
        /// Filter string for sql
        /// </summary>
        private string filter = "";

        /// <summary>
        /// Previously entered filter
        /// </summary>
        private string prevFilter;

        /// <summary>
        /// Currently selected object
        /// </summary>
        //private Invoice currInvoice;

        private bool cbo1 = false;
        private bool cbo2 = false;
        private bool cbo3 = false;
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
                FillInvoiceFilters();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }

        /// <summary>
        /// wndSearch constructor
        /// </summary>
        public wndSearch(ref Invoice currInvoice)
        {
            try
            {
                InitializeComponent();
                searchLogic = new clsSearchLogic();
                //this.currInvoice = currInvoice;

                FillInvoiceDataGrid();
                FillInvoiceFilters();
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
        /// Fill Invoice dataGrid with all Invoices
        /// </summary>
        private void FillInvoiceDataGrid()
        {
            try
            {
                dgInvoice.ItemsSource = new ObservableCollection<Invoice>(clsSearchLogic.GetDGInvoices());
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }

        /// <summary>
        /// Fills each invoice filter combobox
        /// </summary>
        private void FillInvoiceFilters()
        {
            try
            {
                cboInvoiceNum.ItemsSource = clsSearchLogic.GetCboNum();
                cboInvoiceDate.ItemsSource = clsSearchLogic.GetCboDate();
                cboInvoiceTotCharge.ItemsSource = clsSearchLogic.GetCboTotCharge();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }

        /// <summary>
        /// Filter invoices based on combobox choices
        /// </summary>
        private void FilterInvoices(string cboName, string choice)
        {
            try
            {
                prevFilter = filter;

                if (cboName == "cboInvoiceNum")
                {
                    cboName = "InvoiceNum = ";
                }
                else if (cboName == "cboInvoiceDate")
                {
                    cboName = "InvoiceDate LIKE ";
                    string[] choices = choice.Split(' ');
                    choice = "'" + choices[0] + "%'";
                }
                else
                {
                    cboName = "TotalCost = ";
                }

                if (string.IsNullOrEmpty(filter))
                    filter += cboName + choice;
                else
                    filter += " AND " + cboName + choice;


                ObservableCollection<Invoice> filteredInvoices = new ObservableCollection<Invoice>(clsSearchLogic.GetFilteredInvoices(filter));

                dgInvoice.ItemsSource = filteredInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " ->" + ex.Message);
            }
        }
        #endregion

        #region Events_Handlers
        /// <summary>
        /// If user slected an invoice 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectInvoice_click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Selected invoice
                Invoice selectedInvoice = clsSearchLogic.GetCurrInvoice(((Invoice)dgInvoice.SelectedItem).InvoiceNumber.ToString());
                Main.wndMain.searchedInvoiceNumber = selectedInvoice.InvoiceNumber;

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
                cboInvoiceNum.SelectedValue = null;
                cboInvoiceDate.SelectedValue = null;
                cboInvoiceTotCharge.SelectedValue = null;

                //Update Invoice datagrid
                FillInvoiceDataGrid();

                filter = "";

                btnSelectInvoice.IsEnabled = false;
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
        private void cboInvoiceNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((ComboBox)sender).Text != null)
                {
                    if (cbo1 == true)
                        filter = prevFilter;

                    cbo1 = true;
                    FilterInvoices(((ComboBox)sender).Name, ((ComboBox)sender).SelectedValue.ToString());
                }

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
        private void cboInvoiceDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((ComboBox)sender).Text != null)
                {
                    if (cbo2 == true)
                        filter = prevFilter;

                    cbo2 = true;
                    FilterInvoices(((ComboBox)sender).Name, ((ComboBox)sender).SelectedValue.ToString());
                }
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
        private void cboInvoiceTotCharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((ComboBox)sender).Text != null)
                {
                    if (cbo3 == true)
                        filter = prevFilter;

                    cbo3 = true;
                    FilterInvoices(((ComboBox)sender).Name, ((ComboBox)sender).SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name,
                            ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgInvoice_click(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnSelectInvoice.IsEnabled = true;
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
