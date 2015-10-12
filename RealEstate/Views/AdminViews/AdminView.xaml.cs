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
using Framework.UI.Controls;
using RealEstate.Windows;

namespace RealEstate.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView
    {
        public AdminView()
        {
            InitializeComponent();
        }

        private void RE_AdminView_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshAdmins();
        }

        private void BT_AddAdmin_Click(object sender, RoutedEventArgs e)
        {
            OpenAddAdminOverlay();
        }

        private void BT_EditAdmin_Click(object sender, RoutedEventArgs e)
        {
            if( DG_Admin.SelectedIndex != -1)
            {
                OpenEditAdminOverlay();
            }
            else
            {
                NotifyBox.Show(null, "ERROR", "No admin selected to edit", new TimeSpan(0, 0, 2), false);
            }
        }

        private async void BT_DeleteAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (DG_Admin.SelectedIndex != -1)
            {
                MessageBoxResult result = await MessageDialog.ShowAsync("Are you Sure?", "Are you sure you want to delete " + GetSelectedAdmin(), MessageBoxButton.YesNo, MessageDialogType.Light);

                if( result == MessageBoxResult.Yes )
                {
                    DeleteAdmin();
                }

                RefreshAdmins();
            }
            else
            {
                NotifyBox.Show(null, "ERROR", "No admin selected to delete", new TimeSpan(0, 0, 2), false);
            }
        }

        private void BT_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshAdmins();
        }

        private void AdminOverlays_OnClose(object sender, EventArgs e)
        {
            RefreshAdmins();
        }

        private void DG_Admin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if( DG_Admin.SelectedIndex != -1)
            {
                TB_SelectedAdmin.Text = (DG_Admin.SelectedItem as GridViewSources.Admin).Name;
            }
            else
            {
                TB_SelectedAdmin.Text = "";
            }
        }

        private void RefreshAdmins()
        {
            new System.Threading.Thread(() =>
                {
                    Classes.DatabaseManager dbManager = new Classes.DatabaseManager();

                    SetSelectedAdmin("");

                    var adminNames = dbManager.ReturnQuery("SELECT Admin_Username FROM Admin;");

                    ClearAdminGrid();

                    foreach( var name in adminNames )
                    {
                        if( name[0] != GetCurrentAdmin() )
                            InsertIntoAdminGrid(name[0]);
                    }

                }).Start();
        }

        #region Form Features Control
        private void SetSelectedAdmin(string selectedAdmin)
        {
            this.Dispatcher.Invoke(() =>
                {
                    TB_SelectedAdmin.Text = selectedAdmin;
                });
        }

        private void InsertIntoAdminGrid(string adminToAdd)
        {
            this.Dispatcher.Invoke(() =>
                {
                    DG_Admin.Items.Add(new GridViewSources.Admin() { Name = adminToAdd });
                });
        }

        private void ClearAdminGrid()
        {
            this.Dispatcher.Invoke(() =>
                {
                    DG_Admin.Items.Clear();
                });
        }

        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if( loading )
                    {
                        BT_AddAdmin.IsEnabled = false;
                        BT_EditAdmin.IsEnabled = false;
                        BT_DeleteAdmin.IsEnabled = false;
                        BT_Refresh.IsEnabled = false;
                        DG_Admin.IsEnabled = false;
                        (this.Tag as Windows.AdminWindow).TaskbarProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
                        (this.Tag as Windows.AdminWindow).TaskbarIsBusy = true;
                    }
                    else
                    {
                        BT_AddAdmin.IsEnabled = true;
                        BT_EditAdmin.IsEnabled = true;
                        BT_DeleteAdmin.IsEnabled = true;
                        BT_Refresh.IsEnabled = true;
                        DG_Admin.IsEnabled = true;
                        (this.Tag as Windows.AdminWindow).TaskbarProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                        (this.Tag as Windows.AdminWindow).TaskbarIsBusy = false;
                    }
                });
        }

        private void DisplayNotifyBox(string title, string message, int duration)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, duration), false);
            });
        }

        #endregion

        #region Get
        private string GetSelectedAdmin()
        {
            string selectedAdmin = "";

            this.Dispatcher.Invoke(() =>
                {
                    selectedAdmin = TB_SelectedAdmin.Text;
                });

            return selectedAdmin;
        }

        private string GetCurrentAdmin()
        {
            string currentAdmin = "";

            this.Dispatcher.Invoke(() =>
                {
                    currentAdmin = (this.Tag as AdminWindow).currentAdmin;
                });

            return currentAdmin;
        }
        #endregion

        #region Open Overlays
        private void OpenAddAdminOverlay()
        {
            Overlays.Admin.AddAdminOverlay addAdmin = new Overlays.Admin.AddAdminOverlay();
            addAdmin.OnExit += AdminOverlays_OnClose;
            addAdmin.Owner = Framework.UI.Controls.Window.GetWindow(this);
            addAdmin.Show();
        }

        private void OpenEditAdminOverlay()
        {
            Overlays.Admin.EditAdminOverlay editAdmin = new Overlays.Admin.EditAdminOverlay(TB_SelectedAdmin.Text);
            editAdmin.OnExit += AdminOverlays_OnClose;
            editAdmin.Owner = Framework.UI.Controls.Window.GetWindow(this);
            editAdmin.Show();
        }
        #endregion

        #region Delete
        private void DeleteAdmin()
        {
            new System.Threading.Thread(() =>
                {
                    Classes.AdminManager adminManager = new Classes.AdminManager();

                    if( adminManager.DeleteAdmin(GetSelectedAdmin()) )
                    {
                        DisplayNotifyBox("Deleted", GetSelectedAdmin() + " has been deleted",3);
                    }
                    else
                    {
                        DisplayNotifyBox("Not Deleted", GetSelectedAdmin() + " has not been deleted",3);
                    }

                }).Start();
        }


    }
}


        #endregion