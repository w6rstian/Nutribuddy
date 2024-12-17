using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MealsVM()
        {

        }
    }
}
