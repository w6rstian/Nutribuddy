namespace Nutribuddy.Core.Controllers
{
	internal class TaskController
	{
		private readonly List<Models.Task> _tasks = new();

		public void AddTask(string title)
		{
			_tasks.Add(new Models.Task(title));
		}

		public IEnumerable<Models.Task> GetAllTasks() => _tasks;
	}
}
