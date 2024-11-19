using Newtonsoft.Json;
using Nutribuddy.Core.Models;

namespace Nutribuddy.Core.Controllers
{
    internal class FoodController
    {
        private readonly List<FoodItem> _foodItems;

        public FoodController(string filePath)
        {
            try
            {
                var jsonData = File.ReadAllText(filePath);
                _foodItems = JsonConvert.DeserializeObject<List<FoodItem>>(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                _foodItems = new List<FoodItem>();
            }
        }

        public List<FoodItem> GetAllFoods()
        {
            return _foodItems;
        }
    }
}
