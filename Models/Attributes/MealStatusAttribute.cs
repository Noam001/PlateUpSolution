using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MealStatusAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string str = value.ToString();
            if (str == "Available" || str == "Unavailable")
                return true;
            return false;
            return base.IsValid(value);
        }
    }
}
