using Microsoft.Maps.MapControl.WPF;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserPanel.Commands;
using UserPanel.Services;

namespace UserPanel.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class MapVIewModel : BaseViewModel
    {

        public MapVIewModel()
        {
            Centerr = new Microsoft.Maps.MapControl.WPF.Location();
            PushPinLocations = new List<Microsoft.Maps.MapControl.WPF.Location>(3);
            Locations = new LocationCollection();

            GoCommand = new RelayCommand(a =>
            {
                Locations.Clear();
                if(IsVisible == Visibility.Visible)
                {
                    Centerr = GetRouteService.GetRoute(FromLocation, ToLocation, Locations);
                    From = Locations[0].ToString();
                    To = Locations[Locations.Count - 1].ToString();
                }
                else
                {
                    Centerr = GetRouteService.GetRoute(From, To, Locations);
                }
                if (Locations.Count > 0)
                {
                    GeoCoordinate ge = new GeoCoordinate(Locations[0].Latitude, Locations[0].Longitude);
                    Distance = (ge.GetDistanceTo(new GeoCoordinate(Locations[Locations.Count - 1].Latitude, Locations[Locations.Count - 1].Longitude)) / 1000).ToString();
                }
            });
        }


        public string FromLocation { get; set; }

        public string ToLocation { get; set; }



        public RelayCommand GoCommand { get; set; }


        public string From { get; set; }

        public string To { get; set; }


        public string Distance { get; set; }



        private LocationCollection _locations;

        public LocationCollection Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                OnPropertyChanged("Locations");
            }
        }


        private List<Microsoft.Maps.MapControl.WPF.Location> _pushPinLocations;

        public List<Microsoft.Maps.MapControl.WPF.Location> PushPinLocations
        {
            get => _pushPinLocations;
            set
            {
                _pushPinLocations = value;
                OnPropertyChanged("PushPinLocations");
            }
        }



        private Microsoft.Maps.MapControl.WPF.Location _centerr;

        public Microsoft.Maps.MapControl.WPF.Location Centerr
        {
            get => _centerr;
            set
            {
                _centerr = value;
                OnPropertyChanged("Centerr");
            }
        }

        public Visibility IsVisible { get; set; } = Visibility.Visible;


    }


}
