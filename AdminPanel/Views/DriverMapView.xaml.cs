using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
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

namespace AdminPanel.Views
{
    /// <summary>
    /// Interaction logic for DriverMapView.xaml
    /// </summary>
    public partial class DriverMapView : UserControl
    {
        public DriverMapView()
        {
            InitializeComponent();
            Map.CredentialsProvider = new ApplicationIdCredentialsProvider(ConfigurationManager.AppSettings["MapKey"]);
        }
    }
}
