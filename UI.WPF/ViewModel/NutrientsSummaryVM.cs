using Nutribuddy.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.UI.WPF.ViewModel
{
    class NutrientsSummaryVM : ViewModelBase
    {
        private EatHistoryController _eatHistoryController;
        private UserController _userController;

        public Dictionary<string, double> Nutrients { get; set; }
        public double RequiredCalories { get; set; }



        public NutrientsSummaryVM()
        {
            _eatHistoryController = new EatHistoryController(
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\FoodHistory.json", // Path to FoodHistory.json
                "C:\\Users\\Administrator\\source\\repos\\Nutribuddy\\Data\\DishHistory.json" // Path to DishHistory.json
                );
            _userController = new UserController("C:\\Users\\Administrator\\Source\\Repos\\Nutribuddy\\Data\\UserData.json");

            RequiredCalories = _userController.CalculateCaloricNeeds();
            Nutrients = _eatHistoryController.GetTotalNutrientsFromDay(DateTime.Now);
            if ( Nutrients.Count == 0 )
            {
                Nutrients.Add("No data for today", 0);
            }
        }
    }
}
