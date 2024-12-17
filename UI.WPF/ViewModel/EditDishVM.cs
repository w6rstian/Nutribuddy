using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class EditDishVM : ViewModelBase
    {
        private readonly DishController _dishController;
        private Dish _dish;
        private FoodItem _selectedIngredient;
        private string _searchText;

        public Dish Dish
        {
            get => _dish;
            set
            {
                _dish = value;
                OnPropertyChanged();
                FilterIngredients();
            }
        }

        public string DishName
        {
            get => _dish.Name;
            set
            {
                _dish.Name = value;
                OnPropertyChanged();
            }
        }

        public FoodItem SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterIngredients();
            }
        }

        public ObservableCollection<FoodItem> FilteredIngredients { get; set; }
        public ObservableCollection<FoodItem> AllIngredients { get; set; }

        public ICommand AddIngredientCommand { get; }
        public ICommand EditIngredientCommand { get; }
        public ICommand RemoveIngredientCommand { get; }
        public ICommand SaveCommand { get; }

        public EditDishVM()
        {
            _dishController = new DishController("C:\\Users\\Administrator\\Source\\Repos\\Nutribuddy\\Data\\DishData.json");
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            Dish = navigationVM?.TempDish;

            AllIngredients = new ObservableCollection<FoodItem>(_dish.Ingredients);
            FilteredIngredients = new ObservableCollection<FoodItem>(AllIngredients);

            AddIngredientCommand = new RelayCommand(AddIngredient);
            EditIngredientCommand = new RelayCommand(EditIngredient);
            RemoveIngredientCommand = new RelayCommand(RemoveIngredient);
            SaveCommand = new RelayCommand(SaveDish);

            FilterIngredients();
        }

        public EditDishVM(Dish dish)
        {
            _dishController = new DishController("C:\\Users\\Administrator\\Source\\Repos\\Nutribuddy\\Data\\DishData.json");
            Dish = dish;

            AllIngredients = new ObservableCollection<FoodItem>(_dish.Ingredients);
            FilteredIngredients = new ObservableCollection<FoodItem>(AllIngredients);

            AddIngredientCommand = new RelayCommand(AddIngredient);
            EditIngredientCommand = new RelayCommand(EditIngredient);
            RemoveIngredientCommand = new RelayCommand(RemoveIngredient);
            SaveCommand = new RelayCommand(SaveDish);

            FilterIngredients();
        }

        //do naprawy ale usuwanie i zapisanie zmian działa
        private void FilterIngredients()
        {
            if (FilteredIngredients != null)
            {
                FilteredIngredients.Clear();
            }

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? AllIngredients
                : new ObservableCollection<FoodItem>(
                    AllIngredients.Where(i => i.Description.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)));

            if (filtered != null)
            {
                foreach (var item in filtered)
                    FilteredIngredients.Add(item);
            }

            OnPropertyChanged(nameof(FilteredIngredients));
        }

        private void AddIngredient(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.SaveTempDishCommand.Execute(Dish);
            navigationVM?.AddIngredientForEditCommand.Execute(null);
        }

        private void EditIngredient(object obj)
        {
            if (SelectedIngredient != null)
            {
                var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
                navigationVM?.EditIngredientCommand.Execute(SelectedIngredient);
            }
        }

        private void RemoveIngredient(object obj)
        {
            if (SelectedIngredient != null)
            {
                //Dish.Ingredients.Remove(SelectedIngredient);

                AllIngredients.Remove(SelectedIngredient);
                Dish.Ingredients.Remove(SelectedIngredient);

                FilterIngredients();
                OnPropertyChanged(nameof(FilteredIngredients));
            }
        }

        private void SaveDish(object obj)
        {
            _dishController.EditDish(Dish.Name, d =>
            {
                d.Name = Dish.Name;
                d.Ingredients = Dish.Ingredients;
            });
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.DishesCommand.Execute(null);
        }
    }
}