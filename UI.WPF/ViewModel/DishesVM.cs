using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using Nutribuddy.UI.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class DishesVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        private readonly DishController _dishController;
        private string _searchText;
        private Dish _selectedDish;

        public ObservableCollection<Dish> AllDishes { get; set; }
        public ObservableCollection<Dish> FilteredDishes { get; set; }

        public ICommand AddDishCommand { get; set; }
        public ICommand EditDishCommand { get; set; } // może tu zamiast edit to view (skład) i tam możliwośc edycji itp
        public ICommand DeleteDishCommand { get; }

        public DishesVM()
        {
            _dishController = new DishController("C:\\Users\\kszym\\Source\\Repos\\Nutribuddy\\Data\\DishData.json"); // Path to file DishData

            AllDishes = new ObservableCollection<Dish>(_dishController.GetAllDishes());
            FilteredDishes = new ObservableCollection<Dish>(AllDishes);

            AddDishCommand = new RelayCommand(AddDish);
            EditDishCommand = new RelayCommand(EditDish);
            DeleteDishCommand = new RelayCommand(DeleteDish);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterDishes();
            }
        }

        public Dish SelectedDish
        {
            get => _selectedDish;
            set
            {
                _selectedDish = value;
                OnPropertyChanged();
            }
        }

        private void FilterDishes()
        {
            FilteredDishes.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? AllDishes
                : AllDishes.Where(d => d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var dish in filtered)
                FilteredDishes.Add(dish);
        }

        // w przyszlosci przejscia do widokow wlasciwych
        private void AddDish(object obj) => CurrentView = new DishesVM();

        private void EditDish(object obj) => CurrentView = new DishesVM();

        private void DeleteDish(object obj)
        {
            if (SelectedDish != null)
            {
                _dishController.DeleteDish(SelectedDish.Name);
                AllDishes.Remove(SelectedDish);
                FilterDishes();
            }
        }
    }
}

