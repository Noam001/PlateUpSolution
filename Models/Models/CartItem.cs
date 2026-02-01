using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AddMealRequest
    {
        public int MealId { get; set; }
        public int Quantity { get; set; }
        public string? MealNotes { get; set; }
        public string ClientId { get; set; }
    }
    public class CartItem : AddMealRequest
    {
        public int OrderID { get; set; }
        public string MealName { get; set; }
        public double MealPrice { get; set; }
    }
}
