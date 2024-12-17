namespace Nutribuddy.Core.Models
{
    public class FoodItem
    {
        public string Description { get; set; } = String.Empty;
        public Dictionary<string, double> Nutrients { get; set; } = new Dictionary<string, double>();
        public double QuantityInGrams { get; set; } // ilość danego skłdanika w gramach
    }
}
