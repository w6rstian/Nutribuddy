using Nutribuddy.UI.Console;
using Nutribuddy.Core.Controllers;

namespace Nutribuddy
{
    internal class Program
    {
        static void Main(string[] args)
        {
			var taskService = new TaskController();
            var consoleUI = new TaskConsoleUI(taskService);

            consoleUI.Run();
        }
    }
}
