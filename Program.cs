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

            var foodController = new FoodController("Data/FoodData.json");
            var foodConsoleUI = new TestFoodConsoleUI(foodController);
            foodConsoleUI.Run();
        }
    }
}
