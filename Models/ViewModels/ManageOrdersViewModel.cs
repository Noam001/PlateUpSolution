using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ManageOrdersViewModel
    {
        public int? OrderID { get; set; }// מזהה הזמנה אופציונלי לצורך חיפוש וסינון
        public List<Order> Orders { get; set; } // רשימת ההזמנות המוצגות במערכת הניהול
        public bool? OrderStatus { get; set; } // סטטוס ההזמנה (ממתין/הושלם) לצורך סינון או עדכון
    }
}
