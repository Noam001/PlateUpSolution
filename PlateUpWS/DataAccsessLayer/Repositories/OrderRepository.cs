using Models;
using System.Collections.Generic;
using System.Data;

namespace PlateUpWS
{
    public class OrderRepository: Repository, IRepository<Order>
    {
        public OrderRepository(OledbContext dbContext, ModelFactory modelFactory)
            : base(dbContext, modelFactory)
        {
        }

        public bool Create(Order item)
        {
            string sql = @$"
                INSERT INTO Orders(OrderId, ClientId, OrderDate, OrderTime, NumOfPeople, OrderStatus)
                VALUES
                (
                    @OrderId, @ClientId, @OrderDate, @OrderTime, @NumOfPeople, @OrderStatus
                )";

            this.dbContext.AddParameter("@OrderId", item.OrderId);
            this.dbContext.AddParameter("@ClientId", item.ClientId);
            this.dbContext.AddParameter("@OrderDate", item.OrderDate);
            this.dbContext.AddParameter("@OrderTime", item.OrderTime);
            this.dbContext.AddParameter("@NumOfPeople", item.NumOfPeople);
            this.dbContext.AddParameter("@OrderStatus", item.OrderStatus);

            return this.dbContext.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"DELETE FROM Orders WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderId", id);
            return this.dbContext.Delete(sql) > 0;
        }

        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();
            string sql = @"SELECT * FROM Orders";

            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    orders.Add(this.modelFactory.OrderCreator.CreateModel(reader));
                }
            }

            return orders;
        }

        public Order GetById(int id)
        {
            string sql = @"SELECT * FROM Orders WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderId", id);

            using (IDataReader reader = this.dbContext.Select(sql))
            {
                if (reader.Read())
                {
                    return this.modelFactory.OrderCreator.CreateModel(reader);
                }
            }

            return null;
        }

        public bool Update(Order item)
        {
            string sql = @$"
                UPDATE Orders
                SET 
                    ClientId = @ClientId,
                    OrderDate = @OrderDate,
                    OrderTime = @OrderTime,
                    NumOfPeople = @NumOfPeople,
                    OrderStatus = @OrderStatus
                WHERE 
                    OrderId = @OrderId";

            this.dbContext.AddParameter("@OrderId", item.OrderId);
            this.dbContext.AddParameter("@ClientId", item.ClientId);
            this.dbContext.AddParameter("@OrderDate", item.OrderDate);
            this.dbContext.AddParameter("@OrderTime", item.OrderTime);
            this.dbContext.AddParameter("@NumOfPeople", item.NumOfPeople);
            this.dbContext.AddParameter("@OrderStatus", item.OrderStatus);

            return this.dbContext.Update(sql) > 0;
        }
    }
}
