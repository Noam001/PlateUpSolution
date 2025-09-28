using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Meal
    {
        string mealId;
        string mealName;
        string mealPhoto;
        string mealDescription;
        double mealPrice;
        string mealStatus;

        public string MealId
        {
            get { return this.mealId; }
            set { this.mealId = value; }
        }
        [Required(ErrorMessage = "You must enter the meal name")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Meal name must be between 2 and 15 characters")]
        [FirstLetterCapital(ErrorMessage = "First letter must be capital")]
        public string MealName
        {
            get { return this.mealName; }
            set { this.mealName = value; }
        }
        [Required(ErrorMessage = "You must provide a meal photo")]
        [RegularExpression(@".*\.(jpg|jpeg|png)$", ErrorMessage = "Photo must be a valid image file (jpg, jpeg, png).")]
        public string MealPhoto
        {
            get { return this.mealPhoto; }
            set { this.mealPhoto = value; }
        }
        [Required(ErrorMessage = "You must provide a description for the meal")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string MealDescription
        {
            get { return this.mealDescription; }
            set { this.mealDescription = value; }
        }
        [Required(ErrorMessage = "You must enter a price")]
        [Range(1, 50, ErrorMessage = "Price must be between 1 and 50")]
        public double MealPrice
        {
            get { return this.mealPrice; }
            set { this.mealPrice = value; }
        }
        [MealStatusAttribute(ErrorMessage = "Meal status must be either 'Available' or 'Unavailable")]
        [Required(ErrorMessage = "You must set the meal availability status")]
        public string MealStatus
        {
            get { return this.mealStatus; }
            set { this.mealStatus = value; }
        }


    }
}
