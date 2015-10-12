using System;
using System.Linq;
using System.Windows;
using Microsoft.Maps.MapControl.WPF;

namespace RealEstate.BingMaps.Core
{
    public class MapInteractivity
    {
        #region GeocodeResult

        public static readonly DependencyProperty GeocodeResultProperty = DependencyProperty.RegisterAttached("GeocodeResult", typeof(BingMapsService.GeocodeResult), typeof(MapInteractivity), new UIPropertyMetadata(null, OnGeocodeResultChanged));
        public static BingMapsService.GeocodeResult GetGeocodeResult(Map target)
        {
            return (BingMapsService.GeocodeResult)target.GetValue(GeocodeResultProperty);
        }
        public static void SetGeocodeResult(Map target, BingMapsService.GeocodeResult value)
        {
            target.SetValue(GeocodeResultProperty, value);
        }
        private static void OnGeocodeResultChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnGeocodeResultChanged((Map)o, (BingMapsService.GeocodeResult)e.OldValue, (BingMapsService.GeocodeResult)e.NewValue);
        }
        private static void OnGeocodeResultChanged(Map map, BingMapsService.GeocodeResult oldValue, BingMapsService.GeocodeResult newValue)
        {
            Location location = newValue.Locations.Select(x => new Location(x.Latitude, x.Longitude)).First();

            Pushpin pin = new Pushpin();
            pin.Location = location;
            pin.ToolTip = newValue.Address.FormattedAddress;

            var locationLayer = GetGeocodeResultLayer(map);
            if (locationLayer == null)
            {
                locationLayer = new MapLayer();
                SetGeocodeResultLayer(map, locationLayer);
            }

            locationLayer.Children.Clear();
            locationLayer.Children.Add(pin);

            map.SetView(location, map.ZoomLevel);
        }

        #endregion //GeocodeResult

        #region GeocodeResultLayer

        public static readonly DependencyProperty GeocodeResultLayerProperty = DependencyProperty.RegisterAttached("GeocodeResultLayer", typeof(MapLayer), typeof(MapInteractivity), new UIPropertyMetadata(null, OnGeocodeResultLayerChanged));
        public static MapLayer GetGeocodeResultLayer(DependencyObject target)
        {
            return (MapLayer)target.GetValue(GeocodeResultLayerProperty);
        }
        public static void SetGeocodeResultLayer(DependencyObject target, MapLayer value)
        {
            target.SetValue(GeocodeResultLayerProperty, value);
        }
        private static void OnGeocodeResultLayerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnGeocodeResultLayerChanged((Map)o, (MapLayer)e.OldValue, (MapLayer)e.NewValue);
        }
        private static void OnGeocodeResultLayerChanged(Map map, MapLayer oldValue, MapLayer newValue)
        {
            map.Children.Add(newValue);
        }

        #endregion //GeocodeResultLayer
    }
}
