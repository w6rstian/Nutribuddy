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
    class ProductEatVM : ViewModelBase
    {
        private FoodItem _product;
        private double _quantity;
        public string Description { get; set; }
        public Dictionary<string, double> Nutrients { get; set; }
        private EatHistoryController _eatHistoryController;

        public ICommand ConfirmEatProductCommand { get; set; }

        public FoodItem Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        public double Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ConfirmEatProduct(object obj)
        {
            if (_product != null && _quantity > 0)
            {
                FoodItem addedProduct = new FoodItem();
                addedProduct.Description = Description;
                addedProduct.Nutrients = Nutrients;
                addedProduct.QuantityInGrams = _quantity;
                _eatHistoryController.AddFoodItemToHistory(DateTime.Now, addedProduct);
            }
        }

        public ProductEatVM(FoodItem selectedProduct)
        {
            Product = selectedProduct;
            Description = selectedProduct.Description;
            Nutrients = selectedProduct.Nutrients;

            _eatHistoryController = new EatHistoryController(
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\FoodHistory.json", // Path to FoodHistory.json
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\DishHistory.json" // Path to DishHistory.json
                );

            ConfirmEatProductCommand = new RelayCommand(ConfirmEatProduct);
        }
    }
}
