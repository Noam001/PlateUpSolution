using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ManageMenuViewModel
    {
        public string? FoodTypeId { get; set; }
        public List<FoodType> FoodTypes { get; set; }
        public List<Meal> Meals { get; set; }
        public string? MealNameSearch { get; set; }
    }
}
