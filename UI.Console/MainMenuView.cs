using Spectre.Console;

namespace Nutribuddy.UI.Console
{
    internal class MainMenuView : IView
    {
        public Action _navigateToUserDetails;
        public Action _navigateToFoodView;
        public Action _navigateToDishView;
        public Action _navigateToCalendarView;
        private readonly ViewManager _viewManager;
        private readonly Padding menuPad = new Padding(5, 1);
        public MainMenuView(ViewManager viewManager)
        {
            _viewManager = viewManager;
        }
        public void Show()
        {
            string[] menuOptions = [
                "View my profile",
                "Browse food",
                "Browse dishes",
                "View calendar",
                "Exit"
            ];
            AnsiConsole.Clear();

            AnsiConsole.Write(new Panel(
                    Align.Center(
                        new FigletText("NutribuddyDP").Color(Color.MediumPurple),
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

            AnsiConsole.Write(new Rule());

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[#A2D2FF]What do you want to do?[/]")
                .AddChoices(
                [
                    menuOptions[0], menuOptions[1], menuOptions[2], menuOptions[3], menuOptions[4],
                ])
                .HighlightStyle(new Style(foreground: Color.MediumPurple)));

            /* REMINDER:
			 * 
			 * string[] menuOptions = [
			 * "View my profile",
			 * "Browse food",
			 * "Browse dishes",
			 * "View calendar"
			 * ];
			 */

            switch (selected)
            {
                case "View my profile":
                    //_navigateToUserDetails();
                    _viewManager.ShowView("UserDetails");
                    break;

                case "Browse food":
                    //_navigateToFoodView();
                    _viewManager.ShowView("Food");
                    break;

                case "Browse dishes":
                    //_navigateToDishView();
                    _viewManager.ShowView("Dish");
                    break;

                case "View calendar":
                    //_navigateToCalendarView();
                    _viewManager.ShowView("Calendar");
                    break;

                case "Exit":
                    return;
            }
        }
    }
}
