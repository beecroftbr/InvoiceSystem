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
        /// Default constructor for wndItems
        /// </summary>
        public wndItems()
        {
            InitializeComponent();
            SetItems();
        }

        /// <summary>
        /// Gets all of the items from the database and assigns them to the itemDetails Data Grid
        /// </summary>
        public void SetItems()
        {
            itemList = new ObservableCollection<Item>(clsItemsLogic.GetAllItems());
            itemDetails.ItemsSource = itemList;
        }

        /// <summary>
        /// Method called when the cancel button is clicked to exit the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
                // Can't do this operation if we don't have a DataGrid as a source.
                if (!(sender is DataGrid)) return;
                DataGrid sourceGrid = sender as DataGrid;
                // Can't do anything if this grid doesn't hold Items.
                if (!(sourceGrid.SelectedItem is Item)) return;
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
    }
}
