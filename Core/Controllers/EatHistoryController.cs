using Newtonsoft.Json;
using Nutribuddy.Core.Models;
using Spectre.Console;

namespace Nutribuddy.Core.Controllers
{
    internal class EatHistoryController
    {
        public EatHistory EatHistory { get; private set; }
        public Calendar Calendar { get; private set; }
        public EatHistoryController(string filePath)
        {
            try
            {
                var jsonData = File.Exists(filePath) ? File.ReadAllText(filePath) : "[]";
                EatHistory = !string.IsNullOrEmpty(jsonData) ?
                    JsonConvert.DeserializeObject<EatHistory>(jsonData) : new EatHistory();
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
                }
            }

            var foodItemsForDay = EatHistory.FoodItemEatHistory
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
    }
}
