using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ManageMenuViewModel
    {
        public string? FoodTypeId { get; set; } = ""; // מזהה סוג האוכל הנוכחי המשמש לסינון התפריט
        public List<FoodType> FoodTypes { get; set; } // רשימת כל סוגי האוכל 
        public List<Meal> Meals { get; set; }// רשימת המנות המוצגות למנהל
    }
}
