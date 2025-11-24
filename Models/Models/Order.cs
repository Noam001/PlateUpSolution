using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Order
    {
        string clientId;
        int orderId;
        string orderDate;
        string orderTime;
        int numOfPeople;
        bool orderStatus;
        public int OrderId
        {
            get { return this.orderId; }
            set { this.orderId = value; }
        }
        [Required(ErrorMessage = "Client ID is required")]
        public string ClientId
        {
            get { return this.clientId; }
            set { this.clientId = value; }
        }
        [Required(ErrorMessage = "Order date is required")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "Date must be in the format DD/MM/YYYY")]
        public string OrderDate
        {
            get { return this.orderDate; }
            set { this.orderDate = value; }
        }
        [Required(ErrorMessage = "Order time is required")]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$",ErrorMessage = "Time must be in HH:mm format.")]
        public string OrderTime
        {
            get { return this.orderTime; }
            set { this.orderTime = value; }
        }
        [Required(ErrorMessage = "Number of people is required")]
        [Range(0, 50, ErrorMessage = "Number of people must be between 0 and 50")]
        public int NumOfPeople
        {
            get { return this.numOfPeople; }
            set { this.numOfPeople = value; }
        }
        [Required(ErrorMessage = "Order status is required")]
        public bool OrderStatus
        {
            get { return this.orderStatus; }
            set { this.orderStatus = value; }
        }
    }
}
