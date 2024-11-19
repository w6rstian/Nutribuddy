using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
