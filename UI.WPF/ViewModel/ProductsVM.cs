using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class ProductsVM : ViewModelBase
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

        private readonly FoodController _foodController;
        private readonly EatHistoryController _eatHistoryController;
        private string _searchText;
        private FoodItem _selectedProduct;

        public ObservableCollection<FoodItem> AllProducts { get; set; }
        public ObservableCollection<FoodItem> FilteredProducts { get; set; }

        public ICommand EatProductCommand { get; set; }

        public ProductsVM()
        {
            _foodController = new FoodController("C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\FoodData.json"); // Path to file FoodData
            _eatHistoryController = new EatHistoryController(
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\FoodHistory.json", // Path to FoodHistory.json
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\DishHistory.json" // Path to DishHistory.json
                );

            AllProducts = new ObservableCollection<FoodItem>(_foodController.GetAllFoods());
            FilteredProducts = new ObservableCollection<FoodItem>(AllProducts);

            EatProductCommand = new RelayCommand(EatProduct);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterFoodItems();
            }
        }

        public FoodItem SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        private void FilterFoodItems()
        {
            FilteredProducts.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? AllProducts
                : AllProducts.Where(d => d.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var foodItem in filtered)
                FilteredProducts.Add(foodItem);
        }

        private void EatProduct(object obj)
        {
            if (SelectedProduct != null)
            {
                var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
                navigationVM?.EatProductCommand.Execute(SelectedProduct);
                //_eatHistoryController.AddFoodItemToHistory(DateTime.Now, SelectedProduct);
            }
        }
    }
}
