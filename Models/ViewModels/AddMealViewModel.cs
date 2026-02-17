using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AddMealViewModel
    {
        public Meal Meal { get; set; }
        public List<FoodType> FoodTypes { get; set; }
    }
}
