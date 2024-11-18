using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutribuddy.Core.Models
{
    internal class User
    {
        public double Weight { get; set; } = 70.0;
        public double Height { get; set; } = 180.0;
        public int Age { get; set; } = 20;
        public string Gender { get; set; } = "Male";
        public double BMI { get; set; }
        public double CaloricNeeds { get; set; }

        public User()
        {
            BMI = Weight / ((Height / 100) * (Height / 100));
            if (Gender == "Male")
            {
				CaloricNeeds = 10 * Weight + 6.25 * Height - 5 * Age + 5; //chop
			}
            else
            {
				CaloricNeeds = 10 * Weight + 6.25 * Height - 5 * Age - 161; //baba
			}
		}
    }
}
