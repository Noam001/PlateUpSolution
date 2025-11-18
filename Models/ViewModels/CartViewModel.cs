using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class CartViewModel
    {
        // רשימת כל הפריטים שנמצאים בעגלה
        public List<CartItemViewModel> Items { get; set; }
        // סכום כולל של כל העגלה
        public double TotalPrice { get; set; }


    }
    public class CartItemViewModel
    {
        public Meal meal { get; set; }

        // כמות – בגלל כפתור ה +
        public int Quantity { get; set; }

        // חישוב מחיר למנה * כמות
        public double ItemTotal
        {
            get
            {
                return meal.MealPrice * Quantity;
            }
        }
    }

}
