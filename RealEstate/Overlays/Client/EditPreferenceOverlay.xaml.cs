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

namespace RealEstate.Overlays.Client
{
    /// <summary>
    /// Interaction logic for EditPreferenceOverlay.xaml
    /// </summary>
        public partial class EditPreferenceOverlay
    {
        #region Variable Initialization
        string clientEmail = "";
        string temp = "";
        bool flag;
        bool edited;
        int clientID = 0;
        int preferenceID = 0;
        int i = 0;
        int numberOfPreferences = 0;
        int [] preferenceIDArray = new int [50];
        int dbSize = 20;   
        int minBedrooms;
        int maxBedrooms;
        int minBathrooms;
        int maxBathrooms;
        int minGarages;
        int maxGarages;
        int minPlotSize;
        int maxPlotSize;
        int minHouseSize;
        int maxHouseSize;
        int minPrice;
        int maxPrice;
        int hasPool;
        #endregion

        public event EventHandler OnExit;

        public EditPreferenceOverlay(String email)
        {
            InitializeComponent();
            clientEmail = email;
            this.Title = "Edit " + clientEmail + " Preferences";
        }

        private void RE_EditPreference_Loaded(object sender, RoutedEventArgs e)
        {
            SetLoadingState(true);
            GetClientID(clientEmail);
            PopulateInputs();
            LoadProvinces();
            LoadPreferencesBox();
            SetLoadingState(false);
        }

