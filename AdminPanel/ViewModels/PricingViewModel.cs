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

        

        public ICommand ChangePricingCommand { get; set; }


        public PricingViewModel()
        {
            

            if(Pricing == null)
            {
                Pricing = new Pricing();
                JsonSaveService<Pricing>.Save(Pricing, "pricing");
            }


            ChangePricingCommand = new RelayCommand(a =>
            {
                JsonSaveService<Pricing>.Save(Pricing, "pricing");
            });


            
        }

    }
}
