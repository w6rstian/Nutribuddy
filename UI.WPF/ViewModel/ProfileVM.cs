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

        // TODO: DODAC OBSLUGE RESZTY WIDOKOW JAK POWSTANA WIDOKI

        public ICommand SettingsCommand { get; set; }

        private void Settings(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.SettingsCommand.Execute(null);
        }

        public ProfileVM()
        {
            SettingsCommand = new RelayCommand(Settings);
        }
    }
}
