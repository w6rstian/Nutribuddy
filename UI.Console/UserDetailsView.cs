using Nutribuddy.Core.Controllers;
using Spectre.Console;

namespace Nutribuddy.UI.Console
{
    internal class UserDetailsView : IView
    {
        private readonly EatHistoryController _eatHistoryController;
        private readonly UserController _userController;
        private readonly DishController _dishController;
        private readonly Action _navigateToMainMenu;
        private readonly Action _navigateToUserConfig;
        private readonly static Panel userFigletText = new Panel(
                    Align.Center(
                        new FigletText("User Profile").Color(Color.MediumPurple),
                        VerticalAlignment.Middle))
                .Expand().Padding(new Padding(0, 2));

        public UserDetailsView(EatHistoryController eatHistoryController, UserController userController, DishController dishController, Action navigateToMainMenu, Action navigateToUserConfig)
        {
            _eatHistoryController = eatHistoryController;
            _userController = userController;
            _dishController = dishController;
            _navigateToMainMenu = navigateToMainMenu;
            _navigateToUserConfig = navigateToUserConfig;
        }

        public void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(userFigletText);
            var user = _userController.GetUser();
            var table = new Table();
            table.Caption("User Data", style: null);
            table.AddColumn("").Centered();
            table.AddColumn("").Centered();
            table.HideHeaders();

            table.AddRow("Gender", $"{user.Gender}");
            table.AddRow("Age", $"{user.Age}");
            table.AddRow("Height (cm)", $"{user.Height}");
            table.AddRow("Weight (kg)", $"{user.Weight}");
            table.AddRow("BMI", $"{Math.Truncate(user.BMI * 100) / 100}");
            table.AddRow("Your caloric needs", $"{Math.Truncate(user.CaloricNeeds * 100) / 100}");
            table.AddRow("Activity Level", $"{user.PhysicalActivityLevel}");
            table.AddRow("Goal", $"{user.Goal}");

            AnsiConsole.Write(table);

            // Personalized kcal counter
            DisplayMyKcal();

            DisplayCharts();

            var options = new List<string>
                {
                    "Edit User Info",
                    "Return to main menu"
                };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[#A2D2FF]What do you want to do?[/]")
                    .AddChoices(options)
                    .HighlightStyle(new Style(foreground: Color.MediumPurple))
            );

            switch (choice)
            {
                case "Edit User Info":
                    _navigateToUserConfig();
                    break;

                case "Return to main menu":
                    _navigateToMainMenu();
                    break;
            }
        }

        public void DisplayMyKcal()
        {
            // Personalized kcal counter

            var user = _userController.GetUser();
            var todayNutrients = _eatHistoryController.GetTotalNutrientsFromDay(DateTime.Now);
            var godDid = todayNutrients.TryGetValue("Energy (kcal)", out var calories);
            Align kcalGuardPanel;
            if (godDid)
            {
                Markup centeredText;
                if (calories > user.CaloricNeeds * 1.2)
                {
                    centeredText = new Markup($"[bold #FFAFCC]You've consumed {calories} kcal!\nTry not to eat too much...[/]")
                    .Centered();
                    kcalGuardPanel = Align.Center(new Panel(centeredText).BorderColor(new Color(255, 175, 204)).Padding(5, 1));
                }
                else if (calories < user.CaloricNeeds * 0.8)
                {
                    centeredText = new Markup($"[bold #FFAFCC]You've consumed {calories} kcal!\nTry to eat more![/]")
                    .Centered();
                    kcalGuardPanel = Align.Center(new Panel(centeredText).BorderColor(new Color(255, 175, 204)).Padding(5, 1));
                }
                else if (calories > user.CaloricNeeds * 1.1)
                {
                    centeredText = new Markup($"[bold #FFD8BE]You've consumed {calories} kcal!\nYou've had a bit too much..[/]")
                    .Centered();
                    kcalGuardPanel = Align.Center(new Panel(centeredText).BorderColor(new Color(255, 216, 190)).Padding(5, 1));
                }
                else if (calories < user.CaloricNeeds * 0.9)
                {
                    centeredText = new Markup($"[bold #FFD8BE]You've consumed {calories} kcal!\nJust a little bit more![/]")
                    .Centered();
                    kcalGuardPanel = Align.Center(new Panel(centeredText).BorderColor(new Color(255, 216, 190)).Padding(5, 1));
                }
                else
                {
                    centeredText = new Markup($"[bold #FFD8BE]You've consumed {calories} kcal!\nPerfect![/]")
                    .Centered();
                    kcalGuardPanel = Align.Center(new Panel(centeredText).BorderColor(new Color(162, 210, 255)).Padding(5, 1));
                }

            }
            else
            {
                kcalGuardPanel = Align.Center(
                    new Panel(
                        $"[bold #FFAFCC]You haven't eaten anything today.\nYou know how that makes us feel...[/]")
                    .BorderColor(new Color(255, 175, 204)).Padding(5, 1));
            }
            AnsiConsole.Write(kcalGuardPanel);
        }

