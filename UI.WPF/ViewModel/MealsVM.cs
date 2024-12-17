using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class MealsVM : ViewModelBase
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

        // TODO: DODAC OBSLUGE RESZTY WIDOKOW JAK POWSTANA WIDOKI

        public ICommand DishesCommand { get; set; }
        public ICommand ProductsCommand { get; set; }

        private void Dishes(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.DishesCommand.Execute(null);
        }

        private void Products(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.ProductsCommand.Execute(null);
        }

        public MealsVM()
        {
            DishesCommand = new RelayCommand(Dishes);
            ProductsCommand = new RelayCommand(Products);
        }
    }
}
