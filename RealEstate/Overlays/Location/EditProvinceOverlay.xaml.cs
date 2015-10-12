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
    /// Interaction logic for EditProvinceOverlay.xaml
    /// </summary>
    public partial class EditProvinceOverlay
    {
        string provinceToEdit = "";
        public event EventHandler OnExit;
        bool isEdited = false;

        public EditProvinceOverlay(string province)
        {
            InitializeComponent();
            provinceToEdit = province;
            this.Title = "Edit " + province;
        }

        private void OverlayWindow_Closed(object sender, EventArgs e)
        {
            if( !isEdited )
            {
                DisplayNotifyBox(provinceToEdit + " Not Edited", provinceToEdit + " was not changed", 2);
            }

            if (OnExit != null)
                OnExit(this, new EventArgs());
        }

        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BT_Edit_Click(object sender, RoutedEventArgs e)
        {
            if( TB_NewProvinceName.Text != "")
            {
                if( TB_NewProvinceName.Text.ToString() != provinceToEdit )
                {
                    new System.Threading.Thread(() =>
                        {
                            Classes.LocationManager locationManager = new Classes.LocationManager();

                            if( locationManager.CanAddProvince(GetNewProvinceName()))
                            {
                                if( locationManager.ChangeProvinceName(provinceToEdit, GetNewProvinceName() ))
                                {
                                    DisplayNotifyBox(provinceToEdit + " Edited", provinceToEdit + " was changed to " + GetNewProvinceName(),2);
                                    isEdited = true;
                                }
                                else
                                {
                                    DisplayNotifyBox("Could not edit " + provinceToEdit, provinceToEdit + " was not changed. Please try again",3);
                                }

                                if (isEdited == true)
                                {
                                    CloseForm();
                                }
                            }
                            else
                            {
                                DisplayNotifyBox("Province Name Already Exists", "Cannot change " + provinceToEdit + " to " + GetNewProvinceName() + " because " + GetNewProvinceName() + " already exists",3);
                            }

                        }).Start();
                }
                else
                {
                    DisplayNotifyBox("ERROR", "New province name must be different form old province name", 2);
                }
            }
            else
            {
                DisplayNotifyBox("ERROR", "New Province Name cannot be empty", 2);
            }
        }

        #region Form Control
        private void DisplayNotifyBox(string title, string message, int durationSeconds)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, durationSeconds), false);
            });
        }

        private void SetLoadingState(bool loading)
        {
            this.Dispatcher.Invoke(() =>
                {
                    if (loading)
                    {
                        this.IsBusy = true;
                        TB_NewProvinceName.IsEnabled = false;
                        BT_Cancel.IsEnabled = false;
                        BT_Edit.IsEnabled = false;
                    }
                    else
                    {
                        this.IsBusy = false;
                        TB_NewProvinceName.IsEnabled = true;
                        BT_Cancel.IsEnabled = true;
                        BT_Edit.IsEnabled = true;
                    }
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
        private string GetNewProvinceName()
        {
            string newProvinceName = "";

            this.Dispatcher.Invoke(() =>
            {
                newProvinceName = TB_NewProvinceName.Text;
            });

            return newProvinceName;
        }
        #endregion




    }

}
