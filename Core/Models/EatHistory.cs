namespace Nutribuddy.Core.Models
{
    internal class EatHistory
    {
        public List<(DateTime, Dish)> DishEatHistory { get; set; }
        public List<(DateTime, FoodItem)> FoodItemEatHistory { get; set; }

        public EatHistory()
        {
            DishEatHistory = new();
            FoodItemEatHistory = new();
        }

        public EatHistory(List<(DateTime, Dish)> dishEatHistory, List<(DateTime, FoodItem)> foodItemEatHistory)
        {
            DishEatHistory = dishEatHistory;
            FoodItemEatHistory = foodItemEatHistory;
        }
    }
}
