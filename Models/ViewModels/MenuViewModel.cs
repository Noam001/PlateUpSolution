using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MenuViewModel
    {
        public List<Meal> Meals {  get; set; }
        public List<FoodType> FoodTypes { get; set; }
        public int FoodTypeId { get; set; }
        public int Pages {  get; set; } //כמות העמודים
        public int PageNumber {  get; set; }
        public string MealNameSearch { get; set; }

    }
}
