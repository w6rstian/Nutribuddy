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
		public void Show()
		{
			AnsiConsole.Clear();

			var layout = new Layout("Root")
				.SplitRows(
					new Layout("Logo"),
					new Layout("Menu"));

			layout["Logo"].Update(
				new Panel(
					Align.Center(
						new FigletText("Nutribuddy").Color(Color.MediumPurple),
						VerticalAlignment.Middle))
				.Expand());

			layout["Menu"].Update(
				new Grid()
					.AddColumn()
					.AddColumn()
					.AddRow(
					[
						Align.Center(new Panel("Opcja1")),
						Align.Center(new Panel("Opcja2"))
					])
					.AddRow(
					[
						Align.Center(new Panel("Opcja3")),
						Align.Center(new Panel("Opcja4"))
					]
					).Expand());
			
			AnsiConsole.Write(layout);

			var selected = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
				.MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
				.AddChoices(
				[
					"Opcja1", "Opcja2", "Opcja3", "Opcja4"
				]));
		}
	}
}
