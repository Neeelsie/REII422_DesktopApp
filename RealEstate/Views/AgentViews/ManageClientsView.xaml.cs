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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RealEstate.Classes;
using RealEstate.GridViewSources;
using RealEstate.Windows;
using RealEstate.Overlays.Client;
using Framework.UI.Controls;

namespace RealEstate.Views.AgentViews
{
    /// <summary>
    /// Interaction logic for ManageClientsView.xaml
    /// </summary>
    public partial class ManageClientsView
    {
        public ManageClientsView()
        {
            InitializeComponent();
            LoadSearchCriteria();
        }

        private void RE_ManageClients_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshClients();
        }

        #region Input Response
        private void BT_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAgentWindow();
        }


        private void BT_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshClients();
        }

        private void BT_AddClient_Click(object sender, RoutedEventArgs e)
        {
            OpenAddClientOverlay();
        }

        private void BT_EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (DG_Clients.SelectedIndex != -1)
            {
                OpenEditClientOverlay();
            }
            else
            {
                DisplayNotifyBox("Cannot edit", "Please select an client to edit", 2);
            }
        }

        private async void BT_DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (DG_Clients.SelectedIndex != -1)
            {
                MessageBoxResult result = await MessageDialog.ShowAsync("Are you Sure?", "Are you sure you want to delete " + GetSelectedEmail(), MessageBoxButton.YesNo, MessageDialogType.Light);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteClient();
                }
                RefreshClients();
            }
            else
            {
                DisplayNotifyBox("Cannot delete", "Please select an client to delete", 2);
            }
        }

        private void BT_EditPreferences_Click(object sender, RoutedEventArgs e)
        {
            if (DG_Clients.SelectedIndex != -1)
            {
                OpenEditPreferenceOverlay();
            }
            else
            {
                DisplayNotifyBox("Cannot Edit Preference", "Please select an client to edit preferences", 2);
            }
        }

        private void BT_Search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
#endregion
        
        #region Form Control

        private void ClientOverlays_OnClose(object sender, EventArgs e)
        {
            RefreshClients();
        }

        private void DG_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DG_Clients.SelectedIndex != -1)
            {
                TB_SelectedClient.Text = GetSelectedName() + " " + GetSelectedSurname();
            }
            else
            {
                TB_SelectedClient.Text = "";
            }
        }

        private void RefreshClients()
        {
            new System.Threading.Thread(() =>
            {
                ClearClientsGrid();
                DatabaseManager dbManager = new DatabaseManager();
                var clientNames = dbManager.ReturnQuery("SELECT Client_Name, Client_Surname, Client_Phone, Client_Email FROM Clients ORDER BY Client_Surname, Client_Name;");

                foreach (var client in clientNames)
                {
                    InsertIntoClientsGrid(client[0], client[1], client[2], client[3]);
                }
            }).Start();
        }

        private void ClearClientsGrid()
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Clients.Items.Clear();
            });
        }

        private void InsertIntoClientsGrid(string clientName, string clientSurname, string clientPhone, string clientEmail)
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Clients.Items.Add(new Client() { Name = clientName, Surname = clientSurname, Phone = clientPhone, Email = clientEmail });
            });
        }

        private void ShowAgentWindow()
        {
            (this.Tag as AgentWindow).AV_MangeClients.Visibility = System.Windows.Visibility.Hidden;
            (this.Tag as AgentWindow).BT_ManageCustomers.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Listings.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Manage.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Properties.Visibility = System.Windows.Visibility.Visible;
        }

        private void LoadSearchCriteria()
        {
            CB_SearchField.Items.Add("Name");
            CB_SearchField.Items.Add("Surname");
            CB_SearchField.Items.Add("Phone");
            CB_SearchField.Items.Add("Email");
            CB_SearchField.SelectedIndex = 0;
        }

        private void DisplayNotifyBox(string title, string message, int duration)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, duration), false);
            });
        }

        private void Search()
        {
            new System.Threading.Thread(() =>
            {
                ClearClientsGrid();

                int searchColumn = GetSelectedSearchField();

                DatabaseManager dbManager = new DatabaseManager();

                var result = dbManager.ReturnQuery("SELECT Client_Name, Client_Surname, Client_Phone, Client_Email FROM Clients ORDER BY Client_Surname, Client_Name;");

                foreach( var client in result)
                {
                    if( client[searchColumn].ToString().Contains(GetSearchValue()) )
                    {
                        InsertIntoClientsGrid(client[0], client[1], client[2], client[3]);
                    }
                }
            }).Start();
        }

        #endregion

        #region Open Overlays
        private void OpenAddClientOverlay()
        {
            AddClientOverlay clientOverlay = new AddClientOverlay();
            clientOverlay.OnExit += ClientOverlays_OnClose;
            clientOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            clientOverlay.Show();
        }

        private void OpenEditClientOverlay()
        {
            Overlays.Client.EditClientOverlay clientOverlay = new Overlays.Client.EditClientOverlay(GetSelectedEmail());
            clientOverlay.OnExit += ClientOverlays_OnClose;
            clientOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            clientOverlay.Show();
        }
        private void OpenEditPreferenceOverlay()
        {
            Overlays.Client.EditPreferenceOverlay preferenceOverlay = new Overlays.Client.EditPreferenceOverlay(GetSelectedEmail());
            preferenceOverlay.OnExit += ClientOverlays_OnClose;
            preferenceOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            preferenceOverlay.Show();
        }
        #endregion

        #region Get
        private string GetSelectedName()
        {
            string selectedName = "";

            this.Dispatcher.Invoke(() =>
            {
                selectedName = (DG_Clients.SelectedItem as GridViewSources.Client).Name;
            });

            return selectedName;
        }

        private string GetSelectedSurname()
        {
            string selectedSurname = "";

            this.Dispatcher.Invoke(() =>
            {
                selectedSurname = (DG_Clients.SelectedItem as GridViewSources.Client).Surname;
            });

            return selectedSurname;
        }

        private string GetSelectedEmail()
        {
            string selectedEmail = "";

            this.Dispatcher.Invoke(() =>
            {
                selectedEmail = (DG_Clients.SelectedItem as GridViewSources.Client).Email;
            });

            return selectedEmail;
        }

        private int GetSelectedSearchField()
        {
            int searchField = 0;

            this.Dispatcher.Invoke(() =>
                {
                    searchField = CB_SearchField.SelectedIndex;
                });

            return searchField;
        }

        private string GetSearchValue()
        {
            string searchValue = "";

            this.Dispatcher.Invoke(() =>
                {
                    searchValue = TB_SearchValue.Text;
                });

            return searchValue;
        }
        #endregion

        private void DeleteClient()
        {
            new System.Threading.Thread(() =>
            {
                Classes.ClientManager clientManager = new Classes.ClientManager();

                if (clientManager.DeleteClient(GetSelectedEmail()))
                {
                    DisplayNotifyBox("Deleted", "The record has been deleted", 3);
                }
                else
                {
                    DisplayNotifyBox("Not Deleted", GetSelectedName() + " " + GetSelectedSurname() + " has not been deleted", 3);
                }

            }).Start();
        }
       

        
       

        

        

    }
}
