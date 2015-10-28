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

namespace RealEstate.Overlays.Listings
{
    /// <summary>
    /// Interaction logic for AddListingOverlay.xaml
    /// </summary>
    public partial class AddListingOverlay 
    {
        public event EventHandler OnExit;

        public AddListingOverlay(string currentAgent)
        {
            InitializeComponent();
        }


        private void CB_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CB_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CB_City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CB_Province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CB_Complex_Checked(object sender, RoutedEventArgs e)
        {
            TBL_ComplexName.Visibility = System.Windows.Visibility.Visible;
            TB_ComplexName.Visibility = System.Windows.Visibility.Visible;
            TBL_ComplexNo.Visibility = System.Windows.Visibility.Visible;
            TB_ComplexNo.Visibility = System.Windows.Visibility.Visible;
        }

        private void CB_Complex_Unchecked(object sender, RoutedEventArgs e)
        {
            TBL_ComplexName.Visibility = System.Windows.Visibility.Hidden;
            TB_ComplexName.Visibility = System.Windows.Visibility.Hidden;
            TBL_ComplexNo.Visibility = System.Windows.Visibility.Hidden;
            TB_ComplexNo.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
