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
    /// Interaction logic for ProvincesView.xaml
    /// </summary>
    public partial class ProvincesView
    {
        

        public ProvincesView()
        {
            InitializeComponent();
            DataContext = new GeocodeViewModel();
        }

        private void BT_AddProvince_Click(object sender, RoutedEventArgs e)
        {
            OpenAddProvinceOverlay();
        }

        private void BT_EditProvince_Click(object sender, RoutedEventArgs e)
        {
            OpenEditProvinceOverlay();
        }

        private void BT_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshProvinces();
        }

        private void RE_Provinces_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshProvinces();
        }

        private void ProvinceOverlays_OnClose(object sender, EventArgs e)
        {
            RefreshProvinces();
        }

        private void DG_Provinces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DG_Provinces.SelectedIndex != -1)
            {
                TB_SelectedProvince.Text = (DG_Provinces.SelectedItem as GridViewSources.Province).Name;
                UpdateMap();
            }
            else
            {
                TB_SelectedProvince.Text = "";
                ResetMap();
            }
        }

        #region Open Overlays
        private void OpenAddProvinceOverlay()
        {
            Overlays.Location.AddProvinceOverlay provinceOverlay = new Overlays.Location.AddProvinceOverlay();
            provinceOverlay.OnExit += ProvinceOverlays_OnClose;
            provinceOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            provinceOverlay.Show();
        }

        private void OpenEditProvinceOverlay()
        {
            Overlays.Location.EditProvinceOverlay editProvince = new Overlays.Location.EditProvinceOverlay(GetSelectedProvince());
            editProvince.OnExit += ProvinceOverlays_OnClose;
            editProvince.Owner = Framework.UI.Controls.Window.GetWindow(this);
            editProvince.Show();
        }

        private void UpdateMap()
        {
            new System.Threading.Thread(() =>
                {
                    SetMapLocation(GetSelectedProvince() + " South Africa");
                }).Start();
        }

        private void SetMapLocation(string location)
        {
            this.Dispatcher.Invoke(() =>
                {
                    (DataContext as GeocodeViewModel).GeocodeAddress(location);
                    M_Provinces.ZoomLevel = 7;
                });
        }

        private void ResetMap()
        {
            SetMapLocation("");
            M_Provinces.ZoomLevel = 6;
        }
        #endregion

        #region Form Control
        private void RefreshProvinces()
        {
            new System.Threading.Thread(() =>
                {
                    ClearProvincesGrid();
                    Classes.DatabaseManager dbManager = new Classes.DatabaseManager();
                    var provinceNames = dbManager.ReturnQuery("SELECT Province_Name FROM Province ORDER BY Province_Name;");                  

                    foreach( var province in provinceNames)
                    {
                        InsertIntoProvincesGrid(province[0]);
                    }

                }).Start();
        }

        private void ClearProvincesGrid()
        {
            this.Dispatcher.Invoke(() =>
                {
                    DG_Provinces.Items.Clear();
                });
        }

        private void InsertIntoProvincesGrid(string provinceToAdd)
        {
            this.Dispatcher.Invoke(() =>
                {
                    DG_Provinces.Items.Add(new GridViewSources.Province() { Name = provinceToAdd });
                });
            //DG_Admin.Items.Add(new GridViewSources.Admin() { Name = adminToAdd });
        }

        #endregion

        #region Get

        private string GetSelectedProvince()
        {
            string selectedProvince = "";

            this.Dispatcher.Invoke(() =>
                {
                    selectedProvince = TB_SelectedProvince.Text;
                });

            return selectedProvince;
        }

        #endregion







    }
}
