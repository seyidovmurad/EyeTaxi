using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserPanel.Commands;
using UserPanel.Services;
using UserPanel.Stores;

namespace UserPanel.ViewModels
{
    class RatingViewModel : BaseViewModel
    {

        public RatingViewModel(NavigationStore NV)
        {
            NavigateBackCommand = new UpdateViewCommand<MapVIewModel>(NV, () => new MapVIewModel(NV));
            OkCommand = new RelayCommand(OkButton_Click, a => { if (Rating != 0) return true; else return false; });
            CancelCommand = new RelayCommand(CancelButton_Click);
        }

        public float Rating { get; set; }

        public static Driver Driver { get; set; }

        public static List<Driver> Drivers { get; set; }

        public ICommand NavigateBackCommand { get; set; }

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public void OkButton_Click(Object obj)
        {
            Drivers.Find(dr => dr.Id == Driver.Id).Rating = Rating;
            //Salam meyki
            string[] dir = Directory.GetCurrentDirectory().Split('\\');
            string path = "";
            foreach (var item in dir)
            {
                if (item.ToLower() == "eyetaxi")
                    break;
                path += item + "\\";
            }
            JsonSaveService<List<Driver>>.Save(Drivers, path + @"\EyeTaxi\AdminPanel\bin\Debug\driver");

            NavigateBackCommand.Execute(obj);
        }

        public void CancelButton_Click(Object obj)
        {
            NavigateBackCommand.Execute(obj);
        }
    }

}
