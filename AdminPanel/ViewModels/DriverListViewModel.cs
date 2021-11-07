using AdminPanel.Commads;
using AdminPanel.Models;
using AdminPanel.Views;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdminPanel.ViewModels
{
    class DriverListViewModel : BaseViewModel
    {
        public ObservableCollection<Driver> Drivers { get; set; }

        public Driver SelectedDriver { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand ShowCommand { get; set; }

        public DriverListViewModel()
        {
            Drivers = new ObservableCollection<Driver>
            {
                new Driver(200,3,"Murad ","055",new Location(), new Car("a-aa","bmw","red",200)),
                new Driver(200,3,"Kimse","055",new Location(), new Car("a-aa","bmw","red",200)),
                new Driver(200,3,"Nese ","055",new Location(), new Car("a-aa","bmw","red",200))
            };

            AddCommand = new RelayCommand(add =>
            {
                Driver driver = new Driver();
                EditDriver(driver);
            });
            EditCommand = new RelayCommand(edit =>
            {
                EditDriver(SelectedDriver);
            });
            ShowCommand = new RelayCommand(show => { });
            DeleteCommand = new RelayCommand(delete => 
            {
                Driver temp = SelectedDriver;
                if (MessageBox.Show("Do you want to delete this driver?", "Deleting Driver", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    Drivers.Remove(temp);
            });
        }

        private void EditDriver(Driver driver)
        {
            DriverEditViewModel model = new DriverEditViewModel();
            model.Driver = driver;
            DriverEditView editView = new DriverEditView(model);
            //if (editView.ShowDialog() == true)
            //{
            editView.ShowDialog();
                if (!Drivers.Contains(driver))
                    Drivers.Add(model.Driver);
            //}
        }

    }
}
