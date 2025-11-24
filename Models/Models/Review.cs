using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Review
    {
        int reviewId;
        string clientId;
        string reviewDate;
        string reviewComment;
        int reviewRating;
        public int ReviewId
        {
            get { return this.reviewId; }
            set { this.reviewId = value; }
        }
        [Required(ErrorMessage = "Client ID is required")]
        public string ClientId
        {
            get { return this.clientId; }
            set { this.clientId = value; }
        }
        [Required(ErrorMessage = "Review date is required")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "Date must be in the format DD/MM/YYYY")]
        public string ReviewDate
        {
            get { return this.reviewDate; }
            set { this.reviewDate = value; }
        }
        [Required(ErrorMessage = "You must enter your review comment")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "Review must be at least 4 characters and max 250 characters")]
        public string ReviewComment
        {
            get { return this.reviewComment; }
            set { this.reviewComment = value; }
        }
        [Required(ErrorMessage = "You must give a rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int ReviewRating
        {
            get { return this.reviewRating; }
            set { this.reviewRating = value; }
        }


    }
}
