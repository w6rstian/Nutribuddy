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
    internal class FoodView : IView
    {
        private readonly FoodController _foodController;
        private readonly Action _navigateToMainMenu;
        private readonly static Panel foodFigletText = new Panel(
                    Align.Center(
                        new FigletText("Food").Color(Color.MediumPurple),
                        VerticalAlignment.Middle))
                .Expand().Padding(new Padding(0, 2));

		public FoodView(FoodController foodController, Action navigateToMainMenu)
        {
            _foodController = foodController;
            _navigateToMainMenu = navigateToMainMenu;
        }

        public void Show()
        {
			AnsiConsole.Clear();
			AnsiConsole.Write(foodFigletText);
			while (true)
            {
				var options = new List<string>
                {
                    "View all food items",
                    "Return to main menu"
                };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[#A2D2FF]Choose an option:[/]")
                        .AddChoices(options)
						.HighlightStyle(new Style(foreground: Color.MediumPurple))
				);

				switch (choice)
                {
                    case "View all food items":
						AnsiConsole.Clear();
						DisplayFoodList();
                        break;

                    case "Return to main menu":
                        _navigateToMainMenu();
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

            AnsiConsole.Write(foodFigletText);

            var descriptions = foodItems.ConvertAll(f => f.Description);
            var selectedFood = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[#A2D2FF]Select a food item to view its nutritional values:[/]")
                    .AddChoices(descriptions)
					.HighlightStyle(new Style(foreground: Color.MediumPurple))
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
                .AddColumn("[#BDE0FE]Nutrient[/]")
                .AddColumn("[#BDE0FE]Amount[/]")
                .AddColumn("[#BDE0FE]Unit[/]")
                .RoundedBorder();

            foreach (var nutrient in foodItem.Nutrients)
            {
                var unit = nutrient.Key.Contains("Energy") ? "kcal" : nutrient.Key.Contains("(g)") ? "g" : nutrient.Key.Contains("(mg)") ? "mg" : "";
                table.AddRow(nutrient.Key, nutrient.Value.ToString("F2"), unit);
            }

			AnsiConsole.Clear();
			AnsiConsole.Write(foodFigletText);
			AnsiConsole.MarkupLine($"[bold #A2D2FF]Nutritional values for: {foodItem.Description}[/]");
            AnsiConsole.Write(table);
        }
    }
}