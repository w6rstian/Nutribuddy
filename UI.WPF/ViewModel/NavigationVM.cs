using Nutribuddy.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        private string _currentViewName;
        public string CurrentViewName
        {
            get { return _currentViewName; }
            set { _currentViewName = value; OnPropertyChanged(); }
        }
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DishesCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand MealsCommand { get; set; }

        private void Home(object obj)
        {
            CurrentView = new HomeVM();
            CurrentViewName = "Home";
        }
        private void Dishes(object obj)
        {
            CurrentView = new DishesVM();
            CurrentViewName = "Home";
        }
        private void Profile(object obj)
        {
            CurrentView = new ProfileVM();
            CurrentViewName = "Profile";
        }

        private void Meals(object obj)
        {
            CurrentView = new MealsVM();
            CurrentViewName = "Meals";
        }
        private void Settings(object obj)
        {
            CurrentView = new SettingsVM();
            CurrentViewName = "Settings";
        }

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            DishesCommand = new RelayCommand(Dishes);
            ProfileCommand = new RelayCommand(Profile);
            SettingsCommand = new RelayCommand(Settings);
            MealsCommand = new RelayCommand(Meals);

            CurrentView = new HomeVM();
            CurrentViewName = "Home";
        }
    }
}
