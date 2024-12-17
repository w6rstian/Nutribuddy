using Nutribuddy.Core.Models;
using Nutribuddy.Core.Controllers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class CreateDishVM : ViewModelBase
    {
        private readonly DishController _dishController;
        private string _dishName;
        private string _newIngredientDescription;
        private string _newIngredientQuantity;
        private Dish _tempDish;

        public ObservableCollection<FoodItem> Ingredients { get; set; }
        public FoodItem SelectedIngredient { get; set; }

        public string DishName
        {
            get => _dishName;
            set { _dishName = value; OnPropertyChanged(); }
        }

        public string NewIngredientDescription
        {
            get => _newIngredientDescription;
            set { _newIngredientDescription = value; OnPropertyChanged(); }
        }

        public string NewIngredientQuantity
        {
            get => _newIngredientQuantity;
            set { _newIngredientQuantity = value; OnPropertyChanged(); }
        }

        public Dish TempDish
        {
            get
            {
                var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
                _tempDish = navigationVM.TempDish;
                return _tempDish;
            }
            set
            {
                var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
                navigationVM.TempDish = value;
                _tempDish = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddIngredientCommand { get; set; }
        public ICommand RemoveIngredientCommand { get; set; }
        public ICommand SaveDishCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public CreateDishVM()
        {
            _dishController = new DishController("C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\DishData.json");

            Ingredients = [.. TempDish.Ingredients]; // Ingredients jest tworzone na podstawie TempDish.Ingredients

            AddIngredientCommand = new RelayCommand(AddIngredient);
            RemoveIngredientCommand = new RelayCommand(RemoveIngredient);
            SaveDishCommand = new RelayCommand(SaveDish);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void AddIngredient(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.AddIngredientCommand.Execute(null);
        }

        private void RemoveIngredient(object obj)
        {
            if (SelectedIngredient != null)
            {
                _tempDish.Ingredients.Remove(SelectedIngredient);
                Ingredients = new ObservableCollection<FoodItem>(_tempDish.Ingredients);
                OnPropertyChanged(nameof(Ingredients));
            }
        }

        private bool CanAddIngredient()
        {
            return !string.IsNullOrWhiteSpace(NewIngredientDescription) && double.TryParse(NewIngredientQuantity, out _);
        }

        private void SaveDish(object obj)
        {
            if (CanSaveDish())
            {
                var newDish = new Dish
                {
                    Name = DishName,
                    Ingredients = new List<FoodItem>(Ingredients)
                };
                _dishController.AddDish(newDish);
                OnPropertyChanged();
                DishName = string.Empty;
                Ingredients.Clear();
                var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
                navigationVM?.DishesCommand.Execute(null);
            }
        }

        private bool CanSaveDish()
        {
            return !string.IsNullOrWhiteSpace(DishName) && Ingredients.Count > 0;
        }

        private void Cancel(object obj)
        {
            DishName = string.Empty;
            Ingredients.Clear();
            _tempDish.Ingredients.Clear();
            OnPropertyChanged(nameof(Ingredients));
        }
    }
}

