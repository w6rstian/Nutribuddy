using Nutribuddy.UI.Console;
using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Interfaces;

namespace Nutribuddy
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IView consoleUI;
            consoleUI = new IntroSequence();
            consoleUI.Run();

			      var taskService = new TaskController();
            consoleUI = new TaskConsoleUI(taskService);


            //consoleUI.Run();

            var foodService = new FoodService("Data/FoodData.json");
            var foodConsoleUI = new TestFoodConsoleUI(foodService);
            foodConsoleUI.Run();
        }
    }
}
