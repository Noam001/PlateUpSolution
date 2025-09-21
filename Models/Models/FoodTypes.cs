using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FoodTypes
    {
        int foodTypeId;
        string foodTypeName;

        public int FoodTypeId
        {
            get { return this.foodTypeId; }
            set { this.foodTypeId = value; }
        }
        [Required(ErrorMessage = "You must enter Food Type Name")]
        [RegularExpression(@"^[A-Za-z\s]{3,}$", ErrorMessage = "Food type name must be at least 3 characters long and cannot contain numbers.")]
        public string FoodTypeName
        {
            get { return this.foodTypeName; }
            set { this.foodTypeName = value; }
        }


    }
}
