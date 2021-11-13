using AdminPanel.Models;
using Microsoft.Maps.MapControl.WPF;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UserPanel.Commands;
using UserPanel.Services;
using UserPanel.Stores;

namespace UserPanel.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class MapVIewModel : BaseViewModel
    {


        public MapVIewModel(NavigationStore NV)
        {
            Centerr = new Location();
            PushPinLocations = new List<Location>(3);
            Locations = new LocationCollection();

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;

            
            GoCommand = new RelayCommand(a =>
            {
                Locations.Clear();
                if (ChckBox == false)
                {
                    Centerr = GetRouteService.GetRoute(FromLocation, ToLocation, Locations);
                    if (Locations.Count != 0)
                    {
                        From = Locations[0].ToString();
                        To = Locations[Locations.Count - 1].ToString();
                    }
                }
                else
                {
                    Centerr = GetRouteService.GetRoute(From, To, Locations);
                }



                if (Locations.Count > 0)
                {
                    GeoCoordinate ge = new GeoCoordinate(Locations[0].Latitude, Locations[0].Longitude);
                    Distance = (ge.GetDistanceTo(new GeoCoordinate(Locations[Locations.Count - 1].Latitude, Locations[Locations.Count - 1].Longitude)) / 1000).ToString();
                    float dist = float.Parse(Distance);
                    Distance = dist + "  km";
                }
            },

            b => !string.IsNullOrWhiteSpace(FromLocation) && !string.IsNullOrWhiteSpace(ToLocation));


            OrderTaxiCommand = new RelayCommand(a => ApplyButton_Click());
            NavigateBackCommand = new UpdateViewCommand<RegisterViewModel>(NV, () => new RegisterViewModel(NV));
            Centerr = new Location(40.409264, 49.867092);
        }

        public string FromLocation { get; set; }

        public string ToLocation { get; set; }



        public RelayCommand GoCommand { get; set; }

        public RelayCommand OrderTaxiCommand { get; set; }

        public ICommand NavigateBackCommand { get; set; }

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



        private List<Location> _pushPinLocations;

        public List<Location> PushPinLocations
        {
            get => _pushPinLocations;
            set
            {
                _pushPinLocations = value;
                OnPropertyChanged("PushPinLocations");
            }
        }



        private Location _centerr;

        public Location Centerr
        {
            get => _centerr;
            set
            {
                _centerr = value;
                OnPropertyChanged("Centerr");
            }
        }

        public Visibility IsVisiblePin1 { get; set; } = Visibility.Visible;

        public Visibility IsVisiblePin2 { get; set; } = Visibility.Visible;

        public string TaxiLocation { get; set; }

        public Visibility TaxiVisible { get; set; } = Visibility.Hidden;

        public bool ChckBox { get; set; } = false;



        DispatcherTimer Timer;

        bool Help = false;



        private Driver _driver;

        public Driver driver
        {
            get => _driver;
            set
            {
                _driver = value;
                OnPropertyChanged("driver");
            }
        }


        private List<Driver> Drivers;

        public Visibility StackVisibility { get; set; } = Visibility.Hidden;



        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Help == false)
            {
                if (1 < Locations.Count)
                {
                    Locations.Remove(Locations.First());
                    TaxiLocation = Locations.First().Latitude + ", " + Locations.First().Longitude;
                    Centerr = new Location(double.Parse(TaxiLocation.Split(',')[0]), double.Parse(TaxiLocation.Split(',')[1]));
                }
                else
                {
                    Timer.Stop();
                    Help = true;
                    System.Windows.MessageBox.Show("Taxi Arrived!!!");
                    Locations.Clear();
                    IsVisiblePin1 = Visibility.Hidden;
                    IsVisiblePin2 = Visibility.Visible;

                    GetRouteService.GetRoute(TaxiLocation, To, Locations);
                    Timer.Start();
                }
            }

            else
            {
                if (1 < Locations.Count)
                {
                    Locations.Remove(Locations.First());
                    TaxiLocation = Locations.First().Latitude + ", " + Locations.First().Longitude;
                    Centerr = new Location(double.Parse(TaxiLocation.Split(',')[0]), double.Parse(TaxiLocation.Split(',')[1]));
                }
                else
                {
                    Timer.Stop();


                    Drivers.Find(d => d.Id == driver.Id).LastLocation = new Location(double.Parse(TaxiLocation.Split(',')[0]), double.Parse(TaxiLocation.Split(',')[1]));
                    string[] dir = Directory.GetCurrentDirectory().Split('\\');
                    string path = "";
                    foreach (var item in dir)
                    {
                        if (item.ToLower() == "eyetaxi")
                            break;
                        path += item + "\\";
                    }
                    JsonSaveService<List<Driver>>.Save(Drivers, path + @"\EyeTaxi\AdminPanel\bin\Debug\driver");


                    Help = false;
                    System.Windows.MessageBox.Show("Taxi Arrived!!!");
                    TaxiVisible = Visibility.Hidden;
                    To = default;
                    From = default;
                    TaxiLocation = default;
                    IsVisiblePin1 = Visibility.Visible;
                    IsVisiblePin2 = Visibility.Visible;
                    TaxiVisible = Visibility.Hidden;
                    StackVisibility = Visibility.Hidden;
                }
            }
        }



        public void ApplyButton_Click()
        {

            StackVisibility = Visibility.Visible;
            string[] dir = Directory.GetCurrentDirectory().Split('\\');
            string path = "";
            foreach (var item in dir)
            {
                if (item.ToLower() == "eyetaxi")
                    break;
                path += item + "\\";
            }
            Drivers = JsonSaveService<List<Driver>>.Load(path + @"\EyeTaxi\AdminPanel\bin\Debug\driver");



            try
            {
                driver = FindTaxiService.TaxiLocation(new Location(double.Parse(From.Split(',')[0]), double.Parse(From.Split(',')[1])), Drivers);
                TaxiLocation = driver.LastLocation.Latitude + ", " + driver.LastLocation.Longitude;
                MessageBox.Show(driver.Name + " " + driver.Surname);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (TaxiLocation != null)
            {
                Locations.Clear();
                GetRouteService.GetRoute(TaxiLocation, From, Locations);
                TaxiVisible = Visibility.Visible;
                IsVisiblePin2 = Visibility.Hidden;

                Timer.Start();
            }
            else
            {
                MessageBox.Show("Taxi is not found");
                return;
            }
        }


    }
}
