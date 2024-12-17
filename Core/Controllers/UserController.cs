using Newtonsoft.Json;
using Nutribuddy.Core.Models;
using System.IO;

namespace Nutribuddy.Core.Controllers
{
    internal class UserController
    {
        private User _user;
        private readonly string _filePath;

        public UserController(string filePath)
        {
            _filePath = filePath;
            _user = LoadUser();
        }

        public User GetUser()
        {
            return _user;
        }

        public void UpdateUser(double weight, double height, int age, string gender, string physicalActivityLevel, string goal)
        {
            if (weight > 0) _user.Weight = weight;
            if (height > 0) _user.Height = height;
            if (age > 0) _user.Age = age;
            if (gender == "Male" || gender == "Female") _user.Gender = gender;
            _user.PhysicalActivityLevel = physicalActivityLevel;
            _user.Goal = goal;
            _user.BMI = CalculateBMI();
            _user.CaloricNeeds = CalculateCaloricNeeds();

            SaveUser();
        }

        public double CalculateBMI()
        {
            double heightInMeters = _user.Height / 100.0;
            return _user.Weight / (heightInMeters * heightInMeters);
        }

        public double CalculateCaloricNeeds()
        {
            double bmr = _user.Gender == "Male"
                ? 10 * _user.Weight + 6.25 * _user.Height - 5 * _user.Age + 5
                : 10 * _user.Weight + 6.25 * _user.Height - 5 * _user.Age - 161;

            double activityFactor = _user.PhysicalActivityLevel switch
            {
                "Sedentary" => 1.2,
                "Lightly Active" => 1.375,
                "Moderately Active" => 1.55,
                "Very Active" => 1.725,
                "Extra Active" => 1.9,
                _ => 1.2
            };

            double maintenanceCalories = bmr * activityFactor;

            return _user.Goal switch
            {
                "Lose Weight" => maintenanceCalories - 500,
                "Maintain Weight" => maintenanceCalories,
                "Gain Weight" => maintenanceCalories + 500,
                _ => maintenanceCalories
            };
        }

        private User LoadUser()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var jsonData = File.ReadAllText(_filePath);
                    return JsonConvert.DeserializeObject<User>(jsonData) ?? new User();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user data: {ex.Message}");
            }
            return new User();
        }

        private void SaveUser()
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(_user, Formatting.Indented);
                File.WriteAllText(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
            }
        }
    }
}