        public void DisplayCharts()
        {
            var user = _userController.GetUser();
            var chartEnergy = new BarChart().Width(100);
            chartEnergy.MaxValue = user.CaloricNeeds;

            var chartCarbs = new BarChart().Width(100);
            chartCarbs.MaxValue = user.CaloricNeeds * 0.5;

			var chartFat = new BarChart().Width(100);
            chartFat.MaxValue = user.CaloricNeeds * 0.25;

			var chartProtein = new BarChart().Width(100);
            chartProtein.MaxValue = user.Weight;

			var chartSodium = new BarChart().Width(100);
            chartSodium.MaxValue = 1750;

			var chartFiber = new BarChart().Width(100);
            if (user.Gender == "Male")
            {
                chartFiber.MaxValue = 38;
			}
            else
            {
				chartFiber.MaxValue = 25;
			}
            
			var todayNutrients = _eatHistoryController.GetTotalNutrientsFromDay(DateTime.Now);

            if (todayNutrients.Count == 0)
            {
                return;
            }

            string[] nutrients = [
                "Energy (kcal)",
                "Carbohydrate, by difference (g)",
                "Total lipid (fat) (g)",
                "Protein (g)",
                "Sodium, Na (mg)",
                "Fiber, total dietary (g)"
            ];

            if (todayNutrients.ContainsKey(nutrients[0]))
            {
                chartEnergy.AddItem(nutrients[0], todayNutrients[nutrients[0]], color: Color.Yellow);
            }
            if (todayNutrients.ContainsKey(nutrients[1]))
            {
                chartCarbs.AddItem(nutrients[1], todayNutrients[nutrients[1]], Color.SandyBrown);
            }
            if (todayNutrients.ContainsKey(nutrients[2]))
            {
                chartFat.AddItem(nutrients[2], todayNutrients[nutrients[2]], Color.NavajoWhite1);
            }
            if (todayNutrients.ContainsKey(nutrients[3]))
            {
                chartProtein.AddItem(nutrients[3], todayNutrients[nutrients[3]], Color.White);
            }
            if (todayNutrients.ContainsKey(nutrients[4]))
            {
                chartSodium.AddItem(nutrients[4], todayNutrients[nutrients[4]], Color.Silver);
            }
            if (todayNutrients.ContainsKey(nutrients[5]))
            {
                chartFiber.AddItem(nutrients[5], todayNutrients[nutrients[5]], Color.DarkOliveGreen1);
            }

            AnsiConsole.Write(Align.Center(new Panel("[#A2D2FF]Nutrients for today[/]").BorderColor(new Color(162, 210, 255))));
            AnsiConsole.Write(Align.Center(new Padder(chartEnergy)));
			AnsiConsole.Write(Align.Center(new Padder(chartCarbs)));
			AnsiConsole.Write(Align.Center(new Padder(chartFat)));
			AnsiConsole.Write(Align.Center(new Padder(chartProtein)));
			AnsiConsole.Write(Align.Center(new Padder(chartSodium)));
			AnsiConsole.Write(Align.Center(new Padder(chartFiber)));
		}
    }
}
