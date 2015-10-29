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
using RealEstate.Windows;
using System.IO;

namespace RealEstate.Views.AgentViews
{
    /// <summary>
    /// Interaction logic for AddListingView.xaml
    /// </summary>
    public partial class AddListingView
    {

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

        BitmapImage previewImage;

        private List<string> imageSource = new List<string>();
        private List<string> imageCaptions = new List<string>();

        public AddListingView()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void CB_Complex_Click(object sender, RoutedEventArgs e)
        {
            if( (bool)CB_Complex.IsChecked )
            {
                SP_ComplexDetails.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                SP_ComplexDetails.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            (this.Tag as AgentWindow).HideAddListingView();
            (this.Tag as AgentWindow).ShowListingsView();
        }

        private void BT_AddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".jpeg";
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                
                CB_Images.Items.Add(dialog.FileName.ToString());
                IMG_SelectedImage.Source = CloneImage(dialog.FileName.ToString());
                imageSource.Add(dialog.FileName.ToString());
                imageCaptions.Add("");
                
                if( (CB_Images.Items.Count-1)  == 0 )
                {
                    CB_Images.SelectedIndex = 0;
                }
                else
                {
                    CB_Images.SelectedIndex = CB_Images.Items.Count - 1;
                }
            }
        }

        private void CB_Images_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if( CB_Images.SelectedIndex != -1)
            {
                IMG_SelectedImage.Source = CloneImage(CB_Images.SelectedValue.ToString());
                TB_ImageCaption.Text = imageCaptions[CB_Images.SelectedIndex].ToString();
            }
        }


        private void LoadComboBoxes()
        {
            LoadClients();
            LoadProvinces();
        }

        private void LoadClients()
        {
            Classes.ListingManager listManager = new Classes.ListingManager();
            foreach (string clients in listManager.GetClients())
            {
                CB_Client.Items.Add(clients);
            }
            clientsID = listManager.GetClientsID();
        }

        private void LoadProvinces()
        {
            Classes.ListingManager listManager = new Classes.ListingManager();
            foreach (string provinces in listManager.GetProvinces())
            {
                CB_Province.Items.Add(provinces);
            }
            provinceID = listManager.GetProvincesID();
        }

        private void LoadCities()
        {
            this.Dispatcher.Invoke(() =>
            {
                Classes.ListingManager listManager = new Classes.ListingManager();
                foreach (string cities in listManager.GetCities(currentProvinceID))
                {
                    CB_City.Items.Add(cities);
                }
                cityID = listManager.GetCitiesID(currentProvinceID);
            });
        }

        private void ClearCities()
        {
            this.Dispatcher.Invoke(()=>
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
                if (CB_City.SelectedIndex != -1)
                    currentCityID = cityID[CB_City.SelectedIndex];
            });
        }

        private void CreateListings()
        {
            agentEmail = (this.Tag as AgentWindow).currentAgentName;
            Classes.ListingManager listManager = new Classes.ListingManager();
            listManager.AddListingAddress(currentAreaID, streetName, streetNo);
            addressID = listManager.GetAddressID(currentAreaID, streetName, streetNo);
            if (CB_Complex.IsChecked == true)
            {
                listManager.AddListingComplex(complexName, addressID);
                complexID = listManager.GetComplexID(complexName, addressID);
            }
            else
                complexID = -1;

            listManager.AddListingProperty(currentClientID, addressID, complexID, complexNo, bedrooms, bathrooms, garages, hasPool, plotSize, houseSize, propertyValue);
            propertyID = listManager.GetPropertyID(currentClientID);
            agentID = listManager.GetAgentID(agentEmail);
            listManager.AddListing(propertyID, agentID, propertyPrice, isNegotiable, isSold);

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

            if (CB_Complex.IsChecked == true)
            {
                complexName = TB_ComplexName.Text;
                complexNo = Convert.ToInt32(TB_ComplexNo.Text);
            }

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

        private void CB_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentClientID = clientsID[CB_Client.SelectedIndex];
        }

        private void CB_Province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new System.Threading.Thread(() =>
            {
                ClearCities();
                ChangeProvinceID();
                LoadCities();
            }).Start();
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

        private void CB_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Area.SelectedIndex != -1)
                currentAreaID = areaID[CB_Area.SelectedIndex];
        }

        private void BT_CreateListing_Click(object sender, RoutedEventArgs e)
        {
            LoadVariablesTB();
            CreateListings();
            UploadImages();
        }

        private void TB_ImageCaption_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            imageCaptions[CB_Images.SelectedIndex] = TB_ImageCaption.Text.ToString() + e.Text.ToString();
        }

        private void UploadImages()
        {
            Overlays.Listings.LoadingOverlay uploadImages = new Overlays.Listings.LoadingOverlay(propertyID,imageSource,imageCaptions);
            uploadImages.Owner = Framework.UI.Controls.Window.GetWindow(this);
            uploadImages.Show();
        }

        BitmapImage GetBitmapImage(byte[] imageBytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }


        BitmapImage CloneImage(string filePath)
        {
            List<byte> bytes = new List<byte>();
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] buffer = new byte[65536];
            int read = 0;
            do
            {
                read = fs.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < read; i++)
                    bytes.Add(buffer[i]);

            }
            while (read != 0);
            
            fs.Close();


            var allbytes = bytes.ToArray();
            var bitmapimage = GetBitmapImage(allbytes);

            return bitmapimage;

        }

       

    }
}
