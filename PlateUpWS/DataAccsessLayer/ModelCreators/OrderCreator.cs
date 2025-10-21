using Models;
using System.Data;
namespace PlateUpWS
{
    public class OrderCreator : IModelCreator<Order>
    {
        public Order CreateModel(IDataReader reader)
        {
            return new Order
            {
                ClientId = Convert.ToString(reader["ClientId"]),
                OrderId = Convert.ToUInt16(reader["OrderId"]),
                OrderDate = Convert.ToString(reader["OrderDate"]),
                OrderTime = Convert.ToString(reader["OrderTime"]),
                NumOfPeople = Convert.ToUInt16(reader["NumOfPeople"]),
                OrderStatus = Convert.ToBoolean(reader["OrderStatus"])
            };
        }
    }
}
