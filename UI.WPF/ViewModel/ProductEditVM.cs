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
    class ProductEditVM : ViewModelBase
    {
        private FoodItem _product;
        private double _quantity;
        public string Description { get; set; }
        public Dictionary<string, double> Nutrients { get; set; }
        private DishController _dishController;

        public ICommand ConfirmEditProductCommand { get; set; }

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

        private void ConfirmEditProduct(object obj)
        {
            if (_product != null && _quantity > 0)
            {
                var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
                Dish temporaryDish = navigationVM?.TempDish;
                temporaryDish.Ingredients.Find(ingredient => ingredient.Description == Description).QuantityInGrams = _quantity;
                navigationVM?.SaveTempDishCommand.Execute(temporaryDish);
                navigationVM?.ContinueEditingDishCommand.Execute(temporaryDish);
            }
        }

        public ProductEditVM(FoodItem selectedProduct)
        {
            Product = selectedProduct;
            Description = selectedProduct.Description;
            Nutrients = selectedProduct.Nutrients;

            _dishController = new DishController("C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\DishData.json");

            ConfirmEditProductCommand = new RelayCommand(ConfirmEditProduct);
        }
    }
}
