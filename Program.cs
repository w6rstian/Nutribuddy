using Nutribuddy.UI.Console;
using Nutribuddy.UI;
using Nutribuddy.Core.Controllers;

namespace Nutribuddy
{
	internal class Program
    {
        static void Main(string[] args)
        {
			var viewManager = new ViewManager();
			viewManager.RegisterView("IntroSequence", new IntroSequenceView(() => viewManager.ShowView("MainMenu")));
			viewManager.RegisterView("MainMenu", new MainMenuView());

			//viewManager.ShowView("IntroSequence");
			viewManager.ShowView("MainMenu");
		}
    }
}
