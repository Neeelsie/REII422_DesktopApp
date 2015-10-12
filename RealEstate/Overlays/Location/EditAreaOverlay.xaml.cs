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
    /// Interaction logic for EditAreaOverlay.xaml
    /// </summary>
    public partial class EditAreaOverlay
    {
        string areaToEdit = "";
        string areaCity = "";
        string areaProvince = "";

        bool editName = false;
        bool editCity = false;
        public event EventHandler OnExit;

        public EditAreaOverlay(string areaName, string cityName, string provinceName)
        {
            InitializeComponent();
            areaToEdit = areaName;
            areaCity = cityName;
            areaProvince = provinceName;

            this.Title = "Edit " + areaToEdit + "," + areaCity + "," + areaProvince;
        }

        private void RE_EditAreaOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCities();
        }

        private void RE_EditAreaOverlay_Closed(object sender, EventArgs e)
        {
            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CB_ChangeName_Checked(object sender, RoutedEventArgs e)
        {
            TB_NewAreaName.Visibility = System.Windows.Visibility.Visible;
            TBL_NewAreaName.Visibility = System.Windows.Visibility.Visible;
            CB_ChangeName.Visibility = System.Windows.Visibility.Hidden;
            editName = true;
        }

        private void CB_ChangeCity_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewCity.Visibility = System.Windows.Visibility.Visible;
            CB_NewCity.Visibility = System.Windows.Visibility.Visible;
            CB_ChangeCity.Visibility = System.Windows.Visibility.Hidden;
            editCity = true;
        }

        #region Form Control

        private void LoadCities()
        {
            CB_NewCity.Items.Clear();

            new System.Threading.Thread(() =>
                {
                    Classes.DatabaseManager dbManager = new Classes.DatabaseManager();

                    var cities = dbManager.ReturnQuery("SELECT City_Name,Province_Name FROM City,Province WHERE City_Province_ID = Province_ID ORDER BY Province_Name, City_Name;");

                    foreach( var city in cities)
                    {
                        InsertCity(city[0], city[1]);
                    }

                }).Start();

            CB_NewCity.SelectedIndex = 0;
        }

        private void InsertCity(string cityName, string provinceName)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if( (cityName != areaCity) && (provinceName != areaProvince))
                        CB_NewCity.Items.Add(cityName + "," + provinceName);
                });
        }

        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if( loading )
                    {
                        CB_ChangeCity.IsEnabled = false;
                        CB_ChangeName.IsEnabled = false;
                        CB_NewCity.IsEnabled = false;
                        TB_NewAreaName.IsEnabled = false;
                        BT_Cancel.IsEnabled = false;
                        BT_SaveAndClose.IsEnabled = false;
                        this.IsBusy = true;
                    }
                    else
                    {
                        CB_ChangeCity.IsEnabled = true;
                        CB_ChangeName.IsEnabled = true;
                        CB_NewCity.IsEnabled = true;
                        TB_NewAreaName.IsEnabled = true;
                        BT_Cancel.IsEnabled = true;
                        BT_SaveAndClose.IsEnabled = true;
                        this.IsBusy = false;
                    }
                });
        }

        private void DisplayNotifyBox(string title, string message, int durationSeconds)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, durationSeconds), false);
            });
        }

        private void CloseForm()
        {
            this.Dispatcher.Invoke(() =>
                {
                    this.Close();
                });
        }

        #endregion

        #region Get
        private string GetNewAreaName()
        {
            string areaName = "";

            this.Dispatcher.Invoke(() =>
                {
                    areaName = TB_NewAreaName.Text;
                });

            return areaName;
        }
        

        private string GetNewCityName()
        {
            string cityName = "";

            this.Dispatcher.Invoke(() =>
                {
                    string tempCity = CB_NewCity.SelectedValue.ToString();
                    bool commaFound = false;

                    for( int i =0; i < tempCity.Length ; i++)
                    {
                        if (tempCity[i] == ',')
                            commaFound = true;

                        if (!commaFound)
                            cityName += tempCity[i];
                    }
                });

            return cityName;
        }

        private string GetNewProvinceName()
        {
            string provinceName = "";

            this.Dispatcher.Invoke(() =>
                {
                    string tempProvince = CB_NewCity.SelectedValue.ToString();
                    bool commaFound = false;

                    for( int i = 0; i < tempProvince.Length ; i++ )
                    {
                        if (commaFound)
                            provinceName += tempProvince[i];

                        if (tempProvince[i] == ',')
                            commaFound = true;
                    }

                });

            return provinceName;
        }
        #endregion

        
        private void EditArea(string oldName, string newName, string oldCity , string newCity, string oldProvince, string newProvince)
        {
            new System.Threading.Thread(() =>
            {
                bool edited = false;
                SetLoadingState(true);

                Classes.LocationManager locationManager = new Classes.LocationManager();

                if( locationManager.CanAddArea(newName,newCity,newProvince) )
                {
                    if (locationManager.EditArea(oldName, newName, oldCity, newCity, oldProvince, newProvince))
                    {
                        DisplayNotifyBox("Edited", oldName + "," + oldCity + "," + oldProvince + " changed to " + newName + "," + newCity + "," + newProvince,3);
                        edited = true;
                    }
                    else
                    {
                        DisplayNotifyBox("Could not edit", "En error occured. Please try again later", 3);
                    }
                }
                else
                {
                    DisplayNotifyBox("Could not edit", "Cannot change " + oldName + "," + oldCity + "," + oldProvince + " to " + newName + "," + newCity + "," + newProvince + " because " + newName + "," + newCity + "," + newProvince + " already exists", 4);
                }

                SetLoadingState(false);

                if (edited)
                    CloseForm();
            }).Start();
        }

        private void BT_SaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            if (editCity || editName)
            {
                if ((GetNewAreaName() != "" && editName) || editCity)
                {
                    if (editCity && editName)
                    {
                        EditArea(areaToEdit, GetNewAreaName(), areaCity, GetNewCityName(), areaProvince, GetNewProvinceName());
                    }
                    else if (editName)
                    {
                        EditArea(areaToEdit, GetNewAreaName(), areaCity, areaCity, areaProvince, areaProvince);
                    }
                    else if (editCity)
                    {
                        EditArea(areaToEdit, areaToEdit, areaCity, GetNewCityName(), areaProvince, GetNewProvinceName());
                    }
                }
                else
                {
                    DisplayNotifyBox("ERROR", "New area name cannot be empty", 3);
                }
            }
            else
            {
                DisplayNotifyBox("No change selected", "Please select a property to change", 3);
            }
        }

    }
}
