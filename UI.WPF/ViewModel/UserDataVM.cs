using Nutribuddy.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class UserDataVM : ViewModelBase
    {
        private double _weight;
        private double _height;
        private int _age;
        private string _gender;
        private string _physicalActivityLevel;
        private string _goal;
        private double _bmi;
        private double _caloricNeeds;

        public double Weight
        {
            get => _weight;
            set { _weight = value; OnPropertyChanged(); }
        }

        public double Height
        {
            get => _height;
            set { _height = value; OnPropertyChanged(); }
        }

        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(); }
        }

        public string Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged(); }
        }

        public string PhysicalActivityLevel
        {
            get => _physicalActivityLevel;
            set { _physicalActivityLevel = value; OnPropertyChanged(); }
        }

        public string Goal
        {
            get => _goal;
            set { _goal = value; OnPropertyChanged(); }
        }

        public double BMI
        {
            get => _bmi;
            set { _bmi = value; OnPropertyChanged(); }
        }

        public double CaloricNeeds
        {
            get => _caloricNeeds;
            set { _caloricNeeds = value; OnPropertyChanged(); }
        }

        public ICommand SettingsCommand { get; set; }

        private readonly UserController _userController;

        /*        // pusty konstruktor zeby dzialal xml?
                public SettingsVM() : this(new UserController()) { }

                public SettingsVM(UserController userController)
                {
                    _userController = userController;

                    // aktualne dane usera
                    var user = _userController.GetUser();
                    Weight = user.Weight;
                    Height = user.Height;
                    Age = user.Age;
                    Gender = user.Gender;
                    PhysicalActivityLevel = user.PhysicalActivityLevel;
                    Goal = user.Goal;

                    SaveCommand = new RelayCommand(SaveUserDetails);
                }*/
        public UserDataVM()
        {
            _userController = new UserController("C:\\Users\\Administrator\\Source\\Repos\\Nutribuddy\\Data\\UserData.json");

            // aktualne dane usera
            var user = _userController.GetUser();
            Weight = user.Weight;
            Height = user.Height;
            Age = user.Age;
            Gender = user.Gender;
            PhysicalActivityLevel = user.PhysicalActivityLevel;
            Goal = user.Goal;
            BMI = Math.Truncate(_userController.CalculateBMI() * 100) / 100;
            CaloricNeeds = Math.Truncate(_userController.CalculateCaloricNeeds() * 100) / 100;

            SettingsCommand = new RelayCommand(Settings);
        }

        private void Settings(object obj)
        {
            var navigationVM = App.Current.MainWindow.DataContext as NavigationVM;
            navigationVM?.SettingsCommand.Execute(null);
        }
    }
}
