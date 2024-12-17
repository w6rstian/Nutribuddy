using Nutribuddy.UI.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class HomeVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand ProfileCommand { get; set; }

        public ICommand UserDataCommand { get; set; }

        public ICommand SettingsCommand {  get; set; }

        public ICommand MealsCommand { get; set; }

        public ICommand DishesCommand { get; set; }

        public ICommand ProductsCommand { get; set; }

        // TODO: DODAC OBSLUGE RESZTY PRZYCISKOW JAK POWSTANA WIDOKI
        private void Profile(object obj)
        {
            // This will trigger a command in the NavigationVM to update the view.
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.ProfileCommand.Execute(null);
        }

        private void Meals(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.MealsCommand.Execute(null);
        }

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

        private void UserData(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.UserDataCommand.Execute(null);
        }

        private void Settings(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.SettingsCommand.Execute(null);
        }

        public HomeVM()
        {
            ProfileCommand = new RelayCommand(Profile);
            UserDataCommand = new RelayCommand(UserData);
            SettingsCommand = new RelayCommand(Settings);
            MealsCommand = new RelayCommand(Meals);
            DishesCommand = new RelayCommand(Dishes);
            ProductsCommand = new RelayCommand(Products);

        }
    }
}
