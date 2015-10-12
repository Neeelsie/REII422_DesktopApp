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
    /// Interaction logic for AreaView.xaml
    /// </summary>
    public partial class AreaView
    {
        string selectedArea = "";
        string selectedCity = "";
        string selectedProvince = "";

        public AreaView()
        {
            InitializeComponent();
            DataContext = new GeocodeViewModel();
        }

        private void BT_AddArea_Click(object sender, RoutedEventArgs e)
        {
            OpenAddAreaOverlay();
        }

        private void BT_EditArea_Click(object sender, RoutedEventArgs e)
        {
            if( DG_Areas.SelectedIndex != -1)
            {
                OpenEditAreaOverlay();
            }
            else
            {
                DisplayNotifyBox("Cannot Edit", "Please select a area to edit", 3);
            }
        }

        private void AreaOverlays_OnExit(object sender, EventArgs e)
        {
            RefreshAreas();
        }

        private void RE_AreaView_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshAreas();
        }

        private void DG_Areas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if( DG_Areas.SelectedIndex != -1)
            {
                selectedArea = (DG_Areas.SelectedItem as GridViewSources.Area).Name;
                selectedCity = (DG_Areas.SelectedItem as GridViewSources.Area).City;
                selectedProvince = (DG_Areas.SelectedItem as GridViewSources.Area).Province;

                TB_SelectedArea.Text = selectedArea + "," + selectedCity + "," + selectedProvince;

                UpdateMap();
            }
            else
            {
                TB_SelectedArea.Text = "";
                ResetMap();
            }
        }

        private void BT_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshAreas();
        }

        private void BT_DeleteArea_Click(object sender, RoutedEventArgs e)
        {

        }


        #region Form Controld
        private void RefreshAreas()
        {
            new System.Threading.Thread(() =>
            {
                ClearAreaDataGrid();

                Classes.DatabaseManager dbManger = new Classes.DatabaseManager();

                var results = dbManger.ReturnQuery("SELECT Area_Name, City_Name , Province_Name FROM Area,City,Province WHERE Area_City_ID = City_ID AND City_Province_ID = Province_ID ORDER BY Province_Name,City_Name,Area_Name;");

                foreach (var area in results)
                {
                    InsertAreaIntoDataGrid(area[0], area[1], area[2]);
                }

            }).Start();
        }

        private void InsertAreaIntoDataGrid(string areaName, string cityName, string provinceName)
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Areas.Items.Add(new GridViewSources.Area() { Name = areaName, City = cityName, Province = provinceName });
            });
        }

        private void ClearAreaDataGrid()
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Areas.Items.Clear();
            });
        }

        private void DisplayNotifyBox(string title, string message, int durationSeconds)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, durationSeconds), false);
            });
        }

        private void UpdateMap()
        {
            new System.Threading.Thread(() =>
                {
                    SetMapLocation(selectedArea + " " + selectedCity + " " + selectedProvince + " South Africa");
                }).Start();
        }

        private void SetMapLocation(string location)
        {
            this.Dispatcher.Invoke(() =>
                {
                    (DataContext as GeocodeViewModel).GeocodeAddress(location);
                    M_Areas.ZoomLevel = 16;
                });
        }

        private void ResetMap()
        {
            SetMapLocation("");
            M_Areas.ZoomLevel = 6;
        }
        #endregion

        #region Open Overlays

        private void OpenAddAreaOverlay()
        {
            Overlays.Location.AddAreaOverlay addArea = new Overlays.Location.AddAreaOverlay();
            addArea.OnExit += AreaOverlays_OnExit;
            addArea.Owner = Framework.UI.Controls.Window.GetWindow(this);
            addArea.Show();
        }

        private void OpenEditAreaOverlay()
        {
            Overlays.Location.EditAreaOverlay editArea = new Overlays.Location.EditAreaOverlay(selectedArea, selectedCity, selectedProvince);
            editArea.OnExit += AreaOverlays_OnExit;
            editArea.Owner = Framework.UI.Controls.Window.GetWindow(this);
            editArea.Show();
        }

        #endregion

        

        

       
    }
}
