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
			string[] menuOptions = [
				"View my profile",
				"Browse food",
				"Browse dishes",
				"Opcja4"
			];
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
						Align.Center(new Panel(menuOptions[0]).Padding(menuPad)),
						Align.Center(new Panel(menuOptions[1]).Padding(menuPad))
					])
					.AddRow(
					[
						Align.Center(new Panel(menuOptions[2]).Padding(menuPad)),
						Align.Center(new Panel(menuOptions[3]).Padding(menuPad))
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
				.Title("[#A2D2FF]What do you want to do?[/]")
				.AddChoices(
				[
					menuOptions[0], menuOptions[1], menuOptions[2], menuOptions[3]
				])
				.HighlightStyle(new Style(foreground: Color.MediumPurple)));

			/* REMINDER:
			 * 
			 * string[] menuOptions = [
			 * "View my profile",
			 * "Browse food",
			 * "Browse dishes",
			 * "Opcja4"
			 * ];
			 */

			switch (selected)
			{
				case "View my profile":
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
