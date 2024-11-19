using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using Spectre.Console;

namespace Nutribuddy.UI.Console
{
    internal class FoodView : IView
    {
        private readonly EatHistoryController _eatHistoryController;
        private readonly FoodController _foodController;
        private readonly Action _navigateToMainMenu;
        private readonly static Panel foodFigletText = new Panel(
                    Align.Center(
                        new FigletText("Food").Color(Color.MediumPurple),
                        VerticalAlignment.Middle))
                .Expand().Padding(new Padding(0, 2));

        public FoodView(EatHistoryController eatHistoryController, FoodController foodController, Action navigateToMainMenu)
        {
            _eatHistoryController = eatHistoryController;
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
                    "Search for a food item",
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
                        DisplayFoodList("");
                        break;

                    case "Search for a food item":
                        var lookingFor = AnsiConsole.Ask<string>(
                            $"What do you want to look for? ");
                        AnsiConsole.Clear();
                        DisplayFoodList(lookingFor);
                        break;

                    case "Return to main menu":
                        _navigateToMainMenu();
                        return;
                }
            }
        }

        private void DisplayFoodList(string searchPhrase)
        {
            var foodItems = _foodController.GetAllFoods();

            if (foodItems.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No food items available.[/]");
                return;
            }

            AnsiConsole.Write(foodFigletText);

            List<string> descriptions;
            if (searchPhrase == null)
            {
                descriptions = foodItems.ConvertAll(f => f.Description);
            }
            else
            {
                descriptions = foodItems
                    .Where(f => f.Description.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                    .Select(f => f.Description)
                    .ToList();
            }

            var selectedFood = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[#A2D2FF]Select a food item:[/]")
                    .AddChoices(descriptions)
                    .HighlightStyle(new Style(foreground: Color.MediumPurple))
            );

            var foodItem = foodItems.Find(f => f.Description == selectedFood);
            if (foodItem != null)
            {
                DisplayNutrientTable(foodItem);
                var confirmation = AnsiConsole.Prompt(
                    new ConfirmationPrompt("Do you want to add this food item as your meal?"));

                if (confirmation)
                {
                    var quantity = AnsiConsole.Ask<double>(
                        $"Enter the quantity of [pink1]{foodItem.Description}[/] in grams:");

                    var foodWithQuantity = new FoodItem
                    {
                        Description = foodItem.Description,
                        Nutrients = new Dictionary<string, double>(foodItem.Nutrients),
                        QuantityInGrams = quantity
                    };

                    // TODO: ADDING FOOD ITEM ANIMATION

                    _eatHistoryController.EatHistory.FoodItemEatHistory.Add((DateTime.Now, foodWithQuantity));
                    AnsiConsole.MarkupLine($"[bold #A2D2FF]{foodWithQuantity.Description} has been added as a meal![/]");
                    Thread.Sleep(1000);
                }
                AnsiConsole.Clear();
                AnsiConsole.Write(foodFigletText);
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