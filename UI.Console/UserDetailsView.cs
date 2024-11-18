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
	internal class UserDetailsView : IView
	{
		private readonly UserController _userController;
		private readonly DishController _dishController;
		private readonly Action _navigateToMainMenu;
		private readonly Action _navigateToUserConfig;

		public UserDetailsView(UserController userController, DishController dishController, Action navigateToMainMenu, Action navigateToUserConfig)
		{
			_userController = userController;
			_dishController = dishController;
			_navigateToMainMenu = navigateToMainMenu;
			_navigateToUserConfig = navigateToUserConfig;
		}

		public void Show()
		{
			AnsiConsole.Clear();
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
			table.AddRow("BMI", $"{ Math.Truncate(user.BMI * 100) / 100 }");
			table.AddRow("Your caloric needs", $"{user.CaloricNeeds}");

			AnsiConsole.Write(table);

			// Personalized kcal counter
			DisplayMyKcal();
			
			var options = new List<string>
				{
					"Edit User Info",
					"Return to main menu"
				};

			var choice = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[green]Choose an option:[/]")
					.AddChoices(options)
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
			var foreverNutrients = _dishController.GetForeverNutrients();
			var godDid = foreverNutrients.TryGetValue("Energy (kcal)", out var calories);
			if (godDid)
			{
				Align kcalGuardPanel;
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
				AnsiConsole.Write(kcalGuardPanel);
			}
		}
	}
}
