using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.UI.Console
{
	internal class MainMenuView : IView
	{
		public Action _navigateToUserDetails;
		public Action _navigateToFoodView;
		public Action _navigateToDishView;
		private readonly Padding menuPad = new Padding(5, 1);
		public MainMenuView(Action navigateToUserDetails, Action navigateToFoodView, Action navigateToDishView)
		{
			_navigateToUserDetails = navigateToUserDetails;
			_navigateToFoodView = navigateToFoodView;
			_navigateToDishView = navigateToDishView;
		}
		public void Show()
		{
			AnsiConsole.Clear();

			AnsiConsole.Write(new Panel(
					Align.Center(
						new FigletText("Nutribuddy").Color(Color.MediumPurple),
						VerticalAlignment.Middle))
				.Expand().Padding(new Padding(0, 2)));

			AnsiConsole.Write(new Grid()
					.AddColumn()
					.AddColumn()
					.AddRow(
					[
						Align.Center(new Panel("User Details").Padding(menuPad)),
						Align.Center(new Panel("Opcja2").Padding(menuPad))
					])
					.AddRow(
					[
						Align.Center(new Panel("Opcja3").Padding(menuPad)),
						Align.Center(new Panel("Opcja4").Padding(menuPad))
					]).Expand());

			AnsiConsole.Write(
				Align.Center(
					new Grid()
						.AddColumn()
						.Width(90)
						.AddRow(new Rule())
						));

			var selected = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
				.AddChoices(
				[
					"User Details", "Browse food", "Browse dishes", "Opcja4"
				])
				.HighlightStyle(new Style(foreground: Color.MediumPurple)));

			switch (selected)
			{
				case "User Details":
					_navigateToUserDetails();
					break;

				case "Browse food":
					_navigateToFoodView();
					break;

				case "Browse dishes":
					_navigateToDishView();
					break;
			}
		}
	}
}
