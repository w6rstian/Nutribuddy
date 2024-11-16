using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
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
        private readonly FoodController _foodController;

        public TestFoodConsoleUI(FoodController foodController)
        {
            _foodController = foodController;
        }

        public void Run()
        {
            AnsiConsole.MarkupLine("[bold yellow]Welcome to the NutriBuddy Calorie Tracker![/]");

            while (true)
            {
                // Display menu
                var options = new List<string>
                {
                    "View all food items",
                    "Exit"
                };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Choose an option:[/]")
                        .AddChoices(options)
                );

                switch (choice)
                {
                    case "View all food items":
                        DisplayFoodList();
                        break;

                    case "Exit":
                        AnsiConsole.MarkupLine("[bold yellow]Goodbye![/]");
                        return;
                }
            }
        }

        private void DisplayFoodList()
        {
            var foodItems = _foodController.GetAllFoods();

            if (foodItems.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No food items available.[/]");
                return;
            }

            var descriptions = foodItems.ConvertAll(f => f.Description);
            var selectedFood = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select a food item to view its nutritional values:[/]")
                    .AddChoices(descriptions)
            );

            var foodItem = foodItems.Find(f => f.Description == selectedFood);
            if (foodItem != null)
            {
                DisplayNutrientTable(foodItem);
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