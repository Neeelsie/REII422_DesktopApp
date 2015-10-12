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
    /// Interaction logic for AddProvinceOverlay.xaml
    /// </summary>
    public partial class AddProvinceOverlay
    {
        public event EventHandler OnExit;

        public AddProvinceOverlay()
        {
            InitializeComponent();
        }

        private void BT_AddProvince_Click(object sender, RoutedEventArgs e)
        {
            AddProvince();
        }

        private void OverlayWindow_Closed(object sender, EventArgs e)
        {
            if( OnExit != null)
                OnExit(this, new EventArgs());
        }


        #region Form Control

        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if(loading)
                    {
                        this.IsBusy = true;
                        BT_AddProvince.IsEnabled = false;
                    }
                    else
                    {
                        this.IsBusy = false;
                        BT_AddProvince.IsEnabled = true;
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
        
        private void ClearForm()
        {
            this.Dispatcher.Invoke(() =>
                {
                    TB_ProvinceName.Text = "";
                });
        }

        #endregion

        #region Get
        private string GetProvinceName()
        {
            string provinceName = "";

            this.Dispatcher.Invoke(() =>
                {
                    provinceName = TB_ProvinceName.Text;
                });

            return provinceName;
        }
        #endregion

        private void AddProvince()
        {
            new System.Threading.Thread(() =>
                {
                    if( GetProvinceName() != "" )
                    {
                        Classes.LocationManager locationManager = new Classes.LocationManager();

                        if (locationManager.CanAddProvince(GetProvinceName()))
                        {
                            SetLoadingState(true);

                            if (locationManager.AddProvince(GetProvinceName()))
                            {
                                DisplayNotifyBox(GetProvinceName() + " Added", GetProvinceName() + " has been added successfully", 2);
                                ClearForm();
                            }
                            else
                            {
                                DisplayNotifyBox("Could not Add", GetProvinceName() + " was not added", 2);
                            }

                            SetLoadingState(false);

                        }
                        else
                        {
                            DisplayNotifyBox("Cannot add " + GetProvinceName(), "A province with the same name already exists", 2);
                        }
                    }
                    else
                    {
                        DisplayNotifyBox("ERROR", "Province name cannot be empty",2);
                    }                    
                }).Start();
        }

        


    }
}
