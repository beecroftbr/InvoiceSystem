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

namespace InvoiceSystem.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {
        /// <summary>
        /// An ObservableCollection of Items, mainly used as the Items Source
        /// for the datagrid.
        /// </summary>
        public ObservableCollection<Item> itemList;

        public static Boolean editing = false;

        public static Item oldSelectedItem = null;

        List<Invoice> invoiceList = Main.clsMainLogic.GetInvoices();

        public static Boolean addingNew = false;

        /// <summary>
        /// Default constructor for wndItems
        /// </summary>
        public wndItems()
        {
            InitializeComponent();
            SetItems();
            codeText.IsEnabled = false;
            descriptionText.IsEnabled = false;
            costText.IsEnabled = false;

        }

        /// <summary>
        /// Gets all of the items from the database and assigns them to the itemDetails Data Grid
        /// </summary>
        public void SetItems()
        {
            

            itemList = new ObservableCollection<Item>(clsItemsLogic.GetAllItems());
            itemDetails.ItemsSource = itemList;
            // Restore the user's item selection, if possible.
            if (oldSelectedItem != null)
            {
                itemDetails.SelectedItem = itemDetails.Items.OfType<Item>().Where(a => a.ItemCode == oldSelectedItem.ItemCode).FirstOrDefault();
                oldSelectedItem = null;
            }
        }

