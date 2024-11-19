using Nutribuddy.Core.Controllers;
using Spectre.Console;

namespace Nutribuddy.UI.Console
{
    internal class TestUserView
    {
        private readonly UserController _userController;

        public TestUserView(UserController userController)
        {
            _userController = userController;
        }

        public void Run()
        {
            while (true)
            {
                var options = new List<string>
                {
                    "Edit User Info",
                    "Show User Info",
                    "Exit"
                };

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Choose an option:[/]")
                        .AddChoices(options)
                );

                switch (choice)
                {
                    case "Edit User Info":
                        EditUserMenu();
                        break;


                    case "Show User Info":
                        var user = _userController.GetUser();
                        AnsiConsole.MarkupLine($"[bold green]User Information:[/]");
                        AnsiConsole.MarkupLine($"- Weight: [blue]{user.Weight} kg[/]");
                        AnsiConsole.MarkupLine($"- Height: [blue]{user.Height} cm[/]");
                        AnsiConsole.MarkupLine($"- Age: [blue]{user.Age} years[/]");
                        AnsiConsole.MarkupLine($"- Gender: [blue]{user.Gender}[/]");
                        AnsiConsole.MarkupLine($"- BMI: [blue]{_userController.CalculateBMI():F2}[/]");
                        AnsiConsole.MarkupLine($"- Caloric Needs: [blue]{_userController.CalculateCaloricNeeds():F0} kcal[/]\n");

                        break;

                    case "Exit":
                        AnsiConsole.MarkupLine("[bold yellow]Goodbye![/]");
                        return;
                }
            }
        }

        private void EditUserMenu()
        {
            AnsiConsole.MarkupLine("[bold yellow]Edit User Info[/]");

            double weight = AnsiConsole.Ask<double>("Enter your weight (kg):");
            double height = AnsiConsole.Ask<double>("Enter your height (cm):");
            int age = AnsiConsole.Ask<int>("Enter your age (years):");
            string gender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select your gender:")
                    .AddChoices("Male", "Female"));

            _userController.UpdateUser(weight, height, age, gender);

            AnsiConsole.MarkupLine("[bold green]User information updated successfully![/]");
        }

    }
}
