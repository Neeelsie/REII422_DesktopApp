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
using RealEstate.Classes;
using RealEstate.GridViewSources;
using RealEstate.Windows;
using RealEstate.Overlays.Client;
using Framework.UI.Controls;

namespace RealEstate.Views.AgentViews
{
    /// <summary>
    /// Interaction logic for ListingsView.xaml
    /// </summary>
    public partial class ListingsView
    {
        public ListingsView()
        {
            InitializeComponent();
            RefreshListings();
        }

        private void ShowAgentWindow()
        {
            (this.Tag as AgentWindow).AV_Listings.Visibility = System.Windows.Visibility.Hidden;
            (this.Tag as AgentWindow).BT_ManageCustomers.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Listings.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Manage.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Properties.Visibility = System.Windows.Visibility.Visible;
        }
        private string GetCurrentAgent()
        {
            string currentAgent = "";

            this.Dispatcher.Invoke(() =>
            {
                currentAgent = (this.Tag as AgentWindow).currentAgentName;
            });

            return currentAgent;
        }
        private void ListingsOverlays_OnClose(object sender, EventArgs e)
        {
            RefreshListings();
        }
        #region Input Response
        private void BT_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAgentWindow();
        }

        private void DG_Listings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BT_AddListing_Click(object sender, RoutedEventArgs e)
        {
            //OpenEditListingsOverlay();
            OpenAddListingView();

        }

        private void BT_EditListing_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BT_DeleteListing_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BT_Refresh_Click(object sender, RoutedEventArgs e)
        {

        }
#endregion

        #region OpenOverlays
        private void OpenEditListingsOverlay()
        {
            Overlays.Listings.AddListingOverlay listingOverlay = new Overlays.Listings.AddListingOverlay(GetCurrentAgent());
            listingOverlay.OnExit += ListingsOverlays_OnClose;
            listingOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            listingOverlay.Show();
        }

        private void OpenAddListingView()
        {
            (this.Tag as AgentWindow).HideListingsView();
            (this.Tag as AgentWindow).ShowAddListingView();
        }
        #endregion
        #region FormControl
            private void RefreshListings()
        {
            new System.Threading.Thread(() =>
            {
                ClearListingsGrid();
                DatabaseManager dbManager = new DatabaseManager();
                var listingInfo = dbManager.ReturnQuery("SELECT Client_Name, Client_Surname, Client_Phone, Client_Email FROM Clients ORDER BY Client_Surname, Client_Name;");

                foreach (var list in listingInfo)
                {
                    InsertIntoListingsGrid(list[0], list[1], list[2], list[3]);
                }
            }).Start();
        }
        private void ClearListingsGrid()
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Listings.Items.Clear();
            });
        }
        private void InsertIntoListingsGrid(string clientName, string clientSurname, string clientPhone, string clientEmail)
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Listings.Items.Add(new Client() { Name = clientName, Surname = clientSurname, Phone = clientPhone, Email = clientEmail });
            });
        }
        #endregion

    }
}
