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
                _dishes = [];
            }
        }

        public List<Dish> GetAllDishes()
        {
            return _dishes;
        }

        public Dictionary<string, double> GetForeverNutrients()
        {
		    // ta funkcja na razie zlicza wszystkie dania.
		    // TODO: zliczanie dań tylko z dzisiejszego dnia
            Dictionary<string, double> totalNutrients = new();
            foreach (var dish in _dishes)
            {
                foreach (var nutrient in dish.TotalNutrients)
                {
                    var exists = !totalNutrients.TryAdd(nutrient.Key, nutrient.Value);

                    if (exists)
                    {
                        totalNutrients[nutrient.Key] += nutrient.Value;
                    }
                }
            }

            return totalNutrients;
		}

        public void SetIngredientQuantity(Dish dish, string foodDescription, double quantityInGrams)
        {
            var ingredient = dish.Ingredients.FirstOrDefault(f => f.Description == foodDescription);
            if (ingredient != null)
            {
                ingredient.QuantityInGrams = quantityInGrams;
                dish.CalculateTotalNutrients();
            }
            else
            {
                Console.WriteLine($"Ingredient '{foodDescription}' not found in the dish.");
            }
        }

        public void AddDish(Dish dish)
        {
            dish.CalculateTotalNutrients();
            _dishes.Add(dish);
            SaveDishes();
        }

        private void SaveDishes() //zapis do pliku działa tylko przy podaniu sciezki absolutnej :(
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
