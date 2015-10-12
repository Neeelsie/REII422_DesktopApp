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

namespace RealEstate.Overlays.Admin
{
    /// <summary>
    /// Interaction logic for EditAdminOverlay.xaml
    /// </summary>
    public partial class EditAdminOverlay
    {
        string adminToEdit = "";
        public event EventHandler OnExit;

        public EditAdminOverlay(string admin)
        {
            adminToEdit = admin;
            InitializeComponent();
        }

        private void RE_EditAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Edit Admin : " + adminToEdit;
        }

        private void RE_EditAdmin_Closed(object sender, EventArgs e)
        {
            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        private void CB_ChangeUsername_Checked(object sender, RoutedEventArgs e)
        {
            EnableUsernameEdit();
        }

        private void CB_ChangePassword_Checked(object sender, RoutedEventArgs e)
        {
            EnablePasswordEdit();
        }

        private void BT_Edit_Click(object sender, RoutedEventArgs e)
        {
            if( CanEdit() )
            {
                if( (bool)CB_ChangeUsername.IsChecked )
                {
                    EditUsername();
                }
                
                if( (bool)CB_ChangePassword.IsChecked )
                {
                    EditPassword();
                }

                this.Close();
            }
        }

        #region Set Form Features
        private void EnableUsernameEdit()
        {
            CB_ChangeUsername.IsEnabled = false;
            CB_ChangeUsername.Visibility = System.Windows.Visibility.Hidden;
            TB_Username.Visibility = System.Windows.Visibility.Visible;
            TB_Username.IsEnabled = true;
            TBL_Username.Visibility = System.Windows.Visibility.Visible;
            TBL_Username.IsEnabled = true;
        }

        private void EnablePasswordEdit()
        {
            CB_ChangePassword.Visibility = System.Windows.Visibility.Hidden;
            CB_ChangePassword.IsEnabled = false;

            TBL_Password.Visibility = System.Windows.Visibility.Visible;
            PB_Password.Visibility = System.Windows.Visibility.Visible;
            PB_Password.IsEnabled = true;

            TBL_ConfirmPassword.Visibility = System.Windows.Visibility.Visible;
            PB_ConfirmPassword.Visibility = System.Windows.Visibility.Visible;
            PB_ConfirmPassword.IsEnabled = true;
        }

        private void DisplayNotifybox(string title, string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, 2), false);
            });
        }

        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (loading)
                {
                    RE_EditAdmin.IsBusy = true;
                    TB_Username.IsEnabled = false;
                    PB_Password.IsEnabled = false;
                    PB_ConfirmPassword.IsEnabled = false;
                    BT_Edit.IsEnabled = false;
                }
                else
                {
                    RE_EditAdmin.IsBusy = false;
                    TB_Username.IsEnabled = true;
                    PB_Password.IsEnabled = true;
                    PB_ConfirmPassword.IsEnabled = true;
                    BT_Edit.IsEnabled = true;
                }
            });
        }
        #endregion

        #region Get
        private string GetUsername()
        {
            string username = "";

            this.Dispatcher.Invoke(() =>
                {
                    username = TB_Username.Text;
                });

            return username;
        }
        private string GetPassword()
        {
            string password = "";

            this.Dispatcher.Invoke(() =>
                {
                    password = PB_ConfirmPassword.Password.ToString();
                });

            return password;
        }        
        #endregion

        #region Edit Admin
        private void EditUsername()
        {
            new System.Threading.Thread(() =>
                {
                    if( adminToEdit != "")
                    {
                        Classes.AdminManager adminManager = new Classes.AdminManager();

                        SetLoadingState(true);

                        if (adminManager.EditUsername(adminToEdit, GetUsername()))
                        {
                            DisplayNotifybox(adminToEdit + "Modified", adminToEdit + " username changed to " + GetUsername());
                        }
                        else
                        {
                            DisplayNotifybox("Could not edit " + adminToEdit, adminToEdit + " username was not changed");
                        }

                        SetLoadingState(false);                        
                    }
                }).Start();
        }

        private void EditPassword()
        {
            new System.Threading.Thread(() =>
                {
                    if( adminToEdit != "")
                    {
                        Classes.AdminManager adminManger = new Classes.AdminManager();

                        SetLoadingState(true);

                        if( adminManger.EditPassword(adminToEdit, GetPassword()))
                        {
                            DisplayNotifybox(adminToEdit + " Modified", adminToEdit + " password changed");
                        }
                        else
                        {
                            DisplayNotifybox("Could not edit " + adminToEdit, adminToEdit + " password was not changed");
                        }

                        SetLoadingState(false);
                    }

                }).Start();
        }

        private bool CanEdit()
        {
            bool canEdit = false;

            if( (bool)CB_ChangePassword.IsChecked && (bool)CB_ChangeUsername.IsChecked )
            {
                if( CanEditUsername() && CanEditPassword() )
                {
                    canEdit = true;
                }
            }
            else if((bool)CB_ChangeUsername.IsChecked )
            {
                if( CanEditUsername() )
                {
                    canEdit = true;
                }
            }
            else if((bool)CB_ChangePassword.IsChecked )
            {
                if( CanEditUsername() )
                {
                    canEdit = true;
                }
            }
            else
            {
                DisplayNotifybox("ERROR", "No changes selected");
            }

            return canEdit;
        }

        private bool CanEditUsername()
        {
            bool canEditUsername = false;

            if( ( (bool)CB_ChangeUsername.IsChecked) && (TB_Username.Text != "") )
            {
                canEditUsername = true;
            }
            else if (((bool)CB_ChangeUsername.IsChecked) && (TB_Username.Text == "") )
            {
                DisplayNotifybox("ERROR", "Username cannot be empty");
            }

            return canEditUsername;
        }

        private bool CanEditPassword()
        {
            bool canEditPassword = false;

            if( ((bool)CB_ChangePassword.IsChecked) && (PB_Password.Password.ToString() != ""))
            {
                if( PB_Password.Password.ToString() == PB_ConfirmPassword.Password.ToString() )
                {
                    canEditPassword = true;
                }
                else
                {
                    DisplayNotifybox("ERROR", "Password do not match");
                }
            }
            else if( ((bool)CB_ChangePassword.IsChecked) && (PB_Password.Password.ToString() == ""))
            {
                DisplayNotifybox("ERROR", "Password cannot be empty");
            }

            return canEditPassword;
        }
        #endregion

        private void BT_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Error Handeling
        
        #endregion


    }
}
