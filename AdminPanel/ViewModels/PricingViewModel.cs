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
    class PricingViewModel: BaseViewModel
    {

        private Pricing _pricing;
        public Pricing Pricing
        {
            get => _pricing;
            set
            {
                _pricing = value;
                OnPropertChanged("Pricing");
            }
        }

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

        public ICommand ChangePricingCommand { get; set; }

        public ICommand GetInterestCommand { get; set; }

        public PricingViewModel()
        {
            Statistic = JsonSaveService<Statistic>.Load("statistic");

            Pricing = JsonSaveService<Pricing>.Load("pricing");

            Money = Statistic.Interest;

            if(Statistic == null)
                Statistic = new Statistic();

            if(Pricing == null)
            {
                Pricing = new Pricing();
                JsonSaveService<Pricing>.Save(Pricing, "pricing");
            }


            ChangePricingCommand = new RelayCommand(a =>
            {
                JsonSaveService<Pricing>.Save(Pricing, "pricing");
            });


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
