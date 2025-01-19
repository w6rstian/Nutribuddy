using Nutribuddy.UI;
using Spectre.Console;

namespace Nutribuddy.UI.Console
{
    internal class IntroSequenceView : IView
    {
        private readonly ViewManager _viewManager;

        public IntroSequenceView(ViewManager viewManager)
        {
            _viewManager = viewManager;
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

                    while (!ctx.IsFinished)
                    {
                        Thread.Sleep(5);
                        task.Increment(1.5);
                    }

                    if (ctx.IsFinished)
                    {
                        Thread.Sleep(5);
                    }
                });
            AnsiConsole.Clear();
            //_navigateToMainMenu();
            _viewManager.ShowView("MainMenu");
        }
    }
}
