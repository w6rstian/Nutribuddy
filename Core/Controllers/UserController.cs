using Nutribuddy.Core.Models;

namespace Nutribuddy.Core.Controllers
{
    internal class UserController
    {
        private User _user;

        public UserController()
        {
            _user = new User();
        }

        public void UpdateUser(double weight, double height, int age, string gender)
        {
            if (weight > 0) _user.Weight = weight;
            if (height > 0) _user.Height = height;
            if (age > 0) _user.Age = age;
            if (gender == "Male" || gender == "Female") _user.Gender = gender;
        }

        public double CalculateBMI()
        {
            //wzor BMI: waga (kg) / (wzrost (m) ^ 2)
            double heightInMeters = _user.Height / 100.0;
            return _user.Weight / (heightInMeters * heightInMeters);
        }

        public double CalculateCaloricNeeds()
        {
            //wzor Mifflina-St Jeora
            if (_user.Gender == "Male")
            {
                return 10 * _user.Weight + 6.25 * _user.Height - 5 * this._user.Age + 5; //chop
            }
            else
            {
                return 10 * _user.Weight + 6.25 * _user.Height - 5 * _user.Age - 161; //baba
            }
        }
    }
}
