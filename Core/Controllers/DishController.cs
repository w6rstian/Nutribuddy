using Newtonsoft.Json;
using Nutribuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.Core.Controllers
{
    internal class DishController
    {
        private readonly List<Dish> _dishes;
        private readonly string _filePath;

        public DishController(string filePath)
        {
            _filePath = filePath;
            try
            {
                var jsonData = File.Exists(filePath) ? File.ReadAllText(filePath) : "[]";
                _dishes = JsonConvert.DeserializeObject<List<Dish>>(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dishes: {ex.Message}");
                _dishes = new List<Dish>();
            }
        }

        public List<Dish> GetAllDishes()
        {
            return _dishes;
        }

        public void AddDish(Dish dish)
        {
            dish.CalculateTotalNutrients();
            _dishes.Add(dish);
            SaveDishes();
        }

        private void SaveDishes() //nie dziala zapis do pliku
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(_dishes, Formatting.Indented);
                File.WriteAllText(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving dishes: {ex.Message}");
            }
        }
    }
}
