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
using RealEstate.Windows;

namespace RealEstate.Views.AgentViews
{
    /// <summary>
    /// Interaction logic for AddPropertyView.xaml
    /// </summary>
    public partial class AddPropertyView
    {
        public AddPropertyView()
        {
            InitializeComponent();
        }

        private void ShowAgentWindow()
        {
            (this.Tag as AgentWindow).AV_Listings.Visibility = System.Windows.Visibility.Hidden;
            (this.Tag as AgentWindow).BT_ManageCustomers.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Listings.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Manage.Visibility = System.Windows.Visibility.Visible;
            (this.Tag as AgentWindow).BT_Properties.Visibility = System.Windows.Visibility.Visible;
        }

        private void BT_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAgentWindow();
        }
    }
}
