using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class MealViewModel
    {
        public Meal Meal { get; set; }
        public List<FoodType> FoodTypes { get; set; }
    }
}
