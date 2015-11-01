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
using Framework.UI.Controls;

namespace RealEstate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int CONNECTING = 1;
        private const int CONNECTED = 2;
        private const int NOT_CONNECTED = 3;

        bool connectionWorking = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RE_MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TestConnection();
        }

        private void BT_SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (BT_SignIn.Content.ToString() == "Sign In")
            {
                StartSession();
            }
            else
            {
                TestConnection();
            }
        }

        private void B_ServerStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenServerSettingsOverlay();
        }

        private void serverSettings_OnSave(object sender, EventArgs e)
        {
            TestConnection();
        }

        #region Open Windows
        private void OpenAdminWindow(string username)
        {
            this.Dispatcher.Invoke(() =>
                {
                    Windows.AdminWindow adminWindow = new Windows.AdminWindow(GetUsername());
                    adminWindow.Show();
                    this.Close();
                });
        }

        private void OpenAgentWindow(string username)
        {
            this.Dispatcher.Invoke(() =>
                {
                    Windows.AgentWindow agentWindow = new Windows.AgentWindow(username);
                    agentWindow.Show();
                    this.Close();
                });
        }
        #endregion

        #region Open Overlays
        private void OpenServerSettingsOverlay()
        {
            Overlays.ServerSettingsOverlay serverOverlay = new Overlays.ServerSettingsOverlay();
            serverOverlay.OnExit += serverSettings_OnSave;
            serverOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            serverOverlay.Show();

        }
        #endregion

        #region Get Methods
        private string GetPassword()
        {
            string password = "";

            this.Dispatcher.Invoke(() =>
            {
                password = PB_Password.Password.ToString();
            });

            return password;
        }

        private string GetUsername()
        {
            string username = "";

            this.Dispatcher.Invoke(() =>
            {
                username = TB_Username.Text;
            });

            return username;
        }

        #endregion

        private void TestConnection()
        {
            SetServerStatus(CONNECTING);
            SetTaskbarState(CONNECTING);
            SetLoadingStatus(true);
            DisplayNotifyBox("Connecting", "Please wait while the application attempts to connect to the database");

            new System.Threading.Thread(() =>
            {
                Classes.DatabaseManager dbManger = new Classes.DatabaseManager();

                connectionWorking = dbManger.ConnectionWorking();

                if (connectionWorking)
                {
                    SetServerStatus(CONNECTED);
                    SetTaskbarState(CONNECTED);
                    DisplayNotifyBox("Connected", "Successfully connected to the database");
                }
                else
                {
                    SetServerStatus(NOT_CONNECTED);
                    SetTaskbarState(NOT_CONNECTED);
                    DisplayNotifyBox("Not connected", "Could not connect to the database. Please check your internet connection");
                }

                SetFormState(connectionWorking);

                SetLoadingStatus(false);
            }).Start();

        }

        #region Set Form Features
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

        private void SetTaskbarState(int taskbarState)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (taskbarState == CONNECTING)
                {
                    RE_MainWindow.TaskbarProgressState = System.Windows.Shell.TaskbarItemProgressState.Paused;
                    RE_MainWindow.TaskbarProgressValue = 1;
                }
                else if (taskbarState == CONNECTED)
                {
                    RE_MainWindow.TaskbarProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    RE_MainWindow.TaskbarProgressValue = 0;
                }
                else if (taskbarState == NOT_CONNECTED)
                {
                    RE_MainWindow.TaskbarProgressState = System.Windows.Shell.TaskbarItemProgressState.Error;
                }
            });
        }

        private void DisplayNotifyBox(string title, string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, 1), false);
            });
        }

        private void SetLoadingStatus(bool loading)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (loading)
                {
                    G_Loading.Visibility = System.Windows.Visibility.Visible;
                    G_LoginPanel.Visibility = System.Windows.Visibility.Hidden;
                    LC_Loading.IsContentLoaded = false;
                }
                else
                {
                    G_LoginPanel.Visibility = System.Windows.Visibility.Visible;
                    G_Loading.Visibility = System.Windows.Visibility.Hidden;
                    LC_Loading.IsContentLoaded = true;
                }
            });
        }

        private void SetFormState(bool connected)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if (!connected)
                    {
                        BT_SignIn.Content = "Retry Connection";
                        TB_Username.IsEnabled = false;
                        PB_Password.IsEnabled = false;
                    }
                    else
                    {
                        BT_SignIn.Content = "Sign In";
                        TB_Username.IsEnabled = true;
                        PB_Password.IsEnabled = true;
                    }
                });
        }
        #endregion

        #region Session
        private void StartSession()
        {
            new System.Threading.Thread(() =>
            {
                SetLoadingStatus(true);
                DisplayNotifyBox("Attempting to Sign In", "Please wait while we sign you in");

                string passsword = PB_Password.Password.ToString();

                Classes.Session session = new Classes.Session(GetUsername(), passsword);

                if (session.LoginSuccessfull)
                {
                    DisplayNotifyBox("Signed In", "Welcome " + session.username);

                    if( session.IsAdmin )
                    {
                        OpenAdminWindow(session.username);
                    }
                    else
                    {
                        OpenAgentWindow(session.username);
                    }

                    
                }
                else
                {
                    DisplayNotifyBox("Invalid Username or Password", "Could not sign in");
                }

                SetLoadingStatus(false);
            }).Start();
        }
        #endregion

    }
}
