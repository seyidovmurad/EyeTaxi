using AdminPanel.Models;
using AdminPanel.Stores;
using AdminPanel.ViewModels;
using System;
using System.Windows.Input;

namespace AdminPanel.Commands
{
    public class AddDriverCommand<TVViewModel> : ICommand where TVViewModel : BaseViewModel
    {
        private readonly DriverListViewModel _driverListViewModel;
        private readonly NavigationStore _navigationStore;
        private Driver _driver;
        private Predicate<object> CanExecuteDriver;
        public AddDriverCommand(DriverListViewModel driverListViewModel, Driver driver, NavigationStore navigationStore,Predicate<object> predicate)
        {
            _driverListViewModel = driverListViewModel;
            _driver = driver;
            _navigationStore = navigationStore;
            CanExecuteDriver = predicate;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            _driverListViewModel.driverStore.Add(_driver);
            _navigationStore.SelectedViewModel = _driverListViewModel;
        }
    }
}
