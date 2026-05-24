using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HomePageViewModel
    {
        public List<Review> Reviews { get; set; } //רשימת ביקורות המסעדה
        public Review Review { get; set; } // אובייקט ביקורת לצורך קליטת נתוני הטופס החדש
    }
}
