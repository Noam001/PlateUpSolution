using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class City
    {
        int cityId;
        string cityName;

        public int CityId 
        { 
            get { return cityId; } 
            set { cityId = value; } 
        }
        [Required(ErrorMessage = "You must enter city name")]
        //[RegularExpression(@"^[א-ת]", ErrorMessage ="Text must be hebrew")]
        public string CityName
        {
            get { return cityName; } 
            set {cityName = value; } 
        }
    }
}
