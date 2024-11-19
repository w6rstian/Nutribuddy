using Nutribuddy.Core.Controllers;
using Nutribuddy.UI;
using Nutribuddy.UI.Console;

namespace Nutribuddy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userController = new UserController();
            var foodController = new FoodController("Data/FoodData.json");
            var dishController = new DishController("Data/DishData.json");
            var eatHistoryController = new EatHistoryController("Data/EatHistoryData.json");

            var viewManager = new ViewManager();
            viewManager.RegisterView("IntroSequence", new IntroSequenceView(() => viewManager.ShowView("MainMenu")));
            viewManager.RegisterView("MainMenu", new MainMenuView(
                () => viewManager.ShowView("UserDetails"),
                () => viewManager.ShowView("Food"),
                () => viewManager.ShowView("Dish"),
                () => viewManager.ShowView("Calendar")));
            viewManager.RegisterView("UserDetails", new UserDetailsView(
                eatHistoryController,
                userController,
                dishController,
                () => viewManager.ShowView("MainMenu"),
                () => viewManager.ShowView("UserConfig")));
            viewManager.RegisterView("UserConfig", new UserConfigView(userController, () => viewManager.ShowView("UserDetails")));
            viewManager.RegisterView("Food", new FoodView(eatHistoryController, foodController, () => viewManager.ShowView("MainMenu")));
            viewManager.RegisterView("Dish", new DishView(eatHistoryController, foodController, dishController, () => viewManager.ShowView("MainMenu")));
            viewManager.RegisterView("Calendar", new CalendarView(eatHistoryController, () => viewManager.ShowView("MainMenu")));

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
