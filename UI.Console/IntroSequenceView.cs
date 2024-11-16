using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.UI.Console
{
	internal class IntroSequenceView : IView
	{
		private readonly Action _navigateToMainMenu;
	
		public IntroSequenceView(Action navigateToMainMenu)
		{
			_navigateToMainMenu = navigateToMainMenu;
		}

		public void Show()
		{
			AnsiConsole.Clear();

			AnsiConsole.Write(
				new FigletText("Loading")
				.Centered());

			AnsiConsole.Progress()
				.AutoRefresh(true)
				.AutoClear(true)
				.HideCompleted(true)
				.Columns(
				[
					new SpinnerColumn(),
					new ProgressBarColumn()
						.FinishedStyle(Style.Parse("yellow")),
					new PercentageColumn()
						.CompletedStyle(Style.Parse("yellow")),
				])
				.Start(ctx =>
				{
					var task = ctx.AddTask("[yellow]Loading[/]");

					while(!ctx.IsFinished)
					{
						Thread.Sleep(50);
						task.Increment(1.5);
					}

					if(ctx.IsFinished)
					{
						Thread.Sleep(50);
						_navigateToMainMenu();
						return;
					}
				});
		}
	}
}
