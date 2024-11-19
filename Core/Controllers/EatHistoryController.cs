using Newtonsoft.Json;
using Nutribuddy.Core.Models;

namespace Nutribuddy.Core.Controllers
{
    internal class EatHistoryController
    {
        public EatHistory _eatHistory { get; private set; }
        public EatHistoryController(string filePath)
        {
            try
            {
                var jsonData = File.Exists(filePath) ? File.ReadAllText(filePath) : "[]";
                _eatHistory = !string.IsNullOrEmpty(jsonData) ?
                    JsonConvert.DeserializeObject<EatHistory>(jsonData) : new EatHistory();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dishes: {ex.Message}");
                _eatHistory = new EatHistory();
            }
        }
        public Dictionary<string, double> GetTotalNutrientsFromDay(DateTime date)
        {
            var dishesForDay = _eatHistory.DishEatHistory
                .FindAll(record => record.Item1.Date == date.Date);

            var totalNutrients = new Dictionary<string, double>();

            foreach (var record in dishesForDay)
            {
                foreach (var nutrient in record.Item2.TotalNutrients)
                {
                    if (totalNutrients.ContainsKey(nutrient.Key))
                    {
                        totalNutrients[nutrient.Key] += nutrient.Value;
                    }
                    else
                    {
                        totalNutrients[nutrient.Key] = nutrient.Value;
                    }
                }
            }

            var foodItemsForDay = _eatHistory.FoodItemEatHistory
                .FindAll(record => record.Item1.Date == date.Date);

            foreach (var food in foodItemsForDay)
            {
                foreach (var nutrient in food.Item2.Nutrients)
                {
                    double quantity = (nutrient.Value * food.Item2.QuantityInGrams) / 100; //przeliczenie ilości składnika potrzebnego do dania

                    if (totalNutrients.ContainsKey(nutrient.Key))
                        totalNutrients[nutrient.Key] += quantity;
                    else
                        totalNutrients[nutrient.Key] = quantity;
                }
            }
            return totalNutrients;
        }
    }
}
