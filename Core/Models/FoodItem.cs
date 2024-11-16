using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.Core.Models
{
    internal class FoodItem
    {
        public string Description { get; set; }
        public Dictionary<string, double> Nutrients { get; set; }
        public double QuantityInGrams { get; set; } // ilość danego skłdanika w gramach
    }
}
