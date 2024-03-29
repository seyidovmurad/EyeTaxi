﻿using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using UserPanel.Commands;
using UserPanel.Services;
using UserPanel.Stores;
using UserPanel.ViewModels;
using static UserPanel.Services.GetAdressService;

namespace UserPanel.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public bool Help { get; set; } = false;

        public ArrayList Pins { get; set; } = new ArrayList();

        public AutoSuggestService Auto { get; set; } = new AutoSuggestService();


        public MapView()
        {
            InitializeComponent();
            Map.CredentialsProvider = new ApplicationIdCredentialsProvider(ConfigurationManager.AppSettings["MapKey"]);

            FromLocation.FilterMode = AutoCompleteFilterMode.Contains;
            FromLocation.ItemsSource = new string[] { };
            ToLocation.FilterMode = AutoCompleteFilterMode.Contains;
            ToLocation.ItemsSource = new string[] { };

            
        }


        private void ChckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ChckBox.IsChecked == true)
            {
                FromLocation.IsEnabled = false;
                ToLocation.IsEnabled = false;
            }
            if (ChckBox.IsChecked == false)
            {
                FromLocation.IsEnabled = true;
                ToLocation.IsEnabled = true;
            }

            Distance.Content = "";
            Pin1.Location = default;
            Pin2.Location = default;
            FromLocation.Text = "";
            ToLocation.Text = "";
            Help = false;
        }


        private void Map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ChckBox.IsChecked == true)
            {
                e.Handled = true;
                Point mousePosition = e.GetPosition(this);
                Location pinLocation = Map.ViewportPointToLocation(mousePosition);
                RootObject rootObject = getAddress(pinLocation.Latitude, pinLocation.Longitude);

                if (Help == false)
                {
                    Pin2.Location = default;
                    Route.Locations.Clear();
                    FromLocation.Text = "";
                    ToLocation.Text = "";
                    Distance.Content = "";

                    FromLocation.Text = rootObject.display_name;
                    Pin1.Location = pinLocation;
                    Help = true;
                }
                else
                {
                    ToLocation.Text = rootObject.display_name;
                    Pin2.Location = pinLocation;
                    Help = false;
                }
                Map.Center = pinLocation;
            }
        }


        private void FromLocation_TextChanged(object sender, RoutedEventArgs e)
        {
            if (FromLocation.Text.Length > 2 && FromLocation.Text.Length < 10)
                FromLocation.ItemsSource = Auto.GetResponse(FromLocation.Text, "40.409264,49.867092", "30000");
            else if (FromLocation.Text.Length == 0)
            {
                FromLocation.ItemsSource = new string[] { };
            }
        }

        private void ToLocation_TextChanged(object sender, RoutedEventArgs e)
        {
            if (ToLocation.Text.Length > 2 && ToLocation.Text.Length < 10)
                ToLocation.ItemsSource = Auto.GetResponse(ToLocation.Text, "40.409264,49.867092", "30000");
            else if (ToLocation.Text.Length == 0)
            {
                ToLocation.ItemsSource = new string[] { };
            }
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

      
    }
}