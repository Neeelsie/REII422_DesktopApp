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

namespace RealEstate.Windows
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow
    {
        public string currentAdmin { get; private set; }

        public AdminWindow(string username)
        {
            currentAdmin = username;            
            InitializeComponent();
            TI_AdminView.Tag = this;
        }

        private void RE_AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.Title + currentAdmin;
        }

        private void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void RE_AdminWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenMainWindow();
        }
    }
}
