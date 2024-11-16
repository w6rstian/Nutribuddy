using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.Core.Models
{
    internal class Dish
    {
        public string Name { get; set; }
        public List<FoodItem> Ingredients { get; set; }
        public Dictionary<string, double> TotalNutrients { get; set; }

        public Dish()
        {
            Ingredients = new List<FoodItem>();
            TotalNutrients = new Dictionary<string, double>();
        }

        public void CalculateTotalNutrients()
        {
            TotalNutrients.Clear(); //wyczyszczenie buforu

            foreach (var food in Ingredients)
            {
                foreach (var nutrient in food.Nutrients)
                {
                    double quantity = (nutrient.Value * food.QuantityInGrams) / 100; //przeliczenie ilości składnika potrzebnego do dania

                    if (TotalNutrients.ContainsKey(nutrient.Key))
                        TotalNutrients[nutrient.Key] += quantity;
                    else
                        TotalNutrients[nutrient.Key] = quantity;
                }
            }
        }

    }
}
