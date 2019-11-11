using System;
using System.Collections.Generic;
using System.Linq;
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

namespace InvoiceSystem.Main
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        /// <summary>
        /// Bool that is used to handle the value for the ModifyingInvoice property.
        /// </summary>
        private bool _modifyingInvoie = false;
        /// <summary>
        /// Property that handles enabling or disabling certain window controls
        /// if an invoice is being added or edited.
        /// </summary>
        private bool ModifyingInvoice
        {
            get
            {
                return _modifyingInvoie;
            }
            set
            {
                _modifyingInvoie = value;
                mnuItemDefs.IsEnabled = !value;
            }
        }

        private bool _addingInvoice = false;
        private bool AddingInvoice
        {
            get
            {
                return _addingInvoice;
            }
            set
            {
                _addingInvoice = value;

            }
        }

        public Invoice _currentInvoice;

        public wndMain()
        {
            InitializeComponent();
            SetupInvoiceCombo();
        }

        private void SetupInvoiceCombo()
        {
            //cmbInvoiceSelection.ItemsSource = clsMainSQL.GetInvoices(true);
        }

        #region Event Handlers
        /// <summary>
        /// Event handler for the Invoice Search menu button to show the search window.
        /// At this time, there is no indication that the main window will need to send
        /// information to the search window.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void mnuInvoiceSearch_Click(object sender, RoutedEventArgs e)
        {
            // Initialize wndSearch with a reference to the current Invoice object
            // so that it can directly modify it.
            // Commented out for prototype.
            //new Search.wndSearch(ref _currentInvoice).ShowDialog();
            new Search.wndSearch().ShowDialog();
        }

        /// <summary>
        /// Event handler for the Update Item Definitions button to show the item definitions.
        /// At this time, there is no indication that the main window will need to send
        /// information to the items window.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void mnuItemDefs_Click(object sender, RoutedEventArgs e)
        {
            new Items.wndItems().ShowDialog();
        }

        /// <summary>
        /// Event handler for clicking the Exit button on the Menu.
        /// Exits the program.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void CmbInvoiceSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Can't do this operation if this sender isn't a combo box.
            if (!(sender is ComboBox)) return;
            ComboBox senderCombo = sender as ComboBox;
            // Can't do this operation if an invoice isn't selected.
            if (!(senderCombo.SelectedItem is Invoice)) return;

            Invoice selectedInvoice = senderCombo.SelectedItem as Invoice;

            // At this point, we're making some sort of modification.
            ModifyingInvoice = true;

            if (selectedInvoice.InvoiceNumber == -200)
            {
                _currentInvoice = new Invoice()
                {
                    InvoiceDate = DateTime.Today
                };
            }
            else _currentInvoice = selectedInvoice;
        }

        #endregion

        private void InvoiceDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is DataGrid)) return;
            DataGrid sourceGrid = sender as DataGrid;
            
        }
    }
}
