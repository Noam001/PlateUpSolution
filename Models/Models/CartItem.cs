using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CartItem
    {
        public int MealId { get; set; }
        public string MealName { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }
        public double MealPrice { get; set; }
        public string? MealNotes { get; set; }
        public string ClientId { get; set; }
    }
}
