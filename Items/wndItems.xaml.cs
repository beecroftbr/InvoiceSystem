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

        /// <summary>
        /// Boolean used to tell other methods if the user is currently editing an item or not.
        /// Used to enable or disable buttons.
        /// </summary>
        public static Boolean editing = false;

        /// <summary>
        /// Item object used to store the old item to grab information from.
        /// </summary>
        public static Item oldSelectedItem = null;

        /// <summary>
        /// List of Invoice objects used to check if an Invoice contains an item.
        /// </summary>
        List<Invoice> invoiceList = Main.clsMainLogic.GetInvoices();

        /// <summary>
        /// Boolean used when the user is adding a new item. Used mainly to enable and disable buttons.
        /// </summary>
        public static Boolean addingNew = false;

        /// <summary>
        /// Default constructor for wndItems.
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
        /// Gets all of the items from the database and assigns them to the itemDetails Data Grid.
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
        /// Method called when the cancel button is clicked to exit the window, cancel adding a new item, or cancel editing an item.
        /// </summary>
        /// <param name="sender">Cancel Button</param>
        /// <param name="e">Click event</param>
        private void CancelBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                //If the user is not currently editing/adding an Item.
                if (!editing)
                {
                    //Close this window.
                    this.Hide();
                }
                else
                {
                    //Call method to disable the text boxes and save button. 
                    DisableEditing();

                    //If there is an Item currently selected on the DataGrid.
                    if (itemDetails.SelectedItem != null)
                    {
                        //Grab the Item that is selected
                        Item gridSelectedItem = itemDetails.SelectedItem as Item;

                        //Set the text in the text boxes to match the code, description, and cost of the Item.
                        codeText.Text = gridSelectedItem.ItemCode;
                        descriptionText.Text = gridSelectedItem.ItemDesc;
                        costText.Text = Convert.ToString(gridSelectedItem.Cost);
                    }
                    //If there was an item that was selected before, in the case of adding a new item.
                    else if (oldSelectedItem != null)
                    {
                        //Make the SelectedItem of the DataGrid be the old selected Item.
                        itemDetails.SelectedItem = itemDetails.Items.OfType<Item>().Where(a => a.ItemCode == oldSelectedItem.ItemCode).FirstOrDefault();
                    }
                    else
                    {
                        //Reset the code of the text boxes.
                        codeText.Text = "";
                        descriptionText.Text = "";
                        costText.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
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
                //If the user was in the middle of editing an item and changes their selection, disable the editing button.
                DisableEditing();

                //If the user was adding a new item and changes their selection, change the addingNew Boolean and disable the codeText textbox. 
                addingNew = false;
                codeText.IsEnabled = false;

                // Can't do this operation if we don't have a DataGrid as a source.
                if (!(sender is DataGrid)) return;
                DataGrid sourceGrid = sender as DataGrid;
                // Can't do anything if this grid doesn't hold Items.
                if (!(sourceGrid.SelectedItem is Item))
                {
                    //Disable the delete button and return.
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

        /// <summary>
        /// If the edit button was clicked, enable certain boxes, and save the old selected item.
        /// </summary>
        /// <param name="sender">Edit Button</param>
        /// <param name="e">Click Event</param>
        private void EditBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                //As long as there was an Item selected.
                if (itemDetails.SelectedItem != null)
                {
                    //Call the EnableEditing method to enable the text boxes and the save button.
                    EnableEditing();

                    //Capture the selected item and its attributes.
                    oldSelectedItem = (Item)itemDetails.SelectedItem;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// If the save button was clicked, either add a new item, or update the item.
        /// </summary>
        /// <param name="sender">Save Button</param>
        /// <param name="e">Click Event</param>
        private void SaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                //If the user is not adding a new Item, they are editing an existing item.
                if (!addingNew)
                {
                    //Grab the Item from the DataGrid
                    Item selected = (Item)itemDetails.SelectedItem;

                    //Call the CheckText method to check if the textBoxes are empty
                    if (!CheckText())
                    {
                        //If they are empty return.
                        return;
                    }
                    else
                    {
                        //Set the new description for the Item.
                        selected.ItemDesc = descriptionText.Text;

                        //Makes sure the user entered a valid input.
                        if (!Double.TryParse(costText.Text, out double cost))
                        {
                            MessageBox.Show("Please enter a valid amount for this item");
                            costText.Text = oldSelectedItem.Cost.ToString();
                            return;
                        }

                        //Set the new cost for the Item.
                        selected.Cost = cost;

                        //Call the UpdateItem method to make a connection to the database to update the Item on the database.
                        int numRows = clsItemsLogic.UpdateItem(selected.ItemCode, selected.ItemDesc, cost);

                        //If there isn't 1 row returned throw an error.
                        if (numRows != 1)
                        {
                            throw new Exception("There was an error updating the Item");
                        }

                        //Save the Item so its the selected item when the method finishes.
                        oldSelectedItem = selected;

                        //Disable the editing buttons and fields.
                        DisableEditing();

                        //Gather the new Item list from the database and update the DataGrid.
                        SetItems();
                    }
                }
                else
                {
                    //Call the CheckText method to check if the textBoxes are empty
                    if (!CheckText())
                    {
                        return;
                    }
                    else
                    {          
                        //Check the itemList to see if an Item with that Code already exists. Two items cannot have the same code as it is the primary key.
                        if(!clsItemsLogic.CheckPrimaryKey(itemList, codeText.Text))
                        {
                            //Display a message informing the user that an Item with that code exists.
                            MessageBox.Show("An Item with that Code already exists, please enter a different Item Code");
                            return;
                        }

                        //Makes sure the user entered a valid input.
                        if (!Double.TryParse(costText.Text, out double cost))
                        {
                            MessageBox.Show("Please enter a valid amount for this item");
                            return;
                        }

                        //Call the AddItem method to add the new Item to the database with the information entered in the textboxes, and capture the number of 
                        //rows affected by the insert.
                        int numRows = clsItemsLogic.AddItem(codeText.Text, descriptionText.Text, cost);
                        
                        //If the number of affected rows does not equal 1 throw an error.
                        if (numRows != 1)
                        {
                            throw new Exception("There was an error adding the new Item");
                        }

                        //Reset the addingNew boolean.
                        addingNew = false;

                        //Capture this Item as a new Item and save it to the oldSelectedItem so when the method finished it is the selected Item.
                        oldSelectedItem = new Item()
                        {
                            ItemCode = codeText.Text,
                            ItemDesc = descriptionText.Text,
                            Cost = cost
                        };

                        //Disable all editing fields and buttons.
                        DisableEditing();

                        //Get the updated list of items from the database, and update the DataGrid.
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

        /// <summary>
        /// Method called when the user is editing fields of an Item, either adding a new Item, or editing and existing one. 
        /// </summary>
        private void EnableEditing()
        {
            try
            {
                //Set the boolean to true for other methods.
                editing = true;

                //Enable the textboxes.
                descriptionText.IsEnabled = true;
                costText.IsEnabled = true;

                //Hide and disable the Edit Button
                EditButton.Visibility = Visibility.Hidden;
                EditButton.IsEnabled = false;

                //Show enable the Save button
                SaveButton.Visibility = Visibility.Visible;
                SaveButton.IsEnabled = true;

                //Disable the Delete and Add Buttons.
                DeleteButton.IsEnabled = false;
                AddButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Method called when the user is finished editing, hits cancel, or changes the selected Item.
        /// </summary>
        private void DisableEditing()
        {
            try
            {
                //Set the editing boolean to false.
                editing = false;

                //Disable the text boxes.
                descriptionText.IsEnabled = false;
                costText.IsEnabled = false;

                //Show and enable the Edit Button.
                EditButton.Visibility = Visibility.Visible;
                EditButton.IsEnabled = true;

                //Hide and disable the Save Button.
                SaveButton.Visibility = Visibility.Hidden;
                SaveButton.IsEnabled = false;

                //Enable the Add New Button.
                AddButton.IsEnabled = true;

                //If there is an item selected enable the Delete Button.
                if (itemDetails.SelectedItem != null)
                {
                    DeleteButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method called when the Add New Button is clicked. Handles enabling buttons, fields, and resets text boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                //Captures the selected item.
                oldSelectedItem = (Item)itemDetails.SelectedItem;

                //Set the DataGrid to null.
                itemDetails.SelectedItem = null;

                //Enable the editing fields and buttons.
                EnableEditing();

                //Clear the text boxes text fields. 
                codeText.Text = "";
                descriptionText.Text = "";
                costText.Text = "";

                //Enable the Code Text field.
                codeText.IsEnabled = true;

                //Set the addingNew boolean to true for other methods.
                addingNew = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method called to check if the text boxes are empty.
        /// </summary>
        /// <returns>Returns true if none of the text boxes are empty.</returns>
        public Boolean CheckText()
        {
            try
            {
                //If all text boxes are empty display an error and reset the text fields.
                if (codeText.Text == "" && descriptionText.Text == "" && costText.Text == "")
                {
                    MessageBox.Show("Please enter an Item Code, Description and Cost");
                    if (!addingNew)
                    {
                        descriptionText.Text = oldSelectedItem.ItemDesc;
                        costText.Text = oldSelectedItem.Cost.ToString();
                    }
                    return false;
                }
                //If the codeText and descriptionText are empty display and error and reset the description field.
                else if (codeText.Text == "" && descriptionText.Text == "")
                {
                    MessageBox.Show("Please enter an Item Code and Description");
                    if (!addingNew)
                    {
                        descriptionText.Text = oldSelectedItem.ItemDesc;
                    }
                    return false;
                }
                //If the code and cost text fields are empty display and error and reset the cost field. 
                else if (codeText.Text == "" && costText.Text == "")
                {
                    MessageBox.Show("Please enter an Item Code and Cost");
                    if (!addingNew)
                    {
                        costText.Text = oldSelectedItem.Cost.ToString();
                    }
                    return false;
                }
                //If the description and cost text fields are empty display an error and reset the text fields.
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
                //If the code text field is empty display and error.
                else if (codeText.Text == "")
                {
                    MessageBox.Show("Please enter an Item Code");
                    return false;
                }
                //If the description text field is empty, display an error and reset the text field. 
                else if (descriptionText.Text == "")
                {
                    MessageBox.Show("Please enter an Item Description");
                    if (!addingNew)
                    {
                        descriptionText.Text = oldSelectedItem.ItemDesc;
                    }
                    return false;
                }
                //If the cost text field is empty, display an error and reset the text field.
                else if (costText.Text == "")
                {
                    MessageBox.Show("Please enter an Item Cost");
                    if (!addingNew)
                    {
                        costText.Text = oldSelectedItem.Cost.ToString();
                    }
                    return false;
                }

                //If none of the text fields are empty return true.
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method used to ensure only numbers, the decimal key, backspace, or delete, can be entered into the cost text field.
        /// </summary>
        /// <param name="sender">Cost text field</param>
        /// <param name="e">Key event</param>
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

        /// <summary>
        /// Method called when the delete button is clicked. It checks to make sure the selected Item isn't currently being used first. 
        /// </summary>
        /// <param name="sender">Delete Button</param>
        /// <param name="e">Click event</param>
        private void DeleteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                //Capture the selected item.
                Item selected = itemDetails.SelectedItem as Item;

                //Call the CheckItemInvoice method to get a list of all Invoice currently using the selected item.
                string check = clsItemsLogic.CheckItemInvoice(invoiceList, selected.ItemCode);

                //If the list is not empty
                if (!check.Equals(""))
                {
                    //Display a message showing which invoices are currently using the item.
                    MessageBox.Show("That Item is currently being used in Invoice(s): " + check + " so it cannot be deleted.");
                    return;
                }

                //Calls the DeleteItem method to delete the Item from the database with the selected Items ItemCode, and captures the number of deleted rows.
                int rows = clsItemsLogic.DeleteItem(selected.ItemCode);

                //If the number of rows deleted does not equal 1, throw an error.
                if (rows != 1)
                {
                    throw new Exception("There was an error deleting the Item");
                }

                //Grab the new Items from the database and update the DataGrid.
                SetItems();
                
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
    }

    
}
