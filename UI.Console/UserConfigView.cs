using Nutribuddy.Core.Controllers;
using Spectre.Console;

namespace Nutribuddy.UI.Console
{
    internal class UserConfigView : IView
    {
        private readonly UserController _userController;
        private readonly Action _navigateToUserDetails;

        public UserConfigView(UserController userController, Action navigateToUserDetails)
        {
            _userController = userController;
            _navigateToUserDetails = navigateToUserDetails;
        }

        public void Show()
        {
            AnsiConsole.MarkupLine("[bold yellow]Edit User Info[/]");

            double weight = AnsiConsole.Ask<double>("Enter your weight (kg):");
            double height = AnsiConsole.Ask<double>("Enter your height (cm):");
            int age = AnsiConsole.Ask<int>("Enter your age (years):");
            string gender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select your gender:")
                    .AddChoices("Male", "Female"));

            string activityLevel = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select your physical activity level:")
                    .AddChoices("Sedentary", "Lightly Active", "Moderately Active", "Very Active", "Extra Active"));

            string goal = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What is your goal?")
                    .AddChoices("Lose Weight", "Maintain Weight", "Gain Weight"));

            _userController.UpdateUser(weight, height, age, gender, activityLevel, goal);

            AnsiConsole.MarkupLine("[bold green]User information updated successfully![/]");
            Thread.Sleep(750);

            _navigateToUserDetails();
        }
    }
}
