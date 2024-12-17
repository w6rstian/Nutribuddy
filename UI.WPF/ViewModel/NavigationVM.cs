using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
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
        public ICommand EditDishCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand MealsCommand { get; set; }
        public ICommand EatDishCommand { get; set; }
        public ICommand EatProductCommand { get; set; }
        public ICommand ProductsCommand { get; set; }

        private void Home(object obj)
        {
            CurrentView = new HomeVM();
            CurrentViewName = "Home";
        }
        private void Dishes(object obj)
        {
            CurrentView = new DishesVM();
            CurrentViewName = "Dishes";
        }
        private void EditDish(object obj)
        {
            if (obj is Dish selectedDish)
            {
                CurrentView = new EditDishVM(selectedDish);
                CurrentViewName = "Dishes";
            }
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

        private void EatDish(object obj)
        {
            if (obj is Dish selectedDish)
            {
                CurrentView = new DishEatVM(selectedDish);
                CurrentViewName = "Dishes";
            }
        }

        private void EatProduct(object obj)
        {
            if (obj is FoodItem selectedProduct)
            {
                CurrentView = new ProductEatVM(selectedProduct);
                CurrentViewName = "Products";
            }
        }

        private void Products(object obj)
        {
            CurrentView = new ProductsVM();
            CurrentViewName = "Products";
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
            EditDishCommand = new RelayCommand(EditDish);
            ProfileCommand = new RelayCommand(Profile);
            SettingsCommand = new RelayCommand(Settings);
            MealsCommand = new RelayCommand(Meals);
            EatDishCommand = new RelayCommand(EatDish);
            EatProductCommand = new RelayCommand(EatProduct);
            ProductsCommand = new RelayCommand(Products);

            CurrentView = new HomeVM();
            CurrentViewName = "Home";
        }
    }
}
