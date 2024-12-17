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
        public int createOrEditState = 0;
        private object _currentView;
        private string _currentViewName;
        public Dish _tempDish;
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

        public Dish TempDish
        {
            get { return _tempDish; }
            set
            {
                _tempDish = value;
                OnPropertyChanged();
            }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DishesCommand { get; set; }
        public ICommand EditDishCommand { get; set; }
        public ICommand ContinueEditingDishCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand UserDataCommand { get; set; }
        public ICommand NutrientsSummaryCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand MealsCommand { get; set; }
        public ICommand EatDishCommand { get; set; }
        public ICommand EatProductCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand CreateDishCommand { get; set; }
        public ICommand ContinueCreatingDishCommand { get; set; }
        public ICommand SaveTempDishCommand { get; set; }
        public ICommand AddIngredientCommand { get; set; }
        public ICommand AddIngredientForEditCommand { get; set; }
        public ICommand EditIngredientCommand { get; set; }

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
                _tempDish = selectedDish;
                CurrentView = new EditDishVM(selectedDish);
                CurrentViewName = "Dishes";
            }
        }

        private void ContinueEditingDish(object obj)
        {
            if (obj is Dish selectedDish)
            {
                CurrentView = new EditDishVM();
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

        private void EditIngredient(object obj)
        {
            if (obj is FoodItem selectedProduct)
            {
                CurrentView = new ProductEditVM(selectedProduct);
                CurrentViewName = "Products";
            }
        }

        private void Products(object obj)
        {
            CurrentView = new ProductsVM();
            CurrentViewName = "Products";
        }

        private void UserData(object obj)
        {
            CurrentView = new UserDataVM();
            CurrentViewName = "Profile";
        }

        private void NutrientsSummary(object obj)
        {
            CurrentView = new NutrientsSummaryVM();
            CurrentViewName = "Profile";
        }

        private void Settings(object obj)
        {
            CurrentView = new SettingsVM();
            CurrentViewName = "Settings";
        }

        private void CreateDish(object obj)
        {
            _tempDish = new Dish();
            CurrentView = new CreateDishVM();
            CurrentViewName = "Dishes";
        }

        private void ContinueCreatingDish(object obj)
        {
            CurrentView = new CreateDishVM();
            CurrentViewName = "Dishes";
        }

        private void AddIngredient(object obj)
        {
            createOrEditState = 0;
            CurrentView = new DishChooseProductVM();
            CurrentViewName = "Dishes";
        }

        private void AddIngredientForEdit(object obj)
        {
            createOrEditState = 1;
            CurrentView = new DishChooseProductVM();
            CurrentViewName = "Dishes";
        }

        private void SaveTemporaryDish(object obj)
        {
            if (obj is Dish temporaryDish)
            {
                _tempDish = temporaryDish;
            }
        }

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            DishesCommand = new RelayCommand(Dishes);
            EditDishCommand = new RelayCommand(EditDish);
            ProfileCommand = new RelayCommand(Profile);
            UserDataCommand = new RelayCommand(UserData);
            SettingsCommand = new RelayCommand(Settings);
            MealsCommand = new RelayCommand(Meals);
            EatDishCommand = new RelayCommand(EatDish);
            EatProductCommand = new RelayCommand(EatProduct);
            ProductsCommand = new RelayCommand(Products);
            NutrientsSummaryCommand = new RelayCommand(NutrientsSummary);
            CreateDishCommand = new RelayCommand(CreateDish);
            SaveTempDishCommand = new RelayCommand(SaveTemporaryDish);
            AddIngredientCommand = new RelayCommand(AddIngredient);
            ContinueCreatingDishCommand = new RelayCommand(ContinueCreatingDish);
            AddIngredientForEditCommand = new RelayCommand(AddIngredientForEdit);
            EditIngredientCommand = new RelayCommand(EditIngredient);
            ContinueEditingDishCommand = new RelayCommand(ContinueEditingDish);

            CurrentView = new HomeVM();
            CurrentViewName = "Home";
        }
    }
}
