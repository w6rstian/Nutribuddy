using Nutribuddy.UI.Console;
using Nutribuddy.UI;
using Nutribuddy.Core.Controllers;

namespace Nutribuddy
{
	internal class Program
    {
        static void Main(string[] args)
        {
            var userController = new UserController();

			var viewManager = new ViewManager();
			viewManager.RegisterView("IntroSequence", new IntroSequenceView(() => viewManager.ShowView("MainMenu")));
			viewManager.RegisterView("MainMenu", new MainMenuView(() => viewManager.ShowView("UserDetails")));
			viewManager.RegisterView("UserDetails", new UserDetailsView(userController, () => viewManager.ShowView("MainMenu"), () => viewManager.ShowView("UserConfig")));
			viewManager.RegisterView("UserConfig", new UserConfigView(userController, () => viewManager.ShowView("UserDetails")));

            //viewManager.ShowView("IntroSequence");
            //viewManager.ShowView("MainMenu");
            viewManager.ShowView("UserDetails");
			var foodController = new FoodController("Data/FoodData.json");
			var foodUI = new TestFoodView(foodController);
            //foodUI.Run();

            var dishController = new DishController("Data/DishData.json");  
            var dishUI = new TestDishView(foodController, dishController);
            dishUI.Run();

            //var userController = new UserController();
            var userUI = new TestUserView(userController);
            //userUI.Run();
        }
    }
}
