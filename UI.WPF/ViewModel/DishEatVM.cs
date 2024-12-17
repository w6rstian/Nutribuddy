using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class DishEatVM : ViewModelBase
    {
        private object _currentView;
        private string _currentViewName;
        private Dish _dish;
        public string Name { get; }
        public List<FoodItem> Ingredients { get; }
        public Dictionary<string, double> TotalNutrients { get; }

        private EatHistoryController _eatHistoryController;

        public ICommand ConfirmEatDishCommand { get; set; }

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

        public Dish Dish
        {
            get => _dish;
            set
            {
                _dish = value;
                OnPropertyChanged();
            }
        }
        private void ConfirmEatDish(object obj)
        {
            if (_dish != null)
            {
                _eatHistoryController.AddDishToHistory(DateTime.Now, _dish);
            }
        }

        public DishEatVM(Dish selectedDish)
        {
            Dish = selectedDish;
            Name = selectedDish.Name;
            Ingredients = selectedDish.Ingredients ?? new List<FoodItem>();
            TotalNutrients = selectedDish.TotalNutrients ?? new Dictionary<string, double>();

            _eatHistoryController = new EatHistoryController(
                "C:\\Users\\kszym\\Source\\Repos\\Nutribuddy\\Data\\FoodHistory.json", // Path to FoodHistory.json
                "C:\\Users\\Administraotr\\Source\\Repos\\Nutribuddy\\Data\\DishHistory.json" // Path to DishHistory.json
                );

            ConfirmEatDishCommand = new RelayCommand(ConfirmEatDish);
        }
    }
}
