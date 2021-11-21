using AdminPanel.Commands;
using AdminPanel.Stores;
using AdminPanel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminPanel.ViewModels
{
    class MainViewModel : BaseViewModel
    {

        private readonly NavigationStore _navigation;
        public BaseViewModel SelectedViewModel { get; set; }

        public ICommand NavigateDriverListCommand { get; set; }

        public ICommand NavigateAddDriverCommand { get; set; }

        public ICommand NavigateDriverPricingCommand { get; set; }

        public ICommand NavigateStatisticsCommand { get; set; }

        public MainViewModel (NavigationStore navigation)
        {
            _navigation = navigation;
            _navigation.SelectedViewModelChanged += OnSelectedViewChanged;
            NavigateAddDriverCommand = new UpdateViewCommand<DriverEditViewModel>(navigation, () => new DriverEditViewModel(navigation));
            NavigateDriverListCommand = new UpdateViewCommand<DriverListViewModel>(navigation, () => new DriverListViewModel(navigation));
            NavigateDriverPricingCommand = new UpdateViewCommand<PricingViewModel>(navigation, () => new PricingViewModel());
            NavigateStatisticsCommand = new UpdateViewCommand<StatisticsViewModel>(navigation, () => new StatisticsViewModel());
            SelectedViewModel = _navigation.SelectedViewModel;
        }

        protected void OnSelectedViewChanged()
        {
            SelectedViewModel = _navigation.SelectedViewModel;
            OnPropertChanged(nameof(SelectedViewModel));
        }
    }
}
