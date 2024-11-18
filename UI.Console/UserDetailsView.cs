using Nutribuddy.Core.Controllers;
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
		private readonly Action _navigateToMainMenu;
		private readonly Action _navigateToUserConfig;

		public UserDetailsView(UserController userController, Action navigateToMainMenu, Action navigateToUserConfig)
		{
			_userController = userController;
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
	}
}
