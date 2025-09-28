using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Reviews
    {
        string clientId;
        int reviewId;
        string reviewDate;
        string reviewComment;
        int reviewRating;

        [Required(ErrorMessage = "Client ID is required")]
        public string ClientId
        {
            get { return this.clientId; }
            set { this.clientId = value; }
        }

        public int ReviewId
        {
            get { return this.reviewId; }
            set { this.reviewId = value; }
        }
        [Required(ErrorMessage = "Review date is required")]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in the format YYYY-MM-DD")]
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
