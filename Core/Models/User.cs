namespace Nutribuddy.Core.Models
{
    internal class User
    {
        public double Weight { get; set; } = 70.0;
        public double Height { get; set; } = 180.0;
        public int Age { get; set; } = 20;
        public string Gender { get; set; } = "Male";
        public string PhysicalActivityLevel { get; set; } = "Sedentary";
        public string Goal { get; set; } = "Maintain Weight";
        public double BMI { get; set; }
        public double CaloricNeeds { get; set; }

        public User()
        {
            BMI = Weight / ((Height / 100) * (Height / 100));
            CaloricNeeds = 0;
        }
    }
}
