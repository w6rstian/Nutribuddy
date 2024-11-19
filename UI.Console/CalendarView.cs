using Nutribuddy.Core.Controllers;
using Nutribuddy.Core.Models;
using Spectre.Console;

namespace Nutribuddy.UI.Console
{
	internal class CalendarView : IView
	{
		private EatHistoryController _eatHistoryController;
		private Action _navigateToMainMenu;
		private readonly static Panel calendarFigletText = new Panel(
				Align.Center(
					new FigletText("Calendar").Color(Color.MediumPurple),
					VerticalAlignment.Middle))
			.Expand().Padding(new Padding(0, 2));
		public CalendarView(EatHistoryController eatHistoryController, Action navigateToMainMenu)
		{
			_eatHistoryController = eatHistoryController;
			_navigateToMainMenu = navigateToMainMenu;
		}

		public void Show()
		{
			AnsiConsole.Clear();
			AnsiConsole.Write(calendarFigletText);

			var calendar = _eatHistoryController.Calendar;
			calendar.Centered();
			_eatHistoryController.BuildCalendar(calendar, DateTime.Now.Year, DateTime.Now.Month);
			while (true)
			{
				AnsiConsole.Write(calendar);

				var menuOptions = new List<string>
				{
					"Show calories from day this month",
					"Change calendar page",
					"Return to main menu"
				};

				var mainChoice = AnsiConsole.Prompt(
					new SelectionPrompt<string>()
						.Title("[#A2D2FF]What do you want to do?[/]")
						.AddChoices(menuOptions)
									.HighlightStyle(new Style(foreground: Color.MediumPurple))
				);

				switch (mainChoice)
				{
					case "Show calories from day this month":
						var selectedDay = AnsiConsole.Ask<int>(
							$"Enter the [pink1]day[/]: ");

						var eventList = _eatHistoryController.Calendar.CalendarEvents;
						var found = false;
						foreach (var e in eventList)
						{
							if (e.Day == selectedDay && e.Month == calendar.Month && e.Year == calendar.Year)
							{
								found = true;
								AnsiConsole.Write(new Markup($"[pink1]{calendar.Year} - {calendar.Month} - {selectedDay}:[/]"));
								AnsiConsole.Write(new Markup($"\tYou've eaten [pink1]{e.Description} kcal[/]!\n"));
							}
						}
						if (!found)
						{
							AnsiConsole.Write(new Markup($"[pink1]{calendar.Year} - {calendar.Month} - {selectedDay}:[/]"));
							AnsiConsole.Write(new Markup($"\t[pink1]No records found for the selected day[/]\n"));
						}

						var shouldContinue = AnsiConsole.Prompt(
							new TextPrompt<string>("Press Enter to continue")
							.AllowEmpty());

						AnsiConsole.Clear();
						AnsiConsole.Write(calendarFigletText);
						break;

					case "Change calendar page":
						var selectedYear = AnsiConsole.Ask<int>(
							$"Enter the [pink1]year[/]: ");

						var selectedMonth = AnsiConsole.Ask<int>(
							$"Enter the [pink1]month[/]: ");

						_eatHistoryController.BuildCalendar(calendar, selectedYear, selectedMonth);

						AnsiConsole.Clear();
						AnsiConsole.Write(calendarFigletText);
						break;

					case "Return to main menu":
						_navigateToMainMenu();
						return;

					default:
						AnsiConsole.Clear();
						AnsiConsole.Write(calendarFigletText);
						break;
				}
			}
		}
	}
}
