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

namespace RealEstate.Overlays.Location
{
    /// <summary>
    /// Interaction logic for AddCityOverlay.xaml
    /// </summary>
    public partial class AddCityOverlay
    {
        public event EventHandler OnExit;

        public AddCityOverlay()
        {
            InitializeComponent();
        }

   
        private void RE_AddCity_Closed(object sender, EventArgs e)
        {
            if (OnExit != null)
                OnExit(this, new EventArgs());
        }
        private void RE_AddCity_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProvinces();
            CB_Provinces.SelectedIndex = 1;
        }
        private void BT_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BT_Add_Click(object sender, RoutedEventArgs e)
        {
            AddCity();            
        }

        #region Form Control
        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if( loading )
                    {
                        this.IsBusy = true;
                        TB_CityName.IsEnabled = false;
                        CB_Provinces.IsEnabled = false;
                        BT_Add.IsEnabled = false;
                        BT_Close.IsEnabled = false;
                    }
                    else
                    {
                        this.IsBusy = false;
                        TB_CityName.IsEnabled = true;
                        CB_Provinces.IsEnabled = true;
                        BT_Add.IsEnabled = true;
                        BT_Close.IsEnabled = true;
                    }
                });
        }

        private void LoadProvinces()
        {
            new System.Threading.Thread(() =>
                {
                    SetLoadingState(true);
                    Classes.DatabaseManager dbManager = new Classes.DatabaseManager();

                    var provinceNames = dbManager.ReturnQuery("SELECT Province_Name FROM Province ORDER BY Province_Name ASC;");

                    foreach( var province in provinceNames )
                    {
                        InsertProvince(province[0]);
                    }

                    SetLoadingState(false);

                }).Start();
        }

        private void InsertProvince(string provinceName)
        {
            this.Dispatcher.Invoke(() =>
                {
                    CB_Provinces.Items.Add(provinceName);
                });
        }

        private void DisplayNotifyBox(string title, string message, int durationSeconds)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, durationSeconds), false);
            });
        }

        private void ClearForm()
        {
            this.Dispatcher.Invoke(() =>
                {
                    TB_CityName.Text = "";
                });
        }
        

        #endregion

        #region Get
        private string GetCityName()
        {
            string cityName = "";

            this.Dispatcher.Invoke(() =>
                {
                    cityName = TB_CityName.Text.ToString();
                });

            return cityName;
        }

        private string GetProvinceName()
        {
            string provinceName = "";

            this.Dispatcher.Invoke(() =>
                {
                    provinceName = CB_Provinces.SelectedValue.ToString();
                });

            return provinceName;
        }

        #endregion

        private void AddCity()
        {
            if( TB_CityName.Text != "")
            {
                new System.Threading.Thread(() =>
                    {
                        SetLoadingState(true);
                        Classes.LocationManager locationManager = new Classes.LocationManager();

                        if (locationManager.CanAddCity(GetCityName(), GetProvinceName()))
                        {
                            if (locationManager.AddCity(GetCityName(), GetProvinceName()))
                            {
                                DisplayNotifyBox( GetCityName() + " Added", GetCityName() + " was added in " + GetProvinceName(), 3);
                                ClearForm();
                            }
                            else
                            {
                                DisplayNotifyBox("Could not Add City", GetCityName() + " could not be added. Please try again", 3);
                            }
                        }
                        else
                        {
                            DisplayNotifyBox("Cannot Add City", GetCityName() + " already exists in " + GetProvinceName(), 3);
                        }

                        SetLoadingState(false);
                    }).Start();
            }
            else
            {
                DisplayNotifyBox("ERROR", "City name cannot be empty", 2);
            }
        }
        

        
    }
}

