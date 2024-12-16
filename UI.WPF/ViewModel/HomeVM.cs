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
        private void Profile(object obj) => CurrentView = new ProfileVM();
        public HomeVM()
        {
            ProfileCommand = new RelayCommand(Profile);

        }
    }
}
