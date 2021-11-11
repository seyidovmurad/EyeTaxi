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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminPanel.Views
{
    /// <summary>
    /// Interaction logic for DriverEditView.xaml
    /// </summary>
    public partial class DriverEditView : UserControl
    {
        public DriverEditView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in a.Children)
            {
                foreach (var item in (child as StackPanel).Children)
                {
                    if (item is StackPanel)
                    {
                        foreach (var last in (item as StackPanel).Children)
                        {
                            if (last is TextBox txtb)
                            {
                                txtb.BorderBrush = Brushes.Red;
                                
                            }
                        }
                    }
                }
            }
        }
    }
}
