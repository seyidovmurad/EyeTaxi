using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserPanel.Commands;
using UserPanel.Models;
using UserPanel.Stores;

namespace UserPanel.ViewModels
{
    class HistoryViewModel: BaseViewModel
    {
        public ICommand NavigateBackCommand { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public User HistoryUsr { get; set; }

        public HistoryViewModel(NavigationStore NV)
        {
            NavigateBackCommand = new UpdateViewCommand<MapVIewModel>(NV, () => new MapVIewModel(NV));
        }


    }
}
                                                                                                                                                        