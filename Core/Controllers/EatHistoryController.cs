using Newtonsoft.Json;
using Nutribuddy.Core.Models;
using Spectre.Console;
using System.IO;

namespace Nutribuddy.Core.Controllers
{
    internal class EatHistoryController
    {
        public EatHistory EatHistory { get; private set; }
        public Calendar Calendar { get; private set; }

        private readonly string _filePathDish;
        private readonly string _filePathFood;

        public EatHistoryController(string filePathFood, string filePathDish)
        {
            _filePathDish = filePathDish;
            _filePathFood = filePathFood;

            try
            {
                var jsonDataFood = File.Exists(filePathFood) ? File.ReadAllText(filePathFood) : "[]";
                var jsonDataDish = File.Exists(filePathDish) ? File.ReadAllText(filePathDish) : "[]";
                /*EatHistory = !string.IsNullOrEmpty(jsonData) ?
                    JsonConvert.DeserializeObject<EatHistory>(jsonData) : new EatHistory();*/
                var listFood = JsonConvert.DeserializeObject<List<(DateTime, FoodItem)>>(jsonDataFood) ?? new List<(DateTime, FoodItem)>();
                var listDish = JsonConvert.DeserializeObject<List<(DateTime, Dish)>>(jsonDataDish) ?? new List<(DateTime, Dish)>();
                //EatHistory = JsonConvert.DeserializeObject<EatHistory>(jsonData) ?? new EatHistory();
                EatHistory = new EatHistory(listDish, listFood);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dishes: {ex.Message}");
                EatHistory = new EatHistory();
            }
            Calendar = new Calendar(DateTime.Now);
            BuildCalendar(Calendar, DateTime.Now.Year, DateTime.Now.Month);
        }

        public Dictionary<string, double> GetTotalNutrientsFromDay(DateTime date)
        {
            var dishesForDay = EatHistory.DishEatHistory
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

                    totalNutrients[nutrient.Key] = Math.Truncate(totalNutrients[nutrient.Key] * 100) / 100;
                }
            }

            var foodItemsForDay = EatHistory.FoodItemEatHistory
                .FindAll(record => record.Item1.Date == date.Date);

            foreach (var food in foodItemsForDay)
            {
                foreach (var nutrient in food.Item2.Nutrients)
                {
                    double quantity = nutrient.Value * food.Item2.QuantityInGrams / 100;

                    if (totalNutrients.ContainsKey(nutrient.Key))
                        totalNutrients[nutrient.Key] += quantity;
                    else
                        totalNutrients[nutrient.Key] = quantity;

                    totalNutrients[nutrient.Key] = Math.Truncate(totalNutrients[nutrient.Key] * 100) / 100;
                }
            }
            return totalNutrients;
        }

        public void BuildCalendar(Calendar calendar, int year, int month)
        {
            var tempMonth = month;
            DateTime date = new DateTime(year, month, 1);
            var nutrientsDay = new Dictionary<int, Dictionary<string, double>>();
            calendar.HighlightStyle(Style.Parse("bold #A2D2FF"));

            calendar.Year = year;
            calendar.Month = month;
            calendar.Day = 1;
            calendar.CalendarEvents.Clear();
            while (tempMonth == month)
            {
                if (date > DateTime.Now)
                {
                    return;
                }

                nutrientsDay[date.Day] = GetTotalNutrientsFromDay(date);
                if (!nutrientsDay[date.Day].ContainsKey("Energy (kcal)"))
                {
                    date = date.AddDays(1);
                    continue;
                }

                Calendar = calendar.AddCalendarEvent(nutrientsDay[date.Day]["Energy (kcal)"].ToString(), date);
                date = date.AddDays(1);
            }
        }

        public void AddDishToHistory(DateTime dateTime, Dish dish)
        {
            EatHistory.DishEatHistory.Add((dateTime, dish));
            SaveEatHistory();
        }

        public void AddFoodItemToHistory(DateTime dateTime, FoodItem foodItem)
        {
            EatHistory.FoodItemEatHistory.Add((dateTime, foodItem));
            SaveEatHistory();
        }

        private void SaveEatHistory()
        {
            try
            {
                //var jsonData = JsonConvert.SerializeObject(EatHistory, Formatting.Indented);
                var jsonDataDish = JsonConvert.SerializeObject(EatHistory.DishEatHistory, Formatting.Indented);
                var jsonDataFood = JsonConvert.SerializeObject(EatHistory.FoodItemEatHistory, Formatting.Indented);
                File.WriteAllText(_filePathDish, jsonDataDish);
                File.WriteAllText(_filePathFood, jsonDataFood);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving eat history: {ex.Message}");
            }
        }
    }
}
