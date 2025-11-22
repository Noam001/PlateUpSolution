using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ManageOrdersViewModel
    {
        public int? OrderID { get; set; }
        public List<Order> Orders { get; set; }
        public bool? OrderStatus { get; set; }
    }
}