        private void RE_EditPreference_Closed(object sender, EventArgs e)
        {
            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        #region Input Response
        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BT_EditExisting_Click(object sender, RoutedEventArgs e)
        {
            SetPreferences();
            EditPreferences();
        }
        private void CB_Province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BT_AddArea.IsEnabled = false;
            if (CB_Province.SelectedItem != null)
                LoadCities();
        }
        private void CB_City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BT_AddArea.IsEnabled = false;
            if (CB_City.SelectedItem != null)
                LoadAreas();
        }
        private void CB_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Area.SelectedItem != null)
            {
                BT_AddArea.IsEnabled = true;
            }
        }
        private void CB_Preferences_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Preferences.SelectedIndex != -1)
            {
                preferenceID = preferenceIDArray[CB_Preferences.SelectedIndex];
                LoadCurrentPreferences(preferenceID);
            }
        }
        private void BT_AddArea_Click(object sender, RoutedEventArgs e)
        {
            InsertPreferedArea();
        }
        private void BT_CreateNew_Click(object sender, RoutedEventArgs e)
        {
            NewPreferences();
        }
        private void BT_DeletePreference_Click(object sender, RoutedEventArgs e)
        {
            Classes.PreferenceManager preferenceManager = new Classes.PreferenceManager();
            preferenceManager.DeletePreference(preferenceID);
            LoadPreferencesBox();
        }
        private void BT_RemoveArea_Click(object sender, RoutedEventArgs e)
        {
            CB_PreferedAreas.Items.Remove(CB_PreferedAreas.SelectedItem);
            CB_PreferedAreas.SelectedIndex = 0;
            if(CB_PreferedAreas.Items.Count == 0)
                BT_RemoveArea.IsEnabled = false;
        }
        #endregion

        #region Form Control
        private void PopulateInputs()
        {
            for (int i = 0; i < dbSize; i++)
            {
                CB_MinBedrooms.Items.Add(i.ToString());
                CB_MaxBedrooms.Items.Add(i.ToString());
                CB_MinBathrooms.Items.Add(i.ToString());
                CB_MaxBathrooms.Items.Add(i.ToString());
                CB_MinGarages.Items.Add(i.ToString());
                CB_MaxGarages.Items.Add(i.ToString());
            }
            CB_HasPool.Items.Add("Don't care");
            CB_HasPool.Items.Add("Yes");
            CB_HasPool.Items.Add("No");
        }
        private void CloseForm()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Close();
            });
        }
        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (loading)
                {
                    CB_MinBedrooms.IsEnabled = false;
                    CB_MaxBedrooms.IsEnabled = false;
                    CB_MinBathrooms.IsEnabled = false;
                    CB_MaxBathrooms.IsEnabled = false;
                    CB_MinGarages.IsEnabled = false;
                    CB_MaxGarages.IsEnabled = false;
                    TB_MinPlotSize.IsEnabled = false;
                    TB_MaxPlotSize.IsEnabled = false;
                    TB_MinHouseSize.IsEnabled = false;
                    TB_MaxHouseSize.IsEnabled = false;
                    TB_MinPrice.IsEnabled = false;
                    TB_MaxPrice.IsEnabled = false;
                    CB_HasPool.IsEnabled = false;
                    CB_Preferences.IsEnabled = false;
                    CB_Province.IsEnabled = false;
                    CB_City.IsEnabled = false;
                    CB_Area.IsEnabled = false;
                    CB_PreferedAreas.IsEnabled = false;
                    BT_RemoveArea.IsEnabled = false;
                    BT_DeletePreference.IsEnabled = false;
                    BT_AddArea.IsEnabled = false;
                    BT_EditExisting.IsEnabled = false;
                    BT_CreateNew.IsEnabled = false;
                    BT_Cancel.IsEnabled = false;
                }
                else
                {
                    CB_MinBedrooms.IsEnabled = true;
                    CB_MaxBedrooms.IsEnabled = true;
                    CB_MinBathrooms.IsEnabled = true;
                    CB_MaxBathrooms.IsEnabled = true;
                    CB_MinGarages.IsEnabled = true;
                    CB_MaxGarages.IsEnabled = true;
                    TB_MinPlotSize.IsEnabled = true;
                    TB_MaxPlotSize.IsEnabled = true;
                    TB_MinHouseSize.IsEnabled = true;
                    TB_MaxHouseSize.IsEnabled = true;
                    TB_MinPrice.IsEnabled = true;
                    TB_MaxPrice.IsEnabled = true;
                    CB_HasPool.IsEnabled = true;
                    CB_Preferences.IsEnabled = true;
                    CB_Province.IsEnabled = true;
                    CB_City.IsEnabled = true;
                    CB_Area.IsEnabled = true;
                    CB_PreferedAreas.IsEnabled = true;
                    BT_RemoveArea.IsEnabled = true;
                    BT_DeletePreference.IsEnabled = true;
                    BT_AddArea.IsEnabled = true;
                    BT_EditExisting.IsEnabled = true;
                    BT_CreateNew.IsEnabled = true;
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
        #endregion

        #region Load Functions
        private void LoadPreferencesBox()
        {
            flag = false;
            CB_Preferences.Items.Clear();
            numberOfPreferences = 0;
            Classes.DatabaseManager dbManager = new Classes.DatabaseManager();
            Classes.LocationManager locManager = new Classes.LocationManager();
            var preferences = (dbManager.ReturnQuery("SELECT Preference_ID FROM Preference WHERE Preference_Client_ID = " + clientID.ToString() + " ORDER BY Preference_ID;"));

            foreach (var i in preferences)
            {
                numberOfPreferences++;
                preferenceIDArray[numberOfPreferences - 1] = Convert.ToInt32(i[0]);
                flag = true;
                CB_Preferences.Items.Add(numberOfPreferences.ToString());
            }

            if (!flag)
            {
                CB_Preferences.Items.Add("1");
                CB_Preferences.SelectedIndex = 0;
                preferenceIDArray[0] = -1;
                preferenceID = preferenceIDArray[0];
                CB_PreferedAreas.Items.Clear();
                InsertPreferences(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                BT_DeletePreference.IsEnabled = false;
                BT_EditExisting.IsEnabled = false;
            }
            else
            {
                //LoadCurrentPreferences(preferenceIDArray[0]);
                preferenceID = preferenceIDArray[0];
                CB_Preferences.SelectedIndex = 0;
                BT_DeletePreference.IsEnabled = true;
                BT_EditExisting.IsEnabled = true;
            }  
        }
        private void LoadCurrentPreferences(int inPrefferenceID)
        {
            preferenceID = inPrefferenceID;
            CB_PreferedAreas.Items.Clear();
            flag = false;
            this.Dispatcher.Invoke(() =>
            {
                SetLoadingState(true);
                ClearPreferences();

                Classes.DatabaseManager dbManager = new Classes.DatabaseManager();
                Classes.LocationManager locManager = new Classes.LocationManager();
                var preferences = (dbManager.ReturnQuery("SELECT Preference_Min_Bedrooms, Preference_Max_Bedrooms, Preference_Min_Bathrooms, Preference_Max_Bathrooms, Preference_Min_Garages, Preference_Max_Garages, Preference_hasPool, Preference_Min_Plot_Size, Preference_Max_Plot_Size, Preference_Min_House_Size, Preference_Max_House_Size, Preference_Min_Price, Preference_Max_Price FROM Preference WHERE Preference_Client_ID = " + clientID.ToString() + " AND Preference_ID = " + preferenceID.ToString() + ";"));

                foreach (var i in preferences)
                {
                    InsertPreferences(Convert.ToInt32(i[0]), Convert.ToInt32(i[1]), Convert.ToInt32(i[2]), Convert.ToInt32(i[3]), Convert.ToInt32(i[4]), Convert.ToInt32(i[5]), Convert.ToInt32(i[6]), Convert.ToInt32(i[7]), Convert.ToInt32(i[8]), Convert.ToInt32(i[9]), Convert.ToInt32(i[10]), Convert.ToInt32(i[11]), Convert.ToInt32(i[12]));
                }

                var preferedAreas = (dbManager.ReturnQuery("SELECT Area_ID FROM Preference_Area WHERE Preference_ID = " + preferenceID + " ORDER BY Area_ID;"));
                foreach (var j in preferedAreas)
                {
                    flag = true;
                    CB_PreferedAreas.Items.Add(locManager.AreaFullName(Convert.ToInt32(j[0])));
                }

                if(flag)
                {
                    CB_PreferedAreas.SelectedIndex = 0;
                    BT_RemoveArea.IsEnabled = true;
                }
                else
                {
                    BT_RemoveArea.IsEnabled = false;
                }
                SetLoadingState(false);

            });
        }
        private void LoadProvinces()
        {
            ClearProvinces();
            ClearCities();
            ClearAreas();

            Classes.DatabaseManager dbManager = new Classes.DatabaseManager();

            var provinces = dbManager.ReturnQuery("SELECT Province_Name FROM Province ORDER BY Province_Name;");

            foreach (var i in provinces)
            {
                InsertProvince(i[0]);
            }
        }
        private void LoadCities()
        {
            ClearCities();
            ClearAreas();

            Classes.DatabaseManager dbManager = new Classes.DatabaseManager();
            var cities = dbManager.ReturnQuery("SELECT City_Name FROM City WHERE City_Province_ID = " + GetProvinceID(CB_Province.SelectedItem.ToString()) + " ORDER BY City_Name;");

            foreach (var i in cities)
            {
                InsertCity(i[0]);
            }
        }
        private void LoadAreas()
        {
            ClearAreas();

            Classes.DatabaseManager dbManager = new Classes.DatabaseManager();
            var areas = dbManager.ReturnQuery("SELECT Area_Name FROM Area WHERE Area_City_ID = " + GetCityID(CB_City.SelectedItem.ToString(), CB_Province.SelectedItem.ToString()) + " ORDER BY Area_Name;");

            foreach (var i in areas)
            {
                InsertArea(i[0]);
            }
        }
        #endregion

        #region Clear Functions
        private void ClearProvinces()
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_Province.Items.Clear();
            });
        }
        private void ClearCities()
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_City.Items.Clear();
            });
        }
        private void ClearAreas()
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_Area.Items.Clear();
            });
        }
        private void ClearPreferences()
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_MinBedrooms.SelectedIndex = 0; ;
                CB_MaxBedrooms.SelectedIndex = 0;
                CB_MinBathrooms.SelectedIndex = 0;
                CB_MaxBathrooms.SelectedIndex = 0;
                CB_MinGarages.SelectedIndex = 0;
                CB_MaxGarages.SelectedIndex = 0;
                TB_MinPlotSize.Text = "0";
                TB_MaxPlotSize.Text = "0";
                TB_MinHouseSize.Text = "0";
                TB_MaxHouseSize.Text = "0";
                TB_MinPrice.Text = "0";
                TB_MaxPrice.Text = "0";
                CB_HasPool.SelectedIndex = 0;
            });
        }
        #endregion

        #region Get & Set Methods
        private int GetProvinceID(string proviceName)
        {
            Classes.LocationManager locationManager = new Classes.LocationManager();
            return locationManager.ProvinceID(proviceName);
        }
        private int GetCityID(string cityName, string provinceName)
        {
            Classes.LocationManager locationManager = new Classes.LocationManager();
            return locationManager.CityID(cityName, GetProvinceID(provinceName));
        }
        private void GetClientID(string clientEmail)
        {
            Classes.DatabaseManager dbManager = new Classes.DatabaseManager();
            var tryGetClientID = dbManager.ReturnQuery("SELECT Client_ID FROM Clients WHERE Client_Email = '" + clientEmail + "';");

            foreach (var i in tryGetClientID)
            {
                clientID = Convert.ToInt32(i[0]);
            }
        }

        private void SetPreferences()
        {
            this.Dispatcher.Invoke(() =>
            {
                minBedrooms = Convert.ToInt32(CB_MinBedrooms.SelectedItem);
                maxBedrooms = Convert.ToInt32(CB_MaxBedrooms.SelectedItem);
                minBathrooms = Convert.ToInt32(CB_MinBathrooms.SelectedItem);
                maxBathrooms = Convert.ToInt32(CB_MaxBathrooms.SelectedItem);
                minGarages = Convert.ToInt32(CB_MinGarages.SelectedItem);
                maxGarages = Convert.ToInt32(CB_MaxGarages.SelectedItem);
                minPlotSize = Convert.ToInt32(TB_MinPlotSize.Text);
                maxPlotSize = Convert.ToInt32(TB_MaxPlotSize.Text);
                minHouseSize = Convert.ToInt32(TB_MinHouseSize.Text);
                maxHouseSize = Convert.ToInt32(TB_MaxHouseSize.Text);
                minPrice = Convert.ToInt32(TB_MinPrice.Text);
                maxPrice = Convert.ToInt32(TB_MaxPrice.Text);
                hasPool = Convert.ToInt32(CB_HasPool.SelectedIndex.ToString());
            });
        }
        #endregion

        #region Insert
        private void InsertProvince(string provinceName)
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_Province.Items.Add(provinceName);
            });
        }
        private void InsertCity(string cityName)
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_City.Items.Add(cityName);
            });
        }
        private void InsertArea(string areaName)
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_Area.Items.Add(areaName);
            });
        }
        private void InsertPreferedArea()
        {
            flag = false;
            temp = CB_Province.SelectedItem.ToString() + ", " + CB_City.SelectedItem.ToString() + ", " + CB_Area.SelectedItem.ToString();
            for (i = 0; i < CB_PreferedAreas.Items.Count; i++ )
            {
                if(temp == CB_PreferedAreas.Items[i].ToString())
                {
                    flag = true;
                    DisplayNotifyBox("Not Added", "The area " + temp + " is already in your preffered area list", 4);
                }
            }
            if(!flag)
            {
                CB_PreferedAreas.Items.Add(temp);
                CB_PreferedAreas.SelectedIndex = 0;
                BT_RemoveArea.IsEnabled = true;
            }
        }
        private void InsertPreferences(int inMinBedrooms, int inMaxBedrooms, int inMinBathrooms, int inMaxBathrooms, int inMinGarages, int inMaxGarages, int inHasPool, int inMinPlotSize, int inMaxPlotSize, int inMinHouseSize, int inMaxHouseSize, int inMinPrice, int inMaxPrice)
        {
            this.Dispatcher.Invoke(() =>
            {
                CB_MinBedrooms.SelectedIndex = inMinBedrooms;
                CB_MaxBedrooms.SelectedIndex = inMaxBedrooms;
                CB_MinBathrooms.SelectedIndex = inMinBathrooms;
                CB_MaxBathrooms.SelectedIndex = inMaxBathrooms;
                CB_MinGarages.SelectedIndex = inMinGarages;
                CB_MaxGarages.SelectedIndex = inMaxGarages;
                TB_MinPlotSize.Text = inMinPlotSize.ToString();
                TB_MaxPlotSize.Text = inMaxPlotSize.ToString();
                TB_MinHouseSize.Text = inMinHouseSize.ToString();
                TB_MaxHouseSize.Text = inMaxHouseSize.ToString();
                TB_MinPrice.Text = inMinPrice.ToString();
                TB_MaxPrice.Text = inMaxPrice.ToString();
                CB_HasPool.SelectedIndex = inHasPool;
            });
        }
        #endregion  

        private void EditPreferences()
        {
            new System.Threading.Thread(() =>
            {
                edited = false;
                SetLoadingState(true);

                Classes.PreferenceManager preferenceManager = new Classes.PreferenceManager();

                if (preferenceManager.EditPreference(preferenceID, clientID, minBedrooms, maxBedrooms, minBathrooms, maxBathrooms, minGarages, maxGarages, minPlotSize, maxPlotSize, minHouseSize, maxHouseSize, minPrice, maxPrice, hasPool))
                {
                    flag = true;
                    preferenceManager.ClearPreferenceArea(preferenceID);
                    for (i = 0; i < CB_PreferedAreas.Items.Count; i++)
                    {
                        if (!preferenceManager.AddPreferenceArea(preferenceID, CB_PreferedAreas.Items[i].ToString()))
                        {
                            flag = false;
                            DisplayNotifyBox("Could not edit", "An error occured while trying to add area " + CB_PreferedAreas.Items[i].ToString(), 4);
                        }
                    }
                    if (flag)
                    {
                        DisplayNotifyBox("Preference Added", "Preference has been added", 3);
                        edited = true;
                    }
                }
                else
                {
                    DisplayNotifyBox("Could not edit", "An error occured while trying to edit preference. Please try again later", 3);
                }

                SetLoadingState(false);
                if (edited)
                    CloseForm();

            }).Start();
        }

        private void NewPreferences()
        {
            new System.Threading.Thread(() =>
            {
                SetLoadingState(true);
                edited = false;
                SetPreferences();

                Classes.PreferenceManager preferenceManager = new Classes.PreferenceManager();
                Classes.DatabaseManager dbManager = new Classes.DatabaseManager();

                if (preferenceManager.CanAddPreference(clientID, minBedrooms, maxBedrooms, minBathrooms, maxBathrooms, minGarages, maxGarages, minPlotSize, maxPlotSize, minHouseSize, maxHouseSize, minPrice, maxPrice, hasPool))
                {
                    if (preferenceManager.AddPreference(clientID, minBedrooms, maxBedrooms, minBathrooms, maxBathrooms, minGarages, maxGarages, minPlotSize, maxPlotSize, minHouseSize, maxHouseSize, minPrice, maxPrice, hasPool))
                    {
                        preferenceID = preferenceManager.GetPreferenceID(clientID);
                        flag=true;
                        for(i = 0; i < CB_PreferedAreas.Items.Count; i++)
                        {
                            if (!preferenceManager.AddPreferenceArea(preferenceID, CB_PreferedAreas.Items[i].ToString()))
                            {
                                flag = false;
                                DisplayNotifyBox("Could not edit", "An error occured while trying to add area " + CB_PreferedAreas.Items[i].ToString(), 4);
                            }
                        }
                        if (flag)
                        {
                            DisplayNotifyBox("Preference Added", "Preference has been added", 3);
                            edited = true;
                        }
                    }
                    else
                    {
                        DisplayNotifyBox("Could not edit", "An error occured while trying to add a new preference. Please try again later", 3);
                    }

                }
                else
                {
                    DisplayNotifyBox("Could not edit", "The preferences as outlined already exists for this client", 4);
                }

                SetLoadingState(false);
                if (edited)
                    CloseForm();

            }).Start(); 
        }
    }
}
