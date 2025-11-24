using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class FirstLetterCapitalAttribute: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string str = value as string;
            if (!Char.IsUpper(str[0]))
            {
                // If the first character is not uppercase, the validation fails.
                return false;
            }
            // Return true if the validation condition (first letter is capital) is met.
            return true;
        }
    }
}
