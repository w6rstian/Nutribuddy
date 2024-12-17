using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class ProfileVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }


        public ICommand SettingsCommand { get; set; }
        public ICommand UserDataCommand { get; set; }
        public ICommand NutrientsSummaryCommand { get; set; }

        private void Settings(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.SettingsCommand.Execute(null);
        }

        private void UserData(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.UserDataCommand.Execute(null);
        }

        private void NutrientsSummary(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.NutrientsSummaryCommand.Execute(null);
        }

        public ProfileVM()
        {
            SettingsCommand = new RelayCommand(Settings);
            UserDataCommand = new RelayCommand(UserData);
            NutrientsSummaryCommand = new RelayCommand(NutrientsSummary);
        }
    }
}
