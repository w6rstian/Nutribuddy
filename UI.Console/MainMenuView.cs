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
						Align.Center(new Panel("Opcja1").Padding(new Padding(5, 1))),
						Align.Center(new Panel("Opcja2").Padding(new Padding(5, 1)))
					])
					.AddRow(
					[
						Align.Center(new Panel("Opcja3").Padding(new Padding(5, 1))),
						Align.Center(new Panel("Opcja4").Padding(new Padding(5, 1)))
					]).Expand());
			AnsiConsole.Write(Align.Center(new Grid().AddColumn().Width(90).AddRow(new Rule())));
			var selected = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
				.AddChoices(
				[
					"Opcja1", "Opcja2", "Opcja3", "Opcja4"
				])
				.HighlightStyle(new Style(foreground: Color.MediumPurple)));
		}
	}
}
