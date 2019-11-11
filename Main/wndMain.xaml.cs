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
        private bool _modifyingInvoice = false;
        /// <summary>
        /// Property that handles enabling or disabling certain window controls
        /// if an invoice is being added or edited.
        /// </summary>
        private bool ModifyingInvoice
        {
            get
            {
                return _modifyingInvoice;
            }
            set
            {
                _modifyingInvoice = value;
                // Can't open item definitions if we're modifying an invoice.
                mnuItemDefs.IsEnabled = !value;

            }
        }

        /// <summary>
        /// Bool that is used to handle the value for the AddingInvoice property.
        /// </summary>
        private bool _addingInvoice = false;
        /// <summary>
        /// Property that controls access to certain functions if we're adding an invoice.
        /// </summary>
        private bool AddingInvoice
        {
            get
            {
                return _addingInvoice;
            }
            set
            {
                _addingInvoice = value;
                btnEditInvoice.IsEnabled = !value;

            }
        }

        /// <summary>
        /// Holds the Invoice object that is currently being viewed and modified.
        /// </summary>
        public Invoice _currentInvoice;
        /// <summary>
        /// An ObservableCollection of Items, mainly used as the Items Source
        /// for the datagrid.
        /// </summary>
        public ObservableCollection<Item> itemList;

        /// <summary>
        /// A list of items representing the inventory items in the database.
        /// </summary>
        private List<Item> inventoryItems;

        /// <summary>
        /// Default constructor for wndMain.
        /// </summary>
        public wndMain()
        {
            try
            {
                InitializeComponent();
                GetFirstInvoice();
                SetItemsList();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Fetches the first invoice available in the invoice system
        /// and sets the controls to match.
        /// </summary>
        private void GetFirstInvoice()
        {
            try
            {
                SetInvoice(clsMainLogic.GetInvoices().FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Sets the controls of this window around an invoice object.
        /// </summary>
        /// <param name="invoice">The invoice object we're setting to.</param>
        private void SetInvoice(Invoice invoice)
        {
            try
            {
                _currentInvoice = invoice;
                itemList = new ObservableCollection<Item>(_currentInvoice.Items);
                cmbInvoiceItems.ItemsSource = itemList;
                grdInvoiceDetails.ItemsSource = itemList;
                lblInvoiceID.Content = "Invoice ID: " + _currentInvoice.InvoiceNumber;
                lblInvoiceDate.Content = "Invoice Date: " + _currentInvoice.InvoiceDate.ToShortDateString();
                lblTotalCost.Content = "Total Cost: " + Formatting.FormatCurrency(_currentInvoice.TotalCost);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Sets the items list that represents the 
        /// full collection of items available.
        /// </summary>
        private void SetItemsList()
        {
            try
            {
                inventoryItems = clsMainLogic.GetAllItems();
                Item oldSelectedItem = null;
                // To preserve the old item selection, get the selected item, if it exists.
                if(cmbItemSelection.SelectedItem != null)
                    oldSelectedItem = cmbItemSelection.SelectedItem as Item;
                cmbItemSelection.ItemsSource = inventoryItems;
                // Restore the user's item selection, if possible.
                if (oldSelectedItem != null)
                    cmbItemSelection.SelectedItem = cmbItemSelection.Items.OfType<Item>().Where(a => a.ItemCode == oldSelectedItem.ItemCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #region Event Handlers
        /// <summary>
        /// Event handler for the Invoice Search menu button to show the search window.
        /// The main window will probably send a reference to an Invoice object
        /// to the search window.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void mnuInvoiceSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int currentInvoiceID = _currentInvoice.InvoiceNumber;
                new Search.wndSearch().ShowDialog();
                // Initialize wndSearch with a reference to the current Invoice object
                // so that it can directly modify it.
                // Commented out for prototype.
                //new Search.wndSearch(ref _currentInvoice).ShowDialog();
                if (currentInvoiceID != _currentInvoice.InvoiceNumber)
                    SetInvoice(_currentInvoice);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
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
            try
            {
                new Items.wndItems().ShowDialog();
                // Refresh item defs in the event anything was changed.
                SetItemsList();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for clicking the Exit button on the Menu.
        /// Exits the program.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for when a row is selected on the DataGrid
        /// that presents all items in the given invoice.
        /// </summary>
        /// <param name="sender">The DataGrid object that is responsible for sending this event.</param>
        /// <param name="e">Unused.</param>
        private void InvoiceDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Can't do this operation if we don't have a DataGrid as a source.
                if (!(sender is DataGrid)) return;
                DataGrid sourceGrid = sender as DataGrid;
                // Can't do anything if this grid doesn't hold Items.
                if (!(sourceGrid.SelectedItem is Item)) return;
                Item gridSelectedItem = sourceGrid.SelectedItem as Item;
                // Set the combo box for item selection to match what the grid is selecting.
                cmbInvoiceItems.SelectedItem = cmbInvoiceItems.Items.OfType<Item>().Where(a => a.LineItemNumber == gridSelectedItem.LineItemNumber).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the filter text box.  Does an automatic
        /// search of the items list and changes the combo box of items based
        /// on the result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Can't process this event if the sender object isn't a text box.
                if (!(sender is TextBox)) return;
                TextBox senderBox = sender as TextBox;
                Item oldSelectedItem = null;
                // Store the currently selected item.  If their query still matches
                // the item they had selected, we'll make sure to maintain their selection.
                if (cmbItemSelection.SelectedItem is Item)
                    oldSelectedItem = cmbItemSelection.SelectedItem as Item;
                // Set the combo box's items to those items that match by description or item code.
                cmbItemSelection.ItemsSource = inventoryItems.Where(a => a.ItemDesc.ToLower().Contains(senderBox.Text.ToLower()) || a.ItemCode.ToLower().Contains(senderBox.Text.ToLower()));
                // If the selected item is still on the queried list, set it back to where it was;
                // otherwise, it'll set to null.
                if(oldSelectedItem != null)
                    cmbItemSelection.SelectedItem = cmbItemSelection.Items.OfType<Item>().FirstOrDefault(a => a.ItemCode == oldSelectedItem.ItemCode);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for when the Invoice Items combo box is changed.
        /// </summary>
        /// <param name="sender">The Combo Box that had its selection changed.</param>
        /// <param name="e">Unused.</param>
        private void CmbInvoiceItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Can't operate without a source combo box.
            if (!(sender is ComboBox)) return;
            ComboBox senderCombo = sender as ComboBox;
            // If there's nothing in this box, set the grid's selected item to null and
            // return.
            if(senderCombo.SelectedItem == null)
            {
                grdInvoiceDetails.SelectedItem = null;
                return;
            }
            if (!(senderCombo.SelectedItem is Item)) return;
            Item currentItem = senderCombo.SelectedItem as Item;
            grdInvoiceDetails.SelectedItem = grdInvoiceDetails.Items.OfType<Item>().Where(a => a.LineItemNumber == currentItem.LineItemNumber).FirstOrDefault();
        }
        #endregion

    }
}
