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
			var foodController = new FoodController("Data/FoodData.json");
			var dishController = new DishController("Data/DishData.json");

			var viewManager = new ViewManager();
			viewManager.RegisterView("IntroSequence", new IntroSequenceView(() => viewManager.ShowView("MainMenu")));
			viewManager.RegisterView("MainMenu", new MainMenuView(
                () => viewManager.ShowView("UserDetails"),
                () => viewManager.ShowView("Food"),
                () => viewManager.ShowView("Dish")));
			viewManager.RegisterView("UserDetails", new UserDetailsView(userController,
                dishController,
                () => viewManager.ShowView("MainMenu"), 
                () => viewManager.ShowView("UserConfig")));
			viewManager.RegisterView("UserConfig", new UserConfigView(userController, () => viewManager.ShowView("UserDetails")));
            viewManager.RegisterView("Food", new FoodView(foodController, () => viewManager.ShowView("MainMenu")));
            viewManager.RegisterView("Dish", new DishView(foodController, dishController, () => viewManager.ShowView("MainMenu")));

            viewManager.ShowView("IntroSequence");
            //viewManager.ShowView("MainMenu");
            //viewManager.ShowView("UserDetails");
			
			//var foodUI = new FoodView(foodController);
            //foodUI.Show();

            
            //var dishUI = new DishView(foodController, dishController);
            //dishUI.Show();

            //var userController = new UserController();
            //var userUI = new TestUserView(userController);
            //userUI.Show();
        }
    }
}
