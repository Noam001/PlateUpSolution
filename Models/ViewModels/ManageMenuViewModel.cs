using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ManageMenuViewModel
    {
        public string FoodTypeName {  get; set; }
        public List<Meal> Meals { get; set; }
        public string MealName { get; set; }
    }
}
