using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using Spectre.Console;

namespace Nutribuddy.UI.Console
{
    internal class DishView : IView
    {
        private readonly EatHistoryController _eatHistoryController;
        private readonly FoodController _foodController;
        private readonly DishController _dishController;
        private readonly Action _navigateToMainMenu;
        private readonly static Panel foodFigletText = new Panel(
                Align.Center(
                    new FigletText("Dishes").Color(Color.MediumPurple),
                    VerticalAlignment.Middle))
            .Expand().Padding(new Padding(0, 2));

        public DishView(EatHistoryController eatHistoryController, FoodController foodController, DishController dishController, Action navigateToMainMenu)
        {
            _eatHistoryController = eatHistoryController;
            _foodController = foodController;
            _dishController = dishController;
            _navigateToMainMenu = navigateToMainMenu;
        }

        public void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(foodFigletText);
            while (true)
            {
                // Top-level menu
                var mainMenuOptions = new List<string>
                {
                    "Add a Dish",
                    "Show Dishes",
                    "Search for Dishes",
                    "Edit a Dish",
                    "Delete a Dish",
                    "Return to main menu"
                };

                var mainChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold gold1]What do you want to do?[/]")
                        .AddChoices(mainMenuOptions)
                                    .HighlightStyle(new Style(foreground: Color.MediumPurple))
                );

