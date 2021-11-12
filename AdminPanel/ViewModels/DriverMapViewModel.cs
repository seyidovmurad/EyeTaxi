using AdminPanel.Commands;
using AdminPanel.Models;
using AdminPanel.Stores;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdminPanel.ViewModels
{
    class DriverMapViewModel : BaseViewModel
    {
        public Driver Driver { get; set; }

        public Location LastLocation { get; set; }

        public ICommand BackCommand { get; set; }

        public DriverMapViewModel(NavigationStore navigation)
        {
            BackCommand = new UpdateViewCommand<DriverListViewModel>(navigation, () => new DriverListViewModel(navigation));
        }
    }
}
