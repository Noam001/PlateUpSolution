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
        public string? FoodTypeId { get; set; }
        public bool? PriceSort {  get; set; }
        public int Pages {  get; set; } //כמות העמודים
        public int? PageNumber {  get; set; }
        public string? MealNameSearch { get; set; }
        public int TotalMeals { get; set; }

    }
}
