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
    class EditDishVM : ViewModelBase
    {
        private DishController _dishController;
        private Dish _dish;

        public string DishName
        {
            get => _dish.Name;
            set
            {
                _dish.Name = value;
                OnPropertyChanged();
            }
        }

    }
}
