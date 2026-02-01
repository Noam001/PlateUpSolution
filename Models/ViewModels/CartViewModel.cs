using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public int TotalItems { get; set; }
        public double TotalPrice { get; set; }
    }
}
