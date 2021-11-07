using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
using static UserPanel.Services.ReverseGeocodingService;

namespace UserPanel.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public bool Help { get; set; } = false;

        public ArrayList Pins { get; set; } = new ArrayList();

        public MapView()
        {
            InitializeComponent();
            Map.CredentialsProvider = new ApplicationIdCredentialsProvider(ConfigurationManager.AppSettings["MapKey"]);
        }

        private void ChckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ChckBox.IsChecked == true)
            {
                FromLocation.IsEnabled = false;
                ToLocation.IsEnabled = false;
                Pin2.Visibility = Visibility.Hidden;
            }
            if (ChckBox.IsChecked == false)
            {
                FromLocation.IsEnabled = true;
                ToLocation.IsEnabled = true;
                Pin2.Visibility = Visibility.Visible;
                Pin1.Location = default;
                Pin2.Location = default;

                foreach (var item in Pins)
                {
                    Map.Children.Remove(item as Pushpin);
                }
                Pins.Clear();
            }

            Route.Locations.Clear();
            FromLocation.Clear();
            ToLocation.Clear();
            Help = false;
        }


        private void Map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ChckBox.IsChecked == true)
            {
                e.Handled = true;

                Point mousePosition = e.GetPosition(this);
                Location pinLocation = Map.ViewportPointToLocation(mousePosition);

                Pushpin pin = new Pushpin();
                pin.Location = pinLocation;

                if (Help == false)
                {
                    Route.Locations.Clear();
                    foreach (var item in Pins)
                    {
                        Map.Children.Remove(item as Pushpin);
                    }
                    Pins.Clear();
                    FromLocation.Clear();
                    ToLocation.Clear();

                    //FromLocation.Text = pinLocation.ToString();
                    RootObject rootObject = getAddress(pinLocation.Latitude, pinLocation.Longitude);
                    FromLocation.Text = rootObject.display_name;
                    Pin1.Location = pinLocation;
                    Help = true;
                }
                else
                {
                    RootObject rootObject = getAddress(pinLocation.Latitude, pinLocation.Longitude);
                    ToLocation.Text = rootObject.display_name;
                    Pin2.Location = pinLocation;
                    Help = false;
                }
                Map.Children.Add(pin);
                Pins.Add(pin);
            }
        }

        public static RootObject getAddress(double lat, double lon)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            var jsonData = webClient.DownloadData("http://nominatim.openstreetmap.org/reverse?format=json&lat=" + lat + "&lon=" + lon);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            return rootObject;
        }

    }


    

}