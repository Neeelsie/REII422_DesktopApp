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
    /// Interaction logic for EditCityOverlay.xaml
    /// </summary>
    public partial class EditCityOverlay
    {
        string cityToEdit = "";
        string cityProvince = "";
        bool editCity = false;
        bool editProvince = false;

        public event EventHandler OnExit;

        public EditCityOverlay(string city, string province)
        {
            InitializeComponent();
            cityToEdit = city;
            cityProvince = province;
            this.Title = "Edit " + cityToEdit + "," + province;
        }

        private void RE_EditCity_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProvinces();
            CB_NewProvince.SelectedIndex = 0;
        }

        private void CB_ChangeProvince_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewProvince.Visibility = System.Windows.Visibility.Visible;
            CB_NewProvince.Visibility = System.Windows.Visibility.Visible;
            CB_ChangeProvince.Visibility = System.Windows.Visibility.Hidden;
            editProvince = true;
        }

        private void CB_ChangeName_Checked(object sender, RoutedEventArgs e)
        {
            TBL_NewCityName.Visibility = System.Windows.Visibility.Visible;
            TB_NewCityName.Visibility = System.Windows.Visibility.Visible;
            CB_ChangeName.Visibility = System.Windows.Visibility.Hidden;
            editCity = true;
        }

        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BT_SaveAndClose_Click(object sender, RoutedEventArgs e)
        {

            if (editCity || editProvince)
            {
               if( (GetNewCityName() != "" && editCity) || editProvince)
               {
                   if (editCity && editProvince)
                   {
                       EditCity(cityToEdit, cityProvince, GetNewCityName(), GetNewProvinceName());
                   }
                   else if (editCity)
                   {
                       EditCity(cityToEdit, cityProvince, GetNewCityName(), cityProvince);
                   }
                   else if (editProvince)
                   {
                       EditCity(cityToEdit, cityProvince, cityToEdit, GetNewProvinceName());
                   }
               }
               else
               {
                   DisplayNotifyBox("ERROR", "New city name cannot be empty", 3);
               }
            }
            else
            {
                DisplayNotifyBox("No change selected", "Please select a proprty to change", 3);
            }
        }

        private void RE_EditCity_Closed(object sender, EventArgs e)
        {
            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        #region Form Control
        private void LoadProvinces()
        {
            new System.Threading.Thread(() =>
                {
                    ClearProvinces();

                    Classes.DatabaseManager dbManager = new Classes.DatabaseManager();

                    var provinces = dbManager.ReturnQuery("SELECT Province_Name FROM Province ORDER BY Province_Name ASC;");

                    foreach( var i in provinces )
                    {
                        Console.WriteLine("Inserting " + i[0]);
                        InsertProvince(i[0]);
                    }

                }).Start();
        }

        private void ClearProvinces()
        {
            this.Dispatcher.Invoke(() =>
                {
                    CB_NewProvince.Items.Clear();
                });
        }

        private void InsertProvince(string provinceName)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if( provinceName != cityProvince)
                        CB_NewProvince.Items.Add(provinceName);
                });
        }

        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if (loading)
                    {
                        CB_NewProvince.IsEnabled = false;
                        TB_NewCityName.IsEnabled = false;
                        CB_ChangeName.IsEnabled = false;
                        CB_ChangeProvince.IsEnabled = false;
                        BT_SaveAndClose.IsEnabled = false;
                        BT_Cancel.IsEnabled = false;
                    }
                    else
                    {
                        CB_NewProvince.IsEnabled = true;
                        TB_NewCityName.IsEnabled = true;
                        CB_ChangeName.IsEnabled = true;
                        CB_ChangeProvince.IsEnabled = true;
                        BT_SaveAndClose.IsEnabled = true;
                        BT_Cancel.IsEnabled = true;
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
        private string GetNewCityName()
        {
            string newCityName = "";
            
            this.Dispatcher.Invoke(() =>
                {
                    newCityName = TB_NewCityName.Text;
                });

            return newCityName;
        }

        private string GetNewProvinceName()
        {
            string newProvinceName = "";

            this.Dispatcher.Invoke(() =>
                {
                    newProvinceName = CB_NewProvince.SelectedValue.ToString();
                });

            return newProvinceName;
        }
        #endregion

        private void EditCity(string oldCityName, string oldProvince, string newCityName, string newProvinceName)
        {
            new System.Threading.Thread(() =>
            {
                bool edited = false;
                SetLoadingState(true);
                
                Classes.LocationManager locationManager = new Classes.LocationManager();
                
                if( locationManager.CanAddCity(newCityName,newProvinceName))
                {
                    if( locationManager.EditCity(oldCityName,oldProvince,newCityName,newProvinceName) )
                    {
                        DisplayNotifyBox("Edited", oldCityName + "," + oldProvince + " changed to " + newCityName + "," + newProvinceName, 3);
                        edited = true;
                    }
                    else
                    {
                        DisplayNotifyBox("Could not edit", "En error occured. Please try again later", 3);
                    }
                }
                else
                {
                    DisplayNotifyBox("Could not edit","Cannot change " + oldCityName + "," + oldProvince + " to " + newCityName + "," + newProvinceName + " because " + newCityName + "," + newProvinceName + " already exists",4);
                }

                SetLoadingState(false);

                if (edited)
                    CloseForm();

            }).Start();
        }

        




    }
}
