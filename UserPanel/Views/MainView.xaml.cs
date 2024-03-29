﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserPanel.Services;
using UserPanel.Stores;
using UserPanel.ViewModels;

namespace UserPanel.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {

        public MainView()
        {
            InitializeComponent();

            NavigationStore navigationStore = new NavigationStore();
            navigationStore.SelectedViewModel = new LoginViewModel(navigationStore);

            DataContext = new MainViewModel(navigationStore);

            //NavigationStore navigationStore = new NavigationStore();
            //navigationStore.SelectedViewModel = new RegisterViewModel(navigationStore);

            //DataContext = new MainViewModel(navigationStore);
            
        }

        private void Drag_Window(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
