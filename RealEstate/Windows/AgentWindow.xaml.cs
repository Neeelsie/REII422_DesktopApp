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

namespace RealEstate.Windows
{
    /// <summary>
    /// Interaction logic for AgentWindow.xaml
    /// </summary>
    public partial class AgentWindow
    {
        string agentName = "";

        public AgentWindow(string agent)
        {
            InitializeComponent();
            agentName = agent;
            this.Title = this.Title + agent;
            AV_AddProperty.Tag = this;
            AV_MangeClients.Tag = this;
        }

        private void RE_AgentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenMainWindow();
        }

        private void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }


        private void BT_AddProperty_Click(object sender, RoutedEventArgs e)
        {
            ShowAddPropertyView();
        }

        private void BT_ManageCustomers_Click(object sender, RoutedEventArgs e)
        {
            ShowManageClientsView();
        }
        private void ShowAddPropertyView()
        {
            HideButtons();
            AV_AddProperty.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowManageClientsView()
        {
            HideButtons();
            AV_MangeClients.Visibility = System.Windows.Visibility.Visible;
        }

        private void HideButtons()
        {
            BT_AddProperty.Visibility = System.Windows.Visibility.Hidden;
            BT_ManageCustomers.Visibility = System.Windows.Visibility.Hidden;
            BT_PriceEstimator.Visibility = System.Windows.Visibility.Hidden;
            BT_Manage.Visibility = System.Windows.Visibility.Hidden;
        }

        private void ShowButtons()
        {
            BT_AddProperty.Visibility = System.Windows.Visibility.Visible;
            BT_ManageCustomers.Visibility = System.Windows.Visibility.Visible;
            BT_PriceEstimator.Visibility = System.Windows.Visibility.Visible;
            BT_Manage.Visibility = System.Windows.Visibility.Visible;
        }

        
    }
}