                switch (mainChoice)
                {
                    case "Add a Dish":
                        AddDishMenu();
                        break;

                    case "Show Dishes":
                        AnsiConsole.Clear();
                        AnsiConsole.Write(foodFigletText);
                        ShowDishes("");
                        break;

                    case "Search for Dishes":
                        var lookingFor = AnsiConsole.Ask<string>(
              $"What do you want to look for? ");
                        AnsiConsole.Clear();
                        AnsiConsole.Write(foodFigletText);
                        ShowDishes(lookingFor);
                        break;

                    case "Edit a Dish":
                        EditDishMenu();
                        break;

                    case "Delete a Dish":
                        DeleteDishMenu();
                        break;

                    case "Return to main menu":
                        _navigateToMainMenu();
                        return;
                }
            }
        }

        private void AddDishMenu()
        {
            AnsiConsole.Markup("[bold gold1]=== Add a Dish ===[/]\n");

            var dishName = AnsiConsole.Ask<string>("Enter the name of the dish:");
            var newDish = new Dish { Name = dishName };

            while (true)
            {
                // Sub-menu for adding ingredients and finalizing dish
                var addDishOptions = new List<string>
                {
                    "Add an ingredient",
                    "Search for an ingredient",
                    "Finish and save dish",
                    "Exit without saving"
                };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[gold1]Is this all?[/]")
                        .AddChoices(addDishOptions)
                                    .HighlightStyle(new Style(foreground: Color.MediumPurple))
                        );

                switch (choice)
                {
                    case "Add an ingredient":
                        AddIngredientToDish(newDish, "");
                        break;

                    case "Search for an ingredient":
                        AddIngredientToDish(newDish, SearchForAnIngredient());
                        break;

                    case "Finish and save dish":
                        FinalizeDish(newDish);
                        return;

                    case "Exit without saving":
                        AnsiConsole.MarkupLine("[bold red]Dish creation canceled. Exiting...[/]");
                        Thread.Sleep(500);
                        AnsiConsole.Clear();
                        AnsiConsole.Write(foodFigletText);
                        return;
                }
            }
        }

        private void AddIngredientToDish(Dish newDish, string searchPhrase)
        {
            var foods = _foodController.GetAllFoods();

            if (!foods.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No food items available to add as ingredients.[/]");
                return;
            }

            List<string> foodDescriptions;
            if (searchPhrase == null)
            {
                foodDescriptions = foods.Select(f => f.Description).ToList();
            }
            else
            {
                foodDescriptions = foods
                    .Where(f => f.Description.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                    .Select(f => f.Description)
                    .ToList();
            }


            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[pink1]Select an ingredient to add:[/]")
                    .AddChoices(foodDescriptions)
                              .HighlightStyle(new Style(foreground: Color.MediumPurple))
            );


            var selectedFood = foods.First(f => f.Description == choice);

            var quantity = AnsiConsole.Ask<double>(
                $"Enter the quantity of [pink1]{selectedFood.Description}[/] in grams:");

            var foodWithQuantity = new FoodItem
            {
                Description = selectedFood.Description,
                Nutrients = new Dictionary<string, double>(selectedFood.Nutrients),
                QuantityInGrams = quantity
            };

            newDish.Ingredients.Add(foodWithQuantity);

            AnsiConsole.Markup($"[bold pink1]Added {quantity}g of {selectedFood.Description}.[/]\n");
        }

        private string SearchForAnIngredient()
        {
            var foods = _foodController.GetAllFoods();

            if (!foods.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No food items available to add as ingredients.[/]");
                return "";
            }

            var lookingFor = AnsiConsole.Ask<string>(
                        $"What do you want to look for? ");

            return lookingFor;
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
            var confirmation = AnsiConsole.Prompt(
                    new ConfirmationPrompt("Do you want to add this dish as your meal?"));

            if (confirmation)
            {
                _eatHistoryController._eatHistory.DishEatHistory.Add((DateTime.Now, newDish));
            }

            // OLD NOTIFICATION
            //AnsiConsole.Markup($"\n[bold gold1]Dish '{newDish.Name}' has been added![/]\n");
            //AnsiConsole.Markup("[pink1]Total Nutritional Values:[/]\n");
            /*foreach (var nutrient in newDish.TotalNutrients)
      {
          AnsiConsole.Markup($"- {nutrient.Key}: {nutrient.Value}\n");
      }*/

            var dishAddedPanel = Align.Center(new Panel($"[#FFC8DD] Dish '{newDish.Name}' has been added![/]").BorderColor(new Spectre.Console.Color(255, 200, 221)).Padding(5, 1));
            var tableNutrients = new Table().BorderColor(new Color(162, 210, 255));
            tableNutrients.HideHeaders().Centered();
            tableNutrients.AddColumn("").AddColumn("");
            foreach (var nutrient in newDish.TotalNutrients)
            {
                tableNutrients.AddRow($"[#BDE0FE]{nutrient.Key}[/]", $"[#BDE0FE]{nutrient.Value}[/]");
            }
            tableNutrients.Caption("Total nutritional values");
            AnsiConsole.Write(dishAddedPanel);
            AnsiConsole.Write(tableNutrients);

            AnsiConsole.Prompt(
          new TextPrompt<string>("Press Enter to continue")
              .AllowEmpty());

            AnsiConsole.Clear();
            AnsiConsole.Write(foodFigletText);
        }

        private void ShowDishes(string searchPhrase)
        {
            var allDishes = _dishController.GetAllDishes();

            if (!allDishes.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No dishes available.[/]");
                return;
            }

            List<Dish> dishes;
            if (searchPhrase == null)
            {
                dishes = allDishes;
            }
            else
            {
                dishes = allDishes
                    .Where(f => f.Name.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            foreach (var dish in dishes)
            {
                PrintDish(dish);
                AnsiConsole.Write("\n\n\n");
            }
            var dishNames = dishes.Select(f => f.Name)
                .ToList();

            var selectedDish = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[#A2D2FF]Select a dish:[/]")
                .AddChoices(dishNames)
                .HighlightStyle(new Style(foreground: Color.MediumPurple)));

            var theDish = dishes.Find(f => f.Name == selectedDish);
            if (theDish != null)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(foodFigletText);
                PrintDish(theDish);

                var confirmation = AnsiConsole.Prompt(
                  new ConfirmationPrompt("Do you want to add this dish as your meal?"));

                if (confirmation)
                {
                    // TODO: ADDING DISH ANIMATION
                    _eatHistoryController._eatHistory.DishEatHistory.Add((DateTime.Now, theDish));
                    AnsiConsole.MarkupLine($"[bold #A2D2FF]{theDish.Name} has been added as a meal![/]");
                    Thread.Sleep(1000);
                }

                AnsiConsole.Clear();
                AnsiConsole.Write(foodFigletText);
            }
        }

        private void PrintDish(Dish dish)
        {
            var dishNamePanel = Align.Center(new Panel($"[#FFC8DD]{dish.Name}[/]").BorderColor(new Spectre.Console.Color(255, 200, 221)).Padding(5, 1));
            var tableIngredients = new Table().BorderColor(new Color(162, 210, 255));
            tableIngredients.HideHeaders().Centered();
            tableIngredients.AddColumn("").AddColumn("");
            foreach (var ingredient in dish.Ingredients)
            {
                tableIngredients.AddRow($"[#BDE0FE]{ingredient.Description}[/]", $"[#BDE0FE]{ingredient.QuantityInGrams}g[/]");
            }
            tableIngredients.Caption("Ingredients");

            var tableNutrients = new Table().BorderColor(new Color(162, 210, 255));
            tableNutrients.HideHeaders().Centered();
            tableNutrients.AddColumn("").AddColumn("");
            foreach (var nutrient in dish.TotalNutrients)
            {
                tableNutrients.AddRow($"[#BDE0FE]{nutrient.Key}[/]", $"[#BDE0FE]{nutrient.Value}[/]");
            }
            tableNutrients.Caption("Total nutritional values");
            AnsiConsole.Write(dishNamePanel);
            AnsiConsole.Write(tableIngredients);
            AnsiConsole.Write(tableNutrients);

            // OLD STUFF
            /*AnsiConsole.MarkupLine($"\n[bold gold1]Dish:[/] {dish.Name}");
            AnsiConsole.MarkupLine("[pink1]Ingredients:[/]");
            foreach (var ingredient in dish.Ingredients)
            {
                AnsiConsole.MarkupLine($"- {ingredient.Description}: {ingredient.QuantityInGrams}g");
            }
            AnsiConsole.MarkupLine("[pink1]Total Nutritional Values:[/]");
            foreach (var nutrient in dish.TotalNutrients)
            {
                AnsiConsole.MarkupLine($"- {nutrient.Key}: {nutrient.Value}");
            }*/
        }

        private void EditDishMenu()
        {
            var dishes = _dishController.GetAllDishes();

            if (!dishes.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No dishes available.[/]");
                return;
            }

            var dishName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[pink1]Select a dish to edit:[/]")
                .AddChoices(dishes.Select(d => d.Name))
                .HighlightStyle(new Style(foreground: Color.MediumPurple))
            );

            _dishController.EditDish(dishName, dish =>
            {
                AnsiConsole.MarkupLine($"Editing dish: [bold gold1]{dish.Name}[/]");
                var editOptions = new List<string> { "Change Name", "Edit Ingredients", "Back" };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[gold1]What would you like to edit?[/]")
                        .AddChoices(editOptions)
                        .HighlightStyle(new Style(foreground: Color.MediumPurple))
                );

                switch (choice)
                {
                    case "Change Name":
                        var newName = AnsiConsole.Ask<string>("Enter new name:");
                        dish.Name = newName;
                        break;

                    case "Edit Ingredients":
                        EditIngredientsMenu(dish);
                        break;
                }
            });
        }

        private void EditIngredientsMenu(Dish dish)
        {
            while (true)
            {
                var ingredientMenu = new List<string>
                {
                    "Add an Ingredient",
                    "Edit an Ingredient",
                    "Remove an Ingredient",
                    "Back"
                };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[pink1]What would you like to do with the ingredients?[/]")
                        .AddChoices(ingredientMenu)
                        .HighlightStyle(new Style(foreground: Color.MediumPurple))
                );

                switch (choice)
                {
                    case "Add an Ingredient":
                        AddIngredientToDish(dish, "");
                        break;

                    case "Edit an Ingredient":
                        var ingredientToEdit = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[pink1]Select an ingredient to edit:[/]")
                                .AddChoices(dish.Ingredients.Select(i => i.Description))
                                .HighlightStyle(new Style(foreground: Color.MediumPurple))
                        );

                        var newQuantity = AnsiConsole.Ask<double>("Enter new quantity in grams:");
                        var ingredient = dish.Ingredients.First(i => i.Description == ingredientToEdit);
                        ingredient.QuantityInGrams = newQuantity;
                        break;

                    case "Remove an Ingredient":
                        var ingredientToRemove = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[pink1]Select an ingredient to remove:[/]")
                                .AddChoices(dish.Ingredients.Select(i => i.Description))
                                .HighlightStyle(new Style(foreground: Color.MediumPurple))
                        );

                        dish.Ingredients.RemoveAll(i => i.Description == ingredientToRemove);
                        break;

                    case "Back":
                        return;
                }
            }
        }

        private void DeleteDishMenu()
        {
            var dishes = _dishController.GetAllDishes();

            if (!dishes.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No dishes available.[/]");
                return;
            }

            var dishName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[pink1]Select a dish to delete:[/]")
                    .AddChoices(dishes.Select(d => d.Name))
                    .HighlightStyle(new Style(foreground: Color.MediumPurple))
            );

            _dishController.DeleteDish(dishName);
            AnsiConsole.MarkupLine($"[bold red]Dish '{dishName}' has been deleted.[/]");
        }
    }
}
