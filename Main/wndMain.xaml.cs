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
                btnEditInvoice.IsEnabled = !value;
                grpItemControls.IsEnabled = value;
                mnuInvoiceSearch.IsEnabled = !value;
                if (value)
                {
                    btnAddInvoice.Visibility = Visibility.Hidden;
                    btnDeleteInvoice.Visibility = Visibility.Hidden;
                    btnSaveInvoice.Visibility = Visibility.Visible;
                    btnCancelInvoice.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAddInvoice.Visibility = Visibility.Visible;
                    btnDeleteInvoice.Visibility = Visibility.Visible;
                    btnSaveInvoice.Visibility = Visibility.Hidden;
                    btnCancelInvoice.Visibility = Visibility.Hidden;
                }

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
                btnAddInvoice.IsEnabled = !value;
                grpItemControls.IsEnabled = value;
                mnuInvoiceSearch.IsEnabled = !value;
                mnuItemDefs.IsEnabled = !value;
                if (value)
                {
                    btnEditInvoice.Visibility = Visibility.Hidden;
                    btnDeleteInvoice.Visibility = Visibility.Hidden;
                    btnSaveInvoice.Visibility = Visibility.Visible;
                    btnCancelInvoice.Visibility = Visibility.Visible;
                }
                else
                {
                    btnEditInvoice.Visibility = Visibility.Visible;
                    btnDeleteInvoice.Visibility = Visibility.Visible;
                    btnSaveInvoice.Visibility = Visibility.Hidden;
                    btnCancelInvoice.Visibility = Visibility.Hidden;
                }

            }
        }

        /// <summary>
        /// Bool that is used to handle the value for the DeletingInvoice property.
        /// </summary>
        private bool _deletingInvoice = false;
        /// <summary>
        /// Property that controls access to certain functions if we're deleting an invoice.
        /// </summary>
        private bool DeletingInvoice
        {
            get
            {
                return _deletingInvoice;
            }
            set
            {
                _deletingInvoice = value;
                btnDeleteInvoice.IsEnabled = !value;
                mnuInvoiceSearch.IsEnabled = !value;
                mnuItemDefs.IsEnabled = !value;
                if (value)
                {
                    btnAddInvoice.Visibility = Visibility.Hidden;
                    btnEditInvoice.Visibility = Visibility.Hidden;
                    btnDeleteCancel.Visibility = Visibility.Visible;
                    btnDeleteConfirm.Visibility = Visibility.Visible;
                    lblDeleteConfirmation.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAddInvoice.Visibility = Visibility.Visible;
                    btnEditInvoice.Visibility = Visibility.Visible;
                    btnDeleteCancel.Visibility = Visibility.Hidden;
                    btnDeleteConfirm.Visibility = Visibility.Hidden;
                    lblDeleteConfirmation.Visibility = Visibility.Hidden;
                }

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
        /// Holds information on a new (if any) invoice.
        /// </summary>
        private Invoice newInvoice;

        /// <summary>
        /// Holds a searched invoice number, allowing external windows
        /// to manipulate a window-level variable independent of the window
        /// instance.
        /// </summary>
        public static int searchedInvoiceNumber;

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
                if(!AddingInvoice && !ModifyingInvoice)
                    _currentInvoice = invoice;
                if (invoice != null && invoice.Items.Any())
                    itemList = new ObservableCollection<Item>(invoice.Items);
                else itemList = new ObservableCollection<Item>();
                cmbInvoiceItems.ItemsSource = itemList;
                grdInvoiceDetails.ItemsSource = itemList;
                lblInvoiceID.Content = "Invoice ID: " + (AddingInvoice ? "< New Invoice > " : invoice?.InvoiceNumber.ToString());
                lblInvoiceDate.Content = "Invoice Date: " + invoice?.InvoiceDate.ToShortDateString();
                lblTotalCost.Content = "Total Cost: " + (invoice != null ? Formatting.FormatCurrency(invoice.TotalCost) : "");
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
                // The search window should have changed searchedInvoiceNumber.
                // Set the current invoice to the searched invoice if it has changed.
                if (currentInvoiceID != searchedInvoiceNumber)
                {
                    _currentInvoice = clsMainLogic.GetInvoices().FirstOrDefault(a => a.InvoiceNumber == searchedInvoiceNumber);
                    SetInvoice(_currentInvoice);
                }
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
            try
            {
                // Can't operate without a source combo box.
                if (!(sender is ComboBox)) return;
                ComboBox senderCombo = sender as ComboBox;
                // If there's nothing in this box, set the grid's selected item to null and
                // return.
                if (senderCombo.SelectedItem == null)
                {
                    grdInvoiceDetails.SelectedItem = null;
                    return;
                }
                if (!(senderCombo.SelectedItem is Item)) return;
                Item currentItem = senderCombo.SelectedItem as Item;
                grdInvoiceDetails.SelectedItem = grdInvoiceDetails.Items.OfType<Item>().Where(a => a.LineItemNumber == currentItem.LineItemNumber).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the Add Invoice button.
        /// Changes the view to allow the user to add a new invoice.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void BtnAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // When adding invoice, we should disable or make invisible edit/delete buttons.  Perhaps
                // that space can be repurposed.
                // We should disallow search/item edit functions as well.
                AddingInvoice = true;
                newInvoice = new Invoice()
                {
                    InvoiceNumber = 0,
                    InvoiceDate = DateTime.Today,
                    TotalCost = 0,
                    Items = new List<Item>()
                };
                SetInvoice(newInvoice);
                lblInvoiceID.Content = "Invoice ID: < New Invoice >";
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the Save Invoice button.
        /// Allows the user to save the new invoice they're adding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int invoiceNumber = clsMainLogic.SaveInvoice((AddingInvoice || ModifyingInvoice ? newInvoice : _currentInvoice), AddingInvoice);
                if (AddingInvoice)
                {
                    _currentInvoice = clsMainLogic.GetInvoices().Where(a => a.InvoiceNumber == invoiceNumber).FirstOrDefault();
                    AddingInvoice = !AddingInvoice;
                    SetInvoice(_currentInvoice);
                }
                else if (ModifyingInvoice)
                {
                    _currentInvoice = clsMainLogic.GetInvoices().Where(a => a.InvoiceNumber == invoiceNumber).FirstOrDefault();
                    ModifyingInvoice = false;
                    SetInvoice(_currentInvoice);
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the Cancel invoice button.
        /// Allows the user to back out of invoice creation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelInvoice_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if(AddingInvoice)
                    AddingInvoice = false;
                if (ModifyingInvoice)
                    ModifyingInvoice = false;
                SetInvoice(_currentInvoice);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the button called "Add to Invoice".
        /// Adds an item to the current invoice.
        /// </summary>
        /// <param name="sender">The Button object sending the event.</param>
        /// <param name="e">Unused.</param>
        private void BtnAddToInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // If the wrong thing called this, return.
                if (!(sender is Button)) return;
                // If the item selected in the item selection box isn't an item, we can't add anything.
                if (!(cmbItemSelection.SelectedItem is Item)) return;
                Item selectedItem = cmbItemSelection.SelectedItem as Item;
                Invoice selectedInvoice = (AddingInvoice || ModifyingInvoice ? newInvoice : _currentInvoice);
                Item newItemInstance = new Item()
                {
                    ItemCode = selectedItem.ItemCode,
                    Cost = selectedItem.Cost,
                    ItemDesc = selectedItem.ItemDesc,
                    LineItemNumber = selectedInvoice.Items.Any() ? selectedInvoice.Items.Last().LineItemNumber + 1 : 1
                };
                selectedInvoice.Items.Add(newItemInstance);
                selectedInvoice.TotalCost = selectedInvoice.Items.Sum(a => a.Cost);
                SetInvoice(selectedInvoice);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for when the Item Controls groupbox has its IsEnabled property changed.
        /// </summary>
        /// <param name="sender">The GroupBox that sent the event.</param>
        /// <param name="e">Unused.</param>
        private void GrpItemControls_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!this.IsVisible) return;
                if (!(sender is GroupBox)) return;
                cmbInvoiceItems.SelectedItem = null;
                cmbItemSelection.SelectedItem = null;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the "Edit Invoice" button.
        /// Copies the current invoice into a new invoice instance (to support the cancel function).
        /// Very similar to adding a new invoice.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void BtnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Can't edit nothing
                if (_currentInvoice == null) return;
                ModifyingInvoice = true;
                var itemsCopy = new List<Item>();
                foreach(var item in _currentInvoice.Items)
                {
                    itemsCopy.Add(new Item()
                    {
                        ItemCode = item.ItemCode,
                        ItemDesc = item.ItemDesc,
                        LineItemNumber = item.LineItemNumber,
                        Cost = item.Cost
                    });
                }
                newInvoice = new Invoice()
                {
                    InvoiceNumber = _currentInvoice.InvoiceNumber,
                    InvoiceDate = _currentInvoice.InvoiceDate,
                    TotalCost = _currentInvoice.TotalCost,
                    Items = itemsCopy
                };
                SetInvoice(newInvoice);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the button that allows you to remove an item from the invoice.
        /// </summary>
        /// <param name="sender">Remove selected button.</param>
        /// <param name="e">Unused.</param>
        private void BtnRemoveSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(sender is Button)) return;
                if (!(cmbInvoiceItems.SelectedItem is Item)) return;
                Item selectedItem = cmbInvoiceItems.SelectedItem as Item;
                Invoice modifyingInvoice = ((AddingInvoice || ModifyingInvoice) ? newInvoice : _currentInvoice);
                foreach (var item in modifyingInvoice.Items.Where(a => a.LineItemNumber > selectedItem.LineItemNumber))
                {
                    item.LineItemNumber--;
                }
                modifyingInvoice.Items.Remove(selectedItem);
                modifyingInvoice.TotalCost = modifyingInvoice.Items.Sum(a => a.Cost);
                SetInvoice(modifyingInvoice);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the button that confirms an invoice deletion.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void BtnDeleteConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Can't delete nothing
                if (_currentInvoice == null) return;
                clsMainLogic.DeleteInvoice(_currentInvoice.InvoiceNumber);
                _currentInvoice = null;
                SetInvoice(_currentInvoice);
                DeletingInvoice = false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the cancel button on the delete confirmation view.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void BtnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeletingInvoice = false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the delete button on the main Invoice Controls view.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void BtnDeleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Can't delete nothing.
                if (_currentInvoice != null)
                    DeletingInvoice = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion
    }
}
