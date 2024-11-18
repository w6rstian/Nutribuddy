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
    internal class TestDishView
    {
        private readonly FoodController _foodController;
        private readonly DishController _dishController;

        public TestDishView(FoodController foodController, DishController dishController)
        {
            _foodController = foodController;
            _dishController = dishController;
        }

        public void Run()
        {
            while (true)
            {
                // Top-level menu
                var mainMenuOptions = new List<string>
                {
                    "Add a Dish",
                    "Show Dishes",
                    "Exit"
                };

                var mainChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold green]Main Menu: Choose an option:[/]")
                        .AddChoices(mainMenuOptions)
                );

                switch (mainChoice)
                {
                    case "Add a Dish":
                        AddDishMenu();
                        break;

                    case "Show Dishes":
                        ShowDishes();
                        break;

                    case "Exit":
                        AnsiConsole.MarkupLine("[bold yellow]Exiting program. Goodbye![/]");
                        return;
                }
            }
        }

        private void AddDishMenu()
        {
            AnsiConsole.Markup("[bold green]=== Add a Dish ===[/]\n");

            var dishName = AnsiConsole.Ask<string>("Enter the name of the dish:");
            var newDish = new Dish { Name = dishName };

            while (true)
            {
                // Sub-menu for adding ingredients and finalizing dish
                var addDishOptions = new List<string>
                {
                    "Add an ingredient",
                    "Finish and save dish",
                    "Exit without saving"
                };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Choose an option:[/]")
                        .AddChoices(addDishOptions)
                );

                switch (choice)
                {
                    case "Add an ingredient":
                        AddIngredientToDish(newDish);
                        break;

                    case "Finish and save dish":
                        FinalizeDish(newDish);
                        return;

                    case "Exit without saving":
                        AnsiConsole.MarkupLine("[bold red]Dish creation canceled. Exiting...[/]");
                        return;
                }
            }
        }

        private void AddIngredientToDish(Dish newDish)
        {
            var foods = _foodController.GetAllFoods();

            if (!foods.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No food items available to add as ingredients.[/]");
                return;
            }

            var foodDescriptions = foods.Select(f => f.Description).ToList();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select an ingredient to add:[/]")
                    .AddChoices(foodDescriptions));

            var selectedFood = foods.First(f => f.Description == choice);

            var quantity = AnsiConsole.Ask<double>(
                $"Enter the quantity of [yellow]{selectedFood.Description}[/] in grams:");

            var foodWithQuantity = new FoodItem
            {
                Description = selectedFood.Description,
                Nutrients = new Dictionary<string, double>(selectedFood.Nutrients),
                QuantityInGrams = quantity
            };

            newDish.Ingredients.Add(foodWithQuantity);

            AnsiConsole.Markup($"[bold yellow]Added ingredient:[/] {selectedFood.Description} with [blue]{quantity}g[/]\n");
        }

        private void FinalizeDish(Dish newDish)
        {
            if (!newDish.Ingredients.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No ingredients added. Cannot save an empty dish.[/]");
                return;
            }

            newDish.CalculateTotalNutrients();
            _dishController.AddDish(newDish);

            AnsiConsole.Markup($"\n[bold green]Dish '{newDish.Name}' has been added![/]\n");
            AnsiConsole.Markup("[blue]Total Nutritional Values:[/]\n");

            foreach (var nutrient in newDish.TotalNutrients)
            {
                AnsiConsole.Markup($"- {nutrient.Key}: {nutrient.Value}\n");
            }
        }

        private void ShowDishes()
        {
            var dishes = _dishController.GetAllDishes();

            if (!dishes.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No dishes available.[/]");
                return;
            }

            foreach (var dish in dishes)
            {
                AnsiConsole.MarkupLine($"\n[bold green]Dish:[/] {dish.Name}");
                AnsiConsole.MarkupLine("[blue]Ingredients:[/]");
                foreach (var ingredient in dish.Ingredients)
                {
                    AnsiConsole.MarkupLine($"- {ingredient.Description}: {ingredient.QuantityInGrams}g");
                }
                AnsiConsole.MarkupLine("[blue]Total Nutritional Values:[/]");
                foreach (var nutrient in dish.TotalNutrients)
                {
                    AnsiConsole.MarkupLine($"- {nutrient.Key}: {nutrient.Value}");
                }
            }
        }
    }
}