        /// <summary>
        /// Method called when the cancel button is clicked to exit the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (!editing)
            {
                this.Hide();
            }
            else
            {
                DisableEditing();

                if (itemDetails.SelectedItem != null)
                {
                    Item gridSelectedItem = itemDetails.SelectedItem as Item;
                    codeText.Text = gridSelectedItem.ItemCode;
                    descriptionText.Text = gridSelectedItem.ItemDesc;
                    costText.Text = Convert.ToString(gridSelectedItem.Cost);
                }
                else if(oldSelectedItem != null)
                {
                    itemDetails.SelectedItem = itemDetails.Items.OfType<Item>().Where(a => a.ItemCode == oldSelectedItem.ItemCode).FirstOrDefault();
                }
                else
                {
                    codeText.Text = "";
                    descriptionText.Text = "";
                    costText.Text = "";

                }
            }

        }

        /// <summary>
        /// Event handler for when a row is selected on the DataGrid
        /// that presents all items.
        /// </summary>
        /// <param name="sender">The DataGrid object that is responsible for sending this event.</param>
        /// <param name="e">Unused.</param>
        private void ItemDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DisableEditing();
                addingNew = false;
                codeText.IsEnabled = false;

                // Can't do this operation if we don't have a DataGrid as a source.
                if (!(sender is DataGrid)) return;
                DataGrid sourceGrid = sender as DataGrid;
                // Can't do anything if this grid doesn't hold Items.
                if (!(sourceGrid.SelectedItem is Item))
                {
                    DeleteButton.IsEnabled = false;
                    return;
                }
                DeleteButton.IsEnabled = true;

                Item gridSelectedItem = sourceGrid.SelectedItem as Item;
                // Set the labels for item selection to match what the grid is selecting.
                codeText.Text = gridSelectedItem.ItemCode;
                descriptionText.Text = gridSelectedItem.ItemDesc;
                costText.Text = Convert.ToString(gridSelectedItem.Cost);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void EditBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (itemDetails.SelectedItem != null)
            {

                EnableEditing();
                oldSelectedItem = (Item)itemDetails.SelectedItem;
                
            }
        }

        private void SaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!addingNew)
                {
                    Item selected = (Item)itemDetails.SelectedItem;

                    if (!CheckText())
                    {
                        return;
                    }
                    else
                    {
                        selected.ItemDesc = descriptionText.Text;
                        double cost;
                        if(!Double.TryParse(costText.Text, out cost))
                        {
                            MessageBox.Show("Please enter a valid amount for this item");
                            costText.Text = oldSelectedItem.Cost.ToString();
                            return;
                        }
                        selected.Cost = cost;

                        int numRows = clsItemsLogic.UpdateItem(selected.ItemCode, selected.ItemDesc, cost);
                        if (numRows != 1)
                        {
                            throw new Exception("There was an error updating the Item");
                        }

                        oldSelectedItem = selected;

                        DisableEditing();
                        SetItems();
                    }
                }
                else
                {
                    if (!CheckText())
                    {
                        return;
                    }
                    else
                    {                        
                        if(!clsItemsLogic.CheckPrimaryKey(invoiceList, codeText.Text))
                        {
                            MessageBox.Show("An Item with that Code already exists, please enter a different Item Code");
                            return;
                        }
                        double cost;
                        if (!Double.TryParse(costText.Text, out cost))
                        {
                            MessageBox.Show("Please enter a valid amount for this item");                            
                            return;
                        }
                        int numRows = clsItemsLogic.AddItem(codeText.Text, descriptionText.Text, cost);
                        
                        if (numRows != 1)
                        {
                            throw new Exception("There was an error adding the new Item");
                        }
                        addingNew = false;
                        oldSelectedItem = new Item()
                        {
                            ItemCode = codeText.Text,
                            ItemDesc = descriptionText.Text,
                            Cost = cost
                        };
                        DisableEditing();
                        SetItems();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }
        private void EnableEditing()
        {
            editing = true;
            descriptionText.IsEnabled = true;
            costText.IsEnabled = true;

            EditButton.Visibility = Visibility.Hidden;
            EditButton.IsEnabled = false;

            SaveButton.Visibility = Visibility.Visible;
            SaveButton.IsEnabled = true;

            DeleteButton.IsEnabled = false;
            AddButton.IsEnabled = false;

        }

        private void DisableEditing()
        {
            editing = false;
            descriptionText.IsEnabled = false;
            costText.IsEnabled = false;
            EditButton.Visibility = Visibility.Visible;
            EditButton.IsEnabled = true;

            SaveButton.Visibility = Visibility.Hidden;
            SaveButton.IsEnabled = false;

            AddButton.IsEnabled = true;

            if(itemDetails.SelectedItem!= null)
            {
                DeleteButton.IsEnabled = true;
            }
        }

        private void AddNewBtn_Clicked(object sender, RoutedEventArgs e)
        {
            oldSelectedItem = (Item)itemDetails.SelectedItem;
            itemDetails.SelectedItem = null;

            EnableEditing();
            codeText.Text = "";
            descriptionText.Text = "";
            costText.Text = "";
            codeText.IsEnabled = true;
            addingNew = true;

        }

        public Boolean CheckText()
        {
            if(codeText.Text == "" && descriptionText.Text == "" && costText.Text == "")
            {
                MessageBox.Show("Please enter an Item Code, Description and Cost");
                if (!addingNew)
                {
                    descriptionText.Text = oldSelectedItem.ItemDesc;
                    costText.Text = oldSelectedItem.Cost.ToString();
                }
                return false;
            }
            else if (codeText.Text == "" && descriptionText.Text == "")
            {
                MessageBox.Show("Please enter an Item Code and Description");
                if (!addingNew)
                {
                    descriptionText.Text = oldSelectedItem.ItemDesc;
                }
                return false;
            }
            else if (codeText.Text == "" && costText.Text == "")
            {
                MessageBox.Show("Please enter an Item Code and Cost");
                if (!addingNew)
                {
                    costText.Text = oldSelectedItem.Cost.ToString();
                }
                return false;
            }
            else if (descriptionText.Text == "" && costText.Text == "")
            {
                MessageBox.Show("Please enter an Item Description and Cost");
                if (!addingNew)
                {
                    descriptionText.Text = oldSelectedItem.ItemDesc;
                    costText.Text = oldSelectedItem.Cost.ToString();
                }
                return false;
            }
            else if (codeText.Text == "")
            {
                MessageBox.Show("Please enter an Item Code");
                return false;
            }
            else if (descriptionText.Text == "")
            {
                MessageBox.Show("Please enter an Item Description");
                if (!addingNew)
                {
                    descriptionText.Text = oldSelectedItem.ItemDesc;
                }
                return false;
            }
            else if (costText.Text == "")
            {
                MessageBox.Show("Please enter an Item Cost");
                if (!addingNew)
                {
                    costText.Text = oldSelectedItem.Cost.ToString();
                }
                return false;
            }

            return true;

        }

        private void TxtInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ///Try catch block
            try
            {
                ///If the key was not a number key
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                {
                    ///If the key was not the back or delete keys
                    if (!(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Decimal || e.Key == Key.OemPeriod) )
                    {
                        ///Don't allow the key to be pressed and entered into the textBox.
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
    }

    
}
