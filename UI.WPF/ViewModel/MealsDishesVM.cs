using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class MealsDishesVM : ViewModelBase
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

        private readonly DishController _dishController;
        private readonly EatHistoryController _eatHistoryController;
        private string _searchText;
        private Dish _selectedDish;

        public ObservableCollection<Dish> AllDishes { get; set; }
        public ObservableCollection<Dish> FilteredDishes { get; set; }

        public ICommand EatDishCommand { get; set; }

        public MealsDishesVM()
        {
            _dishController = new DishController("C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\DishData.json"); // Path to file DishData
            _eatHistoryController = new EatHistoryController(
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\FoodHistory.json", // Path to FoodHistory.json
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\DishHistory.json" // Path to DishHistory.json
                );

            AllDishes = new ObservableCollection<Dish>(_dishController.GetAllDishes());
            FilteredDishes = new ObservableCollection<Dish>(AllDishes);

            EatDishCommand = new RelayCommand(EatDish);
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

        private void EatDish(object obj)
        {
            if (SelectedDish != null)
            {
                _eatHistoryController.AddDishToHistory(DateTime.Now, SelectedDish);
            }
        }
    }
}
