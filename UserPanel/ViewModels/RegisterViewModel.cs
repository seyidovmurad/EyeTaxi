using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserPanel.Commands;
using UserPanel.Models;
using UserPanel.Services;
using UserPanel.Stores;

namespace UserPanel.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class RegisterViewModel : BaseViewModel
    {

        public List<User> Users { get; set; } = new List<User>(0);

        public ICommand NavigateLoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public ICommand NavigateMapCommand { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPass { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public RegisterViewModel(NavigationStore NV)
        {
            NavigateLoginCommand = new UpdateViewCommand<LoginViewModel>(NV, () => new LoginViewModel(NV));
            RegisterCommand = new RelayCommand(Register);
            Users = JsonSaveService<List<User>>.Load("Users");
            if (Users == null)
                Users = new List<User>(0);

            NavigateMapCommand = new UpdateViewCommand<MapVIewModel>(NV, () => new MapVIewModel(NV));
        }


        public void Register(Object obj)
        {
            if(!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPass) && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname))
            {
                if(Password.Length >= 6)
                {
                    if (Password == ConfirmPass)
                    {
                        var usr = Users.Find(u => u.Username == Username);
                        if (usr == null)
                        {
                            Users.Add(new User(Username, Password));
                            JsonSaveService<object>.Save(Users, "Users");
                            MessageBox.Show("You have registered successfully");
                            MapVIewModel.Usr = usr;
                            MapVIewModel.Users = Users;
                            NavigateMapCommand.Execute(obj);
                        }
                        else
                        {
                            MessageBox.Show("This username already exists :(");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please confirm password correctly :(");
                    }
                }
                else
                {
                    MessageBox.Show("Password must contain minumum 6 symbols");
                }
            }
            else
            {
                MessageBox.Show("Enter all fields");
            }
        }



    }
}
