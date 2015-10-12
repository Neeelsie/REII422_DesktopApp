using System;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using RealEstate.BingMaps.Core;

namespace RealEstate.BingMaps.ViewModels
{
    public class GeocodeViewModel : INotifyPropertyChanged
    {
        public ICommand GeocodeAddressCommand { get; private set; }

        private BingMapsService.GeocodeResult _geocodeResult;
        public BingMapsService.GeocodeResult GeocodeResult
        {
            get { return _geocodeResult; }
            set
            {
                _geocodeResult = value;
                OnPropertyChanged("GeocodeResult");
            }
        }

        public GeocodeViewModel()
        {
            GeocodeAddressCommand = new DelegateCommand<String>(GeocodeAddress);
        }

        public void GeocodeAddress(string address)
        {
            using (BingMapsService.GeocodeServiceClient client = new BingMapsService.GeocodeServiceClient("CustomBinding_IGeocodeService"))
            {
                client.GeocodeCompleted += (o, e) =>
                {
                    if (e.Error == null)
                    {
                        if (e.Result.Results.Length > 0)
                            GeocodeResult = e.Result.Results[0];
                    }
                };

                BingMapsService.GeocodeRequest request = new BingMapsService.GeocodeRequest();
                request.Credentials = new Credentials() { ApplicationId = "aOyFCmxtUgVSoHjkiyg~BGHaMho-ZThx5-KH-3X2kg~AgxaPsUOnRYhaxfAC6W8Wpx8pd-ZIQI17FRkPkd9llQm4I5-5XpkUYDOcGXqc_x5" };
                request.Query = address;
                client.GeocodeAsync(request);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
