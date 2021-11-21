using AdminPanel.Commands;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserPanel.Services;

namespace AdminPanel.ViewModels
{
    class StatisticsViewModel: BaseViewModel
    {

        private Statistic _statistic;
        public Statistic Statistic
        {
            get => _statistic;
            set
            {
                _statistic = value;
                OnPropertChanged("Statistic");
            }
        }

        private string _money;

        public string Money
        {
            get => _money;
            set
            {
                _money = value;
                OnPropertChanged("Money");
            }
        }

        public ICommand GetInterestCommand { get; set; }

        public StatisticsViewModel()
        {
            Statistic = JsonSaveService<Statistic>.Load("statistic");


            Money = Statistic.Interest;

            if (Statistic == null)
                Statistic = new Statistic();

            GetInterestCommand = new RelayCommand(a =>
            {
                Statistic.Interest = "0";
                Money = "0";
                Statistic.IncomeAfterLWD = 0;
                JsonSaveService<Statistic>.Save(Statistic, "statistic");

            },
            p =>
            {

                if (Convert.ToDouble(Money) > 0)
                    return true;
                return false;
            });
        }

    }
}
