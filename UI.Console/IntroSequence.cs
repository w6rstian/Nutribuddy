using Nutribuddy.Core.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.UI.Console
{
	internal class IntroSequence : IView
	{
		public void Run()
		{
			AnsiConsole.Clear();

			AnsiConsole.Write(
				new FigletText("Nutribuddy")
				.Centered()
				.Color(Color.MediumPurple));

			AnsiConsole.Progress()
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
						task.Increment(1);
					}

					if(ctx.IsFinished)
					{
						Thread.Sleep(1000);
					}
				});
		}
	}
}
