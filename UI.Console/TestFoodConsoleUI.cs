using Nutribuddy.Core.Models;
using Nutribuddy.Core.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.UI.Console
{
    internal class TestFoodConsoleUI
    {
        private readonly FoodService _foodService;

        public TestFoodConsoleUI(FoodService foodService)
        {
            _foodService = foodService;
        }

        public void Run()
        {
            AnsiConsole.MarkupLine("[bold yellow]Welcome to the NutriBuddy Calorie Tracker![/]");

            while (true)
            {
                // Ask user for a food item
                var searchQuery = AnsiConsole.Ask<string>("Enter the [green]description[/] of the food item (or type [red]exit[/] to quit):");

                if (searchQuery.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    AnsiConsole.MarkupLine("[bold yellow]Goodbye![/]");
                    break;
                }

                // Find the food item
                var foodItem = _foodService.GetFoodByDescription(searchQuery);

                if (foodItem == null)
                {
                    AnsiConsole.MarkupLine("[bold red]Food item not found.[/]");
                }
                else
                {
                    // Display nutrient details
                    DisplayNutrientTable(foodItem);
                }
            }
        }

        private static void DisplayNutrientTable(FoodItem foodItem)
        {
            var table = new Table()
                .AddColumn("[blue]Nutrient[/]")
                .AddColumn("[green]Amount[/]")
                .AddColumn("[yellow]Unit[/]")
                .Border(TableBorder.Rounded);

            foreach (var nutrient in foodItem.Nutrients)
            {
                var unit = nutrient.Key.Contains("Energy") ? "kcal" : nutrient.Key.Contains("(g)") ? "g" : nutrient.Key.Contains("(mg)") ? "mg" : "";
                table.AddRow(nutrient.Key, nutrient.Value.ToString("F2"), unit);
            }

            AnsiConsole.MarkupLine($"[bold green]Nutritional values for: {foodItem.Description}[/]");
            AnsiConsole.Render(table);
        }
    }
}
