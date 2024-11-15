using Spectre.Console;
using Nutribuddy.Core.Controllers;

namespace Nutribuddy.UI.Console
{
	internal class TaskConsoleUI
	{
		private readonly TaskController _taskService;

		public TaskConsoleUI(TaskController taskService)
		{
			_taskService = taskService;
		}

		public void Run()
		{
			while (true) // nie można tak
			{
				AnsiConsole.Clear();
				AnsiConsole.MarkupLine("[bold yellow]Task Manager[/]");

				// this is all wrong ._. but prototype and testing..
				var table = new Table()
					.AddColumn("No. ")
					.AddColumn("Title");
				var tasks = _taskService.GetAllTasks();
				int count = 1;
				foreach (var task in tasks)
				{
					table.AddRow(
						count++.ToString(),
						task.Title);
				}
				if (table.Rows.Count > 0)
				{
					AnsiConsole.Write(table);
				}

				// we need arrows navigation for menu
				AnsiConsole.MarkupLine("\n[blue]1.[/] Add Task");
				AnsiConsole.MarkupLine("[blue]2.[/] Exit");
				var choice = AnsiConsole.Ask<int>("Choose an option");

				switch (choice)
				{
					case 1:
						var title = AnsiConsole.Ask<string>("Enter task title:");
						_taskService.AddTask(title);
						break;

					case 2:
						return;

					default: break;
				}
			}
		}
	}
}
