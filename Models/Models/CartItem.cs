using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CartItem : OrderItem
    {
        public string MealName { get; set; }
        public double MealPrice { get; set; }
        public string ClientId { get; set; }
    }

    public class OrderItem
    {
        public int MealId { get; set; }
        public int Quantity { get; set; }
        public string? MealNotes { get; set; }
        public int OrderID { get; set; }

    }


}
