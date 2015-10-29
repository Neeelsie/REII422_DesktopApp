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
using RealEstate.Views.AgentViews;

namespace RealEstate.Overlays.Listings
{
    /// <summary>
    /// Interaction logic for AddListingOverlay.xaml
    /// </summary>
    public partial class AddListingOverlay 
    {
        public event EventHandler OnExit;
        public List<int> clientsID;
        public List<int> provinceID;
        public List<int> cityID;
        public List<int> areaID;
        public int currentProvinceID;
        public int currentCityID;
        public int currentAreaID;
        public int currentClientID;

        public string streetName;
        public int streetNo;
        public string complexName;
        public int complexNo;

        public string agentEmail;
        public int agentID;
        public int propertyID;
        public int complexID;
        public int addressID;
        public int bedrooms;
        public int bathrooms;
        public int garages;
        public int plotSize;
        public int houseSize;
        public int propertyPrice;
        public int propertyValue;
        public int isSold;
        public int isNegotiable;
        public int hasPool;

        public AddListingOverlay(string currentAgent)
        {
            agentEmail = currentAgent;
            InitializeComponent();
            LoadComboBoxes();
        }


        private void CB_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentClientID = clientsID[CB_Client.SelectedIndex];
        }

        private void CB_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CB_Area.SelectedIndex != -1)
            currentAreaID = areaID[CB_Area.SelectedIndex];
        }

        private void CB_City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new System.Threading.Thread(() =>
            {
                ClearAreas();
                ChangeCityID();
                LoadArea();
            }).Start();
        }

        private void CB_Province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new System.Threading.Thread(() =>
                {
                    ClearCities();
                    ChangeProvinceID();
                    LoadCity();
                }).Start();

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

        private void BT_CreateNew_Click(object sender, RoutedEventArgs e)
        {
            LoadVariablesTB();
            CreateListings();
        }

        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Add Listing
        private void AddListing()
        {

        }

        #endregion

        #region Load CB

        private void LoadComboBoxes()
    {
        LoadClient();
        LoadProvince();
        LoadPictures();


    }

        private void LoadClient()
        {
            Classes.ListingManager listManager = new Classes.ListingManager();
            foreach(string clients in listManager.GetClients())
            {
                CB_Client.Items.Add(clients);
            }
            clientsID = listManager.GetClientsID();

        }

        private void LoadProvince()
        {
            Classes.ListingManager listManager = new Classes.ListingManager();
            foreach (string provinces in listManager.GetProvinces())
            {
                CB_Province.Items.Add(provinces);
            }
            provinceID = listManager.GetProvincesID();

        }
        private void LoadCity()
        {            this.Dispatcher.Invoke(() =>
                {
            Classes.ListingManager listManager = new Classes.ListingManager();
            foreach (string cities in listManager.GetCities(currentProvinceID))
            {
                CB_City.Items.Add(cities);
            }
            cityID = listManager.GetCitiesID(currentProvinceID);
                });

        }
        private void LoadArea()
        {
            this.Dispatcher.Invoke(() =>
            {
                Classes.ListingManager listManager = new Classes.ListingManager();
                foreach (string areas in listManager.GetAreas(currentCityID))
                {
                    CB_Area.Items.Add(areas);
                }
                areaID = listManager.GetAreasID(currentCityID);
            });

        }
        private void LoadPictures()
        {

        }
        #endregion
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
        private void ChangeProvinceID()
        {
            this.Dispatcher.Invoke(() =>
                {
                    currentProvinceID = provinceID[CB_Province.SelectedIndex];
                });
        }
        private void ChangeCityID()
        {
            this.Dispatcher.Invoke(() =>
            {
                if(CB_City.SelectedIndex != -1)
                currentCityID = cityID[CB_City.SelectedIndex];
            });
        }

        private void CreateListings()
        {
            Classes.ListingManager listManager = new Classes.ListingManager();
            listManager.AddListingAddress(currentAreaID,streetName,streetNo);
            addressID = listManager.GetAddressID(currentAreaID, streetName, streetNo);
            if(CB_Complex.IsChecked == true)
            {
                listManager.AddListingComplex(complexName, addressID);
                complexID = listManager.GetComplexID(complexName,addressID);
            }
            else 
                complexID = -1;

            listManager.AddListingProperty(currentClientID, addressID, complexID, complexNo, bedrooms, bathrooms, garages, hasPool, plotSize, houseSize, propertyValue);
            propertyID = listManager.GetPropertyID(currentClientID);
            agentID = listManager.GetAgentID(agentEmail);
            listManager.AddListing(propertyID,agentID,propertyPrice,isNegotiable,isSold);

        }
        private void LoadVariablesTB()
        {
            streetName = TB_Streetname.Text;
            streetNo = Convert.ToInt32(TB_Streetno.Text);
            bedrooms = Convert.ToInt32(TB_Bedrooms.Text);
            bathrooms = Convert.ToInt32(TB_Bathrooms.Text);
            garages = Convert.ToInt32(TB_Garages.Text);
            plotSize = Convert.ToInt32(TB_PlotSize.Text);
            houseSize = Convert.ToInt32(TB_HouseSize.Text);
            propertyPrice = Convert.ToInt32(TB_ListPrice.Text);
            propertyValue = Convert.ToInt32(TB_Price.Text);
            isSold = Convert.ToInt32(CB_isSold.IsChecked);
            isNegotiable = Convert.ToInt32(CB_isNegotiable.IsChecked);
            hasPool = Convert.ToInt32(CB_hasPool.IsChecked);
            
            complexName = "Null";
            complexNo = 0;

            if(CB_Complex.IsChecked == true)
            {
                complexName = TB_ComplexName.Text;
                complexNo = Convert.ToInt32(TB_ComplexNo.Text);
            }
            
        }
    }
}
