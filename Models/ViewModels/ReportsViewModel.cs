using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReportsViewModel: OrderReport
    {
        public List<Meal> Top3MostOrderedMeals { get; set; }
        public List<Meal> Top3LeastOrderedMeals { get; set; }
        
    }

    public class OrderReport
    {
        public int TotalOrdersInDateRange { get; set; }
        public double TotalIncomeInDateRange { get; set; }
    }
}
