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

namespace RealEstate.Overlays
{
    /// <summary>
    /// Interaction logic for ServerSettingsOverlay.xaml
    /// </summary>
    public partial class ServerSettingsOverlay
    {
        private const int CONNECTING = 1;
        private const int CONNECTED = 2;
        private const int NOT_CONNECTED = 3;

        private bool settingsSaved = false;
        public event EventHandler OnExit;

        public ServerSettingsOverlay()
        {
            InitializeComponent();
        }

        private void RE_ServerSettingsOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        private async void RE_ServerSettingsOverlay_Closed(object sender, EventArgs e)
        {
            if (!settingsSaved)
            {
                MessageBoxResult result = await MessageDialog.ShowAsync("Settings not saved", "Server settings will not be changed", MessageBoxButton.OK, MessageDialogType.Light);
            }

            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        private void B_ServerStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TestSettings();
        }

        private void BT_SaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        #region Validation
        private void IPOctetValidation(object sender, TextCompositionEventArgs e)
        {
            bool eHandled = true;
            TextBox temp = (TextBox)sender;
            string temp_text;
            Classes.Validation validation = new Classes.Validation();

            if( validation.IsTextNumeric(e.Text) )
            {
                temp_text = temp.Text.ToString() + e.Text;
                eHandled = !validation.IsNumberInRange(0, 255, temp_text);
            }

            e.Handled = eHandled;
        }

        private void PortValidation(object sender, TextCompositionEventArgs e)
        {
            Classes.Validation validation = new Classes.Validation();
            e.Handled = !validation.IsTextNumeric(e.Text);
        }
        #endregion

        #region Load Settings
        private void LoadSettings()
        {
            Classes.ConfigManager configManager = new Classes.ConfigManager();

            if (configManager.ConfigLoaded)
            {
               if( configManager.ServerIP != null)
               { 
                    TB_Username.Text = configManager.ServerUser;
                    TB_Database.Text = configManager.ServerDB;
                    TB_Port.Text = configManager.ServerPort;

                    string tempIP = configManager.ServerIP;

                    TextBox[] tempIpOctets = new TextBox[4] { TB_IPOctet1, TB_IPOctet2, TB_IPOctet3, TB_IPOctet4 };

                    int octetCounter = 0;

                    for (int i = 0; i < tempIP.Count(); i++)
                    {
                        if (tempIP[i] == '.')
                        {
                            octetCounter++;
                        }
                        else
                        {
                            tempIpOctets[octetCounter].Text = tempIpOctets[octetCounter].Text + tempIP[i];
                        }
                    }
               }
            }
        }
        #endregion

        #region Form Features Control
        private void DisplayNotifyBox(string title, string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, 1), false);
            });
        }

        private void SetTestingState(bool testing)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (testing)
                {
                    RE_ServerSettingsOverlay.IsBusy = true;
                    TB_IPOctet1.IsEnabled = false;
                    TB_IPOctet2.IsEnabled = false;
                    TB_IPOctet3.IsEnabled = false;
                    TB_IPOctet4.IsEnabled = false;
                    TB_Port.IsEnabled = false;
                    TB_Username.IsEnabled = false;
                    PB_Password.IsEnabled = false;
                    TB_Database.IsEnabled = false;
                    BT_SaveAndClose.IsEnabled = false;
                }
                else
                {
                    RE_ServerSettingsOverlay.IsBusy = false;
                    TB_IPOctet1.IsEnabled = true;
                    TB_IPOctet2.IsEnabled = true;
                    TB_IPOctet3.IsEnabled = true;
                    TB_IPOctet4.IsEnabled = true;
                    TB_Port.IsEnabled = true;
                    TB_Username.IsEnabled = true;
                    PB_Password.IsEnabled = true;
                    TB_Database.IsEnabled = true;
                    BT_SaveAndClose.IsEnabled = true;
                }
            });
        }

        private void SetServerStatus(int serverStatus)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (serverStatus == CONNECTING)
                {
                    B_ServerStatus.Background = Brushes.Orange;
                    B_ServerStatus.BorderBrush = Brushes.PaleGoldenrod;
                }
                else if (serverStatus == CONNECTED)
                {
                    B_ServerStatus.Background = Brushes.LightGreen;
                    B_ServerStatus.BorderBrush = Brushes.Green;
                }
                else if (serverStatus == NOT_CONNECTED)
                {
                    B_ServerStatus.Background = Brushes.Red;
                    B_ServerStatus.BorderBrush = Brushes.DarkRed;
                }
            });
        }
        #endregion

        #region Settings Test
        private void TestSettings()
        {
            DisplayNotifyBox("Testing", "Please wait while the settings are tested");
            SetTestingState(true);
            SetServerStatus(CONNECTING);

            new System.Threading.Thread(() =>
            {
                Classes.DatabaseManager dbManger = new Classes.DatabaseManager();

                if (dbManger.TestConnectionSettings(IpAddress(), Username(), Password(), Database(), Port()))
                {
                    SetServerStatus(CONNECTED);
                    DisplayNotifyBox("Connected", "Successfully connected to the server");
                }
                else
                {
                    SetServerStatus(NOT_CONNECTED);
                    DisplayNotifyBox("Not Connected", "Could not connected to the server");
                }

                SetTestingState(false);

            }).Start();
        }
        #endregion

        #region Save Settings
        private void SaveSettings()
        {
            Classes.ConfigManager configManger = new Classes.ConfigManager();
            settingsSaved = configManger.SaveDBConfig(IpAddress(), Username(), Password(), Database(), Port());
        }
        #endregion

        #region Get Values
        private string IpAddress()
        {
            string ip = "";
            this.Dispatcher.Invoke(() =>
            {
                ip = TB_IPOctet1.Text.ToString() + "." + TB_IPOctet2.Text.ToString() + "." + TB_IPOctet3.Text.ToString() + "." + TB_IPOctet4.Text.ToString();
            });
            return ip;
        }

        private string Username()
        {
            string username = "";

            this.Dispatcher.Invoke(() =>
            {
                username = TB_Username.Text.ToString();
            });

            return username;
        }

        private string Password()
        {
            string password = "";

            this.Dispatcher.Invoke(() =>
            {
                password = PB_Password.Password.ToString();
            });

            return password;
        }

        private string Database()
        {
            string database = "";

            this.Dispatcher.Invoke(() =>
            {
                database = TB_Database.Text.ToString();
            });

            return database;
        }

        private string Port()
        {
            string port = "";

            this.Dispatcher.Invoke(() =>
            {
                port = TB_Port.Text.ToString();
            });

            return port;
        }
        #endregion

        

        
    }
}
