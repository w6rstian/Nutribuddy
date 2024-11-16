using Nutribuddy.Core.Services;
using Nutribuddy.UI.Console;
using System;

namespace Nutribuddy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var taskService = new Core.Services.TaskService();
            var consoleUI = new TaskConsoleUI(taskService);

            //consoleUI.Run();

            var foodService = new FoodService("Data/FoodData.json");
            var foodConsoleUI = new TestFoodConsoleUI(foodService);
            foodConsoleUI.Run();
        }
    }
}
