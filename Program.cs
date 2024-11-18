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
			//viewManager.ShowView("MainMenu");

			var foodController = new FoodController("Data/FoodData.json");
			var foodUI = new TestFoodView(foodController);
            //foodUI.Run();

            var dishController = new DishController("Data/DishData.json");  
            var dishUI = new TestDishView(foodController, dishController);
            dishUI.Run();

            var userController = new UserController();
            var userUI = new TestUserView(userController);
            //userUI.Run();
        }
    }
}
