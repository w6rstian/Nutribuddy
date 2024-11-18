using Nutribuddy.Core.Controllers;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.UI.Console
{
	internal class UserConfigView : IView
	{
		private readonly UserController _userController;
		private readonly Action _navigateToMainMenu;
		
		public UserConfigView(Action navigateToMainMenu)
		{
			_navigateToMainMenu = navigateToMainMenu;
		}

		public void Show()
		{
			AnsiConsole.Clear();

			var table = new Table();
			table.Caption("User Data", style: null);
			table.AddColumn("").Centered();
			table.AddColumn("").Centered();

		}
	}
}
