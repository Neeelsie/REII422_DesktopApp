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

namespace RealEstate.Overlays.Client
{
    /// <summary>
    /// Interaction logic for EditClientOverlay.xaml
    /// </summary>
    public partial class EditClientOverlay
    {
        string clientEmail = "";
        bool editName = false;
        bool editSurname = false;
        bool editPhone = false;
        bool editEmail = false;
        bool editPassword = false;
        bool edited = false;

        public event EventHandler OnExit;
        public EditClientOverlay(string email)
        {
            InitializeComponent();
            clientEmail = email;
            this.Title = "Edit " + clientEmail;
        }

        private void CB_ChangeName_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewName.Visibility = System.Windows.Visibility.Visible;
            TB_NewName.Visibility = System.Windows.Visibility.Visible;
            CB_ChangeName.Visibility = System.Windows.Visibility.Hidden;
            editName = true;
        }

        private void CB_ChangeSurname_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewSurname.Visibility = System.Windows.Visibility.Visible;
            TB_NewSurname.Visibility = System.Windows.Visibility.Visible;
            CB_ChangeSurname.Visibility = System.Windows.Visibility.Hidden;
            editSurname = true;
        }

        private void CB_ChangePhone_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewPhone.Visibility = System.Windows.Visibility.Visible;
            TB_NewPhone.Visibility = System.Windows.Visibility.Visible;
            CB_ChangePhone.Visibility = System.Windows.Visibility.Hidden;
            editPhone = true;
        }

        private void CB_ChangeEmail_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewEmail.Visibility = System.Windows.Visibility.Visible;
            TB_NewEmail.Visibility = System.Windows.Visibility.Visible;
            CB_ChangeEmail.Visibility = System.Windows.Visibility.Hidden;
            editEmail = true;
        }

        private void CB_ChangePassword_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewPassword.Visibility = System.Windows.Visibility.Visible;
            PB_NewPassword.Visibility = System.Windows.Visibility.Visible;
            TBL_ConfirmPassword.Visibility = System.Windows.Visibility.Visible;
            PB_ConfirmPassword.Visibility = System.Windows.Visibility.Visible;
            CB_ChangePassword.Visibility = System.Windows.Visibility.Hidden;
            editPassword = true;
        }

        private void RE_EditClient_Closed(object sender, EventArgs e)
        {
            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BT_SaveAndClose_Click(object sender, RoutedEventArgs e)
        {

            if (editName || editSurname || editPhone || editEmail || editPassword)
            {
                edited = true;
                if (editName)
                {
                    if (GetNewName() != "")
                    {
                        EditName(clientEmail, GetNewName());
                    }
                    else
                    {
                        edited = false;
                        DisplayNotifyBox("ERROR", "New name cannot be empty", 3);
                    }
                }

                if (editSurname)
                {
                    if (GetNewSurname() != "")
                    {
                        EditSurname(clientEmail, GetNewSurname());
                    }
                    else
                    {
                        edited = false;
                        DisplayNotifyBox("ERROR", "New surname cannot be empty", 3);
                    }
                }

                if (editPhone)
                {
                    if (GetNewPhone() != "")
                    {
                        EditPhone(clientEmail, GetNewPhone());
                    }
                    else
                    {
                        edited = false;
                        DisplayNotifyBox("ERROR", "New phone number cannot be empty", 3);
                    }
                }

                if (editEmail)
                {
                    if (GetNewEmail() != "")
                    {
                        EditEmail(clientEmail, GetNewEmail());
                    }
                    else
                    {
                        edited = false;
                        DisplayNotifyBox("ERROR", "New email cannot be empty", 3);
                    }
                }

                if (editPassword)
                {
                    if (GetNewName() != "" && CanEditPassword())
                    {
                        EditPassword(clientEmail, GetNewPassword());
                    }
                    else
                    {
                        edited = false;
                        DisplayNotifyBox("ERROR", "New name cannot be empty", 3);
                    }
                }

                if (edited)
                {
                    CloseForm();
                }
            }
            else
            {
                DisplayNotifyBox("No change selected", "Please select a property to change", 3);
            }
        }

        private void RE_EditCity_Closed(object sender, EventArgs e)
        {
            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        #region Form Control

        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (loading)
                {
                    CB_ChangeName.IsEnabled = false;
                    TB_NewName.IsEnabled = false;
                    CB_ChangeSurname.IsEnabled = false;
                    TB_NewSurname.IsEnabled = false;
                    CB_ChangePhone.IsEnabled = false;
                    TB_NewPhone.IsEnabled = false;
                    CB_ChangeEmail.IsEnabled = false;
                    TB_NewEmail.IsEnabled = false;
                    CB_ChangePassword.IsEnabled = false;
                    PB_NewPassword.IsEnabled = false;
                    PB_ConfirmPassword.IsEnabled = false;
                    BT_SaveAndClose.IsEnabled = false;
                    BT_Cancel.IsEnabled = false;
                }
                else
                {
                    CB_ChangeName.IsEnabled = true;
                    TB_NewName.IsEnabled = true;
                    CB_ChangeSurname.IsEnabled = true;
                    TB_NewSurname.IsEnabled = true;
                    CB_ChangePhone.IsEnabled = true;
                    TB_NewPhone.IsEnabled = true;
                    CB_ChangeEmail.IsEnabled = true;
                    TB_NewEmail.IsEnabled = true;
                    CB_ChangePassword.IsEnabled = true;
                    PB_NewPassword.IsEnabled = true;
                    PB_ConfirmPassword.IsEnabled = true;
                    BT_SaveAndClose.IsEnabled = true;
                    BT_Cancel.IsEnabled = true;
                }
            });
        }

        private void DisplayNotifyBox(string title, string message, int durationSeconds)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, durationSeconds), false);
            });
        }

        private void CloseForm()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Close();
            });
        }
        #endregion

        #region Get
        private string GetNewName()
        {
            string newName = "";

            this.Dispatcher.Invoke(() =>
            {
                newName = TB_NewName.Text;
            });

            return newName;
        }

        private string GetNewSurname()
        {
            string newSurname = "";

            this.Dispatcher.Invoke(() =>
            {
                newSurname = TB_NewSurname.Text;
            });

            return newSurname;
        }

        private string GetNewPhone()
        {
            string newPhone = "";

            this.Dispatcher.Invoke(() =>
            {
                newPhone = TB_NewPhone.Text;
            });

            return newPhone;
        }

        private string GetNewEmail()
        {
            string newEmail = "";

            this.Dispatcher.Invoke(() =>
            {
                newEmail = TB_NewEmail.Text;
            });

            return newEmail;
        }

        private string GetNewPassword()
        {
            string newPassword = "";

            this.Dispatcher.Invoke(() =>
            {
                newPassword = PB_NewPassword.Password.ToString();
            });

            return newPassword;
        }
        #endregion

        private bool CanEditPassword()
        {
            bool canEditPassword = false;

            if (((bool)CB_ChangePassword.IsChecked) && (PB_NewPassword.Password.ToString() != ""))
            {
                if (PB_NewPassword.Password.ToString() == PB_ConfirmPassword.Password.ToString())
                {
                    canEditPassword = true;
                }
                else
                {
                    DisplayNotifyBox("ERROR", "Password do not match", 3);
                }
            }
            else if (((bool)CB_ChangePassword.IsChecked) && (PB_NewPassword.Password.ToString() == ""))
            {
                DisplayNotifyBox("ERROR", "Password cannot be empty", 3);
            }

            return canEditPassword;
        }


        #region Edit

        private void EditName(string email, string name)
        {
            new System.Threading.Thread(() =>
            {
                SetLoadingState(true);

                Classes.ClientManager clientManager = new Classes.ClientManager();

                if (clientManager.EditName(email, name))
                {
                    DisplayNotifyBox("Edited", "Name changed to " + name, 5);
                }
                else
                {
                    edited = false;
                    DisplayNotifyBox("Could not edit", "En error occured while trying to change name. Please try again later", 5);
                }
                SetLoadingState(false);

            }).Start();
        }

        private void EditSurname(string email, string surname)
        {
            new System.Threading.Thread(() =>
            {
                SetLoadingState(true);

                Classes.ClientManager clientManager = new Classes.ClientManager();

                if (clientManager.EditSurname(email, surname))
                {
                    DisplayNotifyBox("Edited", "Surname changed to " + surname, 5);
                }
                else
                {
                    edited = false;
                    DisplayNotifyBox("Could not edit", "En error occured while trying to change surname. Please try again later", 5);
                }
                SetLoadingState(false);

            }).Start();
        }

        private void EditPhone(string email, string phone)
        {
            new System.Threading.Thread(() =>
            {
                SetLoadingState(true);

                Classes.ClientManager clientManager = new Classes.ClientManager();

                if (clientManager.EditPhone(email, phone))
                {
                    DisplayNotifyBox("Edited", "Phone number changed to " + phone, 5);
                }
                else
                {
                    edited = false;
                    DisplayNotifyBox("Could not edit", "En error occured while trying to change phone number. Please try again later", 5);
                }
                SetLoadingState(false);

            }).Start();
        }

        private void EditEmail(string email, string newEmail)
        {
            Classes.ClientManager clientManager = new Classes.ClientManager();

            if (clientManager.CanAddClient(newEmail))
            {
                new System.Threading.Thread(() =>
                {
                    SetLoadingState(true);

                    if (clientManager.EditEmail(email, newEmail))
                    {
                        DisplayNotifyBox("Edited", "Email changed to " + newEmail, 5);
                    }
                    else
                    {
                        edited = false;
                        DisplayNotifyBox("Could not edit", "En error occured while trying to change email. Please try again later", 5);
                    }
                    SetLoadingState(false);

                }).Start();
            }
            else
            {
                edited = false;
                DisplayNotifyBox("Could not edit", "Cannot change " + email + " to " + newEmail + " because " + newEmail + " already exists", 4);
            }
        }

        private void EditPassword(string email, string password)
        {
            new System.Threading.Thread(() =>
            {
                SetLoadingState(true);

                Classes.ClientManager clientManager = new Classes.ClientManager();

                if (clientManager.EditPassword(email, password))
                {
                    DisplayNotifyBox("Edited", "Password changed to " + password, 5);
                }
                else
                {
                    edited = false;
                    DisplayNotifyBox("Could not edit", "En error occured while trying to change password. Please try again later", 5);
                }
                SetLoadingState(false);
            }).Start();
        }

        #endregion
    }
}