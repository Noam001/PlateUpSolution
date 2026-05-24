using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ClientFormViewModel
    {
        public List<City> Cities {  get; set; } //ערים לבחירה בטופס העדכון פרופיל או הרשמה
        public Client Client { get; set; } //פרטי הלקוח
    }
}
