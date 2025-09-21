using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
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
        public string ClientEmail
        {
            get { return clientEmail; }
            set { clientEmail = value; }
        }
        public string Password
        {
            get { return clientPassword; }
            set { clientPassword = value; }
        }
        public string ClientAddress
        {
            get { return clientAddress; }
            set { clientAddress = value; }
        }
        public string ClientPhoneNumber
        {
            get { return clientPhoneNumber; }
            set { clientPhoneNumber = value; }
        }
        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }
    }
}
