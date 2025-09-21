using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Clients
    {
        string clientId;
        string clientName;
        string clientLastName;
        string clientEmail;
        string clientPassword;
        string clientAddress;
        string clientPhoneNumber;
        int cityId;

        [Required(ErrorMessage = "You must enter your ID")]
        [RegularExpression("^\\d+$",ErrorMessage = "Invalid ID format. Please try again.")]
        [StringLength(9, MinimumLength =9, ErrorMessage = "Invalid ID format. Please try again.")]
        public string ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }
        [Required(ErrorMessage ="You must enter your name")]
        [StringLength(15, MinimumLength =2, ErrorMessage = "First name cannot be longer than 15 characters and less than 2")]
        [FirstLetterCapital(ErrorMessage = "First letter must be capital")]
        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }
        [Required(ErrorMessage = "You must enter your last name")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Last name cannot be longer than 15 characters and less than 2")]
        [FirstLetterCapital(ErrorMessage = "First letter must be capital")]
        public string ClientLastName
        {
            get { return clientLastName; }
            set { clientLastName = value; }
        }
        [Required(ErrorMessage = "You must enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string ClientEmail
        {
            get { return clientEmail; }
            set { clientEmail = value; }
        }
        [Required(ErrorMessage = "You must enter your password")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Password must be between 4 and 25 characters.")]
        [RegularExpression(@"^(?=.*\d)", ErrorMessage = "Password must contain at least one number.")]
        public string Password
        {
            get { return clientPassword; }
            set { clientPassword = value; }
        }
        [Required(ErrorMessage = "You must enter your address")]
        public string ClientAddress
        {
            get { return clientAddress; }
            set { clientAddress = value; }
        }
        [Required(ErrorMessage = "You must enter your phone number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string ClientPhoneNumber
        {
            get { return clientPhoneNumber; }
            set { clientPhoneNumber = value; }
        }
        [Required(ErrorMessage = "You must enter your city")]
        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }
    }
}
