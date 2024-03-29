﻿using AdminPanel.Models;
using AdminPanel.Services;
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
using UserPanel.Models;
using UserPanel.Services;
using UserPanel.Stores;





namespace UserPanel.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class MapVIewModel : BaseViewModel
    {
        public string path = "";

        public MapVIewModel(NavigationStore NV)
        {
            Centerr = new Location(40.409264, 49.867092);
            Locations = new LocationCollection();

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            Timer.Tick += Timer_Tick;

            string[] dir = Directory.GetCurrentDirectory().Split('\\');
            foreach (var item in dir)
            {
                if (item.ToLower() == "eyetaxi")
                    break;
                path += item + "\\";
            }

            Pricing price = JsonSaveService<Pricing>.Load(path + @"\EyeTaxi\AdminPanel\bin\Debug\pricing");
            if (price == null)
                price = new Pricing();
            PricePerKm = price.PricePerKm;

            Drivers = JsonSaveService<List<Driver>>.Load(path + @"\EyeTaxi\AdminPanel\bin\Debug\driver");


            GoCommand = new RelayCommand(a => ConfirmButton_Click(),
                b => !string.IsNullOrWhiteSpace(FromLocation) && !string.IsNullOrWhiteSpace(ToLocation));
            OrderTaxiCommand = new RelayCommand(a => ApplyButton_Click(),
                b =>
                {
                    if (!string.IsNullOrWhiteSpace(Distance)) return true;
                    else return false;
                });
            NavigateBackCommand = new UpdateViewCommand<RegisterViewModel>(NV, () => new RegisterViewModel(NV));
            CancelRideCommand = new RelayCommand(a => CancelRideButton_Click());
            NavigateHistoryCommand = new UpdateViewCommand<HistoryViewModel>(NV, () => new HistoryViewModel(NV) { HistoryUsr = Usr });
            NavigateRatingCommand = new UpdateViewCommand<RatingViewModel>(NV, () => new RatingViewModel(NV));
        }


        public ICommand GoCommand { get; set; }

        public ICommand OrderTaxiCommand { get; set; }

        public ICommand NavigateBackCommand { get; set; }

        public ICommand NavigateHistoryCommand { get; set; }

        public ICommand CancelRideCommand { get; set; }

        public ICommand NavigateRatingCommand { get; set; }


        public string FromLocation { get; set; }

        public string ToLocation { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Distance { get; set; }

        public string Price { get; set; }

        private float PricePerKm;

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

        bool PickedUp = false;




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

        public Visibility StackVisibility2 { get; set; } = Visibility.Visible;

        public static User Usr { get; set; }

        public static List<User> Users { get; set; }




        private void Timer_Tick(object sender, EventArgs e)
        {
            if (PickedUp == false)
            {
                if (1 < Locations.Count)
                {
                    Locations.Remove(Locations.First());
                    TaxiLocation = Locations.First().Latitude + ", " + Locations.First().Longitude;
                    Centerr = new Location(double.Parse(TaxiLocation.Split(',')[0]), double.Parse(TaxiLocation.Split(',')[1]));
                }
                else
                {
                    PickedUp = true;
                    Timer.Stop();
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

                    var history = new History(driver, FromLocation, ToLocation, float.Parse(Price), Distance);
                    Users.Find(u => u.Username == Usr.Username).Histories.Add(history);
                    JsonSaveService<List<User>>.Save(Users, "Users");

                    CreateStatisticService.SetStatistic(float.Parse(Price));
                    Timer.Stop();

                    Drivers.Find(d => d.Id == driver.Id).LastLocation = new Location(double.Parse(TaxiLocation.Split(',')[0]), double.Parse(TaxiLocation.Split(',')[1]));
                    Drivers.Find(d => d.Id == driver.Id).Balance += float.Parse(Price);
                    JsonSaveService<List<Driver>>.Save(Drivers, path + @"\EyeTaxi\AdminPanel\bin\Debug\driver");


                    PickedUp = false;
                    System.Windows.MessageBox.Show("Taxi Arrived!!!");
                    TaxiVisible = Visibility.Hidden;
                    To = default;
                    From = default;
                    TaxiLocation = default;
                    IsVisiblePin1 = Visibility.Visible;
                    IsVisiblePin2 = Visibility.Visible;
                    TaxiVisible = Visibility.Hidden;
                    StackVisibility = Visibility.Hidden;
                    StackVisibility2 = Visibility.Visible;
                    ToLocation = "";
                    FromLocation = "";
                    Distance = "";
                    Price = "";
                    Centerr = new Location(40.409264, 49.867092);

                    RatingViewModel.Driver = driver;
                    RatingViewModel.Drivers = Drivers;

                    NavigateRatingCommand.Execute(sender);
                }
            }
        }



        public void ApplyButton_Click()
        {
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
                StackVisibility = Visibility.Visible;
                StackVisibility2 = Visibility.Hidden;
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


        public void CancelRideButton_Click()
        {
            if (PickedUp == true)
            {
                PickedUp = false;

                Drivers.Find(d => d.Id == driver.Id).Balance += float.Parse(Price);
            }


            Timer.Stop();
            Drivers.Find(d => d.Id == driver.Id).LastLocation = new Location(double.Parse(TaxiLocation.Split(',')[0]), double.Parse(TaxiLocation.Split(',')[1]));

            JsonSaveService<List<Driver>>.Save(Drivers, path + @"\EyeTaxi\AdminPanel\bin\Debug\driver");


            Locations.Clear();
            System.Windows.MessageBox.Show("You have cancelled the ride!!!");
            TaxiVisible = Visibility.Hidden;
            To = default;
            From = default;
            TaxiLocation = default;
            IsVisiblePin1 = Visibility.Visible;
            IsVisiblePin2 = Visibility.Visible;
            TaxiVisible = Visibility.Hidden;
            StackVisibility = Visibility.Hidden;
            StackVisibility2 = Visibility.Visible;
            ToLocation = "";
            FromLocation = "";
            Distance = "";
            Price = "";
            Centerr = new Location(40.409264, 49.867092);
            Locations.Clear();
        }


        public void ConfirmButton_Click()
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
                Distance = dist.ToString("0.##") + "  km";
                Price = (dist * PricePerKm).ToString("0.##");
                if (float.Parse(Price) < 1.6f)
                    Price = "1.6";
            }
        }


    }



}
