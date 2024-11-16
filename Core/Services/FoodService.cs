using Newtonsoft.Json;
using Nutribuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.Core.Services
{
    internal class FoodService
    {
        private readonly List<FoodItem> _foodItems;

        public FoodService(string filePath)
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

        public FoodItem GetFoodByDescription(string description)
        {
            return _foodItems.FirstOrDefault(f => f.Description.Contains(description, StringComparison.OrdinalIgnoreCase));
        }

        public List<FoodItem> GetAllFoods()
        {
            return _foodItems;
        }
    }
}
