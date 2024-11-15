using Nutribuddy.UI.Console;

namespace Nutribuddy
{
    internal class Program
    {
        static void Main(string[] args)
        {
			var taskService = new Core.Services.TaskService();
            var consoleUI = new TaskConsoleUI(taskService);

            consoleUI.Run();
        }
    }
}
