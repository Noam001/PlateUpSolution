using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Meal : Model
    {
        int mealId;
        string mealName;
        string mealPhoto;
        string mealDescription;
        double mealPrice;
        bool mealStatus=false;

        public int MealId
        {
            get { return this.mealId; }
            set { this.mealId = value; }
        }
        [Required(ErrorMessage = "You must enter the meal name")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Meal name must be between 2 and 35 characters")]
        [FirstLetterCapital(ErrorMessage = "First letter must be capital")]
        public string MealName
        {
            get { return this.mealName; }
            set { this.mealName = value;
                ValidateProperty(value,"MealName");
            }
        }
        [Required(ErrorMessage = "You must provide a meal photo")]
        [RegularExpression(@".*\.(jpg|jpeg|png)$", ErrorMessage = "Photo must be a valid image file (jpg, jpeg, png).")]
        public string MealPhoto
        {
            get { return this.mealPhoto; }
            set { this.mealPhoto = value;
                ValidateProperty(value, "MealPhoto");
            }
        }
        [Required(ErrorMessage = "You must provide a description for the meal")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 200 characters")]
        public string MealDescription
        {
            get { return this.mealDescription; }
            set { this.mealDescription = value;
                ValidateProperty(value, "MealDescription");
            }
        }
        [Required(ErrorMessage = "You must enter a price")]
        [Range(1, 50, ErrorMessage = "Price must be between 1 and 50")]
        public double MealPrice
        {
            get { return this.mealPrice; }
            set { this.mealPrice = value;
                ValidateProperty(value, "MealPrice");
            }
        }
        [Required(ErrorMessage = "You must set the meal availability status")]
        public bool MealStatus
        {
            get { return this.mealStatus; }
            set { this.mealStatus = value;
                ValidateProperty(value, "MealStatus");
            }
        }


    }
}
