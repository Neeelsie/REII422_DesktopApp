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
using RealEstate.BingMaps.ViewModels;

namespace RealEstate.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for CitiesView.xaml
    /// </summary>
    public partial class CitiesView
    {
        string selectedCity = "";
        string selectedProvince = "";

        public CitiesView()
        {
            InitializeComponent();
            DataContext = new GeocodeViewModel();
        }

        private void BT_AddCity_Click(object sender, RoutedEventArgs e)
        {
            OpenAddCityOverlay();
        }

        private void BT_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshCities();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshCities();
        }

        private void CityOverlays_OnExit(object sender, EventArgs e)
        {
            RefreshCities();
        }

        #region Form Control
        private void RefreshCities()
        {
            new System.Threading.Thread(() =>
                {
                    ClearCities();
                    Classes.DatabaseManager dbManager = new Classes.DatabaseManager();

                    var cities = dbManager.ReturnQuery("SELECT City_Name, Province_Name FROM City,Province WHERE Province_ID = City_Province_ID");

                    foreach( var city in cities)
                    {
                        InserCityIntoDataGrid(city[0], city[1]);
                    }
                }).Start();
        }

        private void ClearCities()
        {
            this.Dispatcher.Invoke(() =>
                {
                    DG_Cities.Items.Clear();
                });
        }

        private void InserCityIntoDataGrid(string cityName, string provinceName)
        {
            this.Dispatcher.Invoke(() =>
                {
                    DG_Cities.Items.Add(new GridViewSources.City() { Name = cityName, Province = provinceName });
                });
        }

        private void UpdateMap()
        {
            new System.Threading.Thread(() =>
                {
                    SetMapLocation(selectedCity + " " + selectedProvince + " South Africa");
                }).Start();
        }

        private void SetMapLocation(string location)
        {
            this.Dispatcher.Invoke(() =>
            {
                (DataContext as GeocodeViewModel).GeocodeAddress(location);
                M_Cities.ZoomLevel = 13;
            });
        }

        private void ResetMap()
        {
            SetMapLocation("");
            M_Cities.ZoomLevel = 6;
        }
        #endregion

        #region Open Overlays
        private void OpenAddCityOverlay()
        {
            Overlays.Location.AddCityOverlay addCity = new Overlays.Location.AddCityOverlay();
            addCity.OnExit += CityOverlays_OnExit;
            addCity.Owner = Framework.UI.Controls.Window.GetWindow(this);
            addCity.Show();
        }

        private void OpenEditCityOverlay()
        {
            Overlays.Location.EditCityOverlay editCity = new Overlays.Location.EditCityOverlay(selectedCity, selectedProvince);
            editCity.OnExit += CityOverlays_OnExit;
            editCity.Owner = Framework.UI.Controls.Window.GetWindow(this);
            editCity.Show();
        }
        #endregion

        private void DG_Cities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if( DG_Cities.SelectedIndex != -1)
            {
                selectedCity = (DG_Cities.SelectedItem as GridViewSources.City).Name;
                selectedProvince = (DG_Cities.SelectedItem as GridViewSources.City).Province;
                TB_SelectedCity.Text = selectedCity + "," + selectedProvince;
                UpdateMap();
            }
            else
            {
                selectedProvince = "";
                selectedCity = "";
                TB_SelectedCity.Text = "";
                ResetMap();
            }
        }

        private void BT_EditCity_Click(object sender, RoutedEventArgs e)
        {
            if( DG_Cities.SelectedIndex != -1)
            {
                OpenEditCityOverlay();
            }
            else
            {
                NotifyBox.Show(null, "Cannot edit", "Please select a city to edit", new TimeSpan(0, 0, 2), false);
            }
        }

        #region Get
        
        #endregion


    }
}
