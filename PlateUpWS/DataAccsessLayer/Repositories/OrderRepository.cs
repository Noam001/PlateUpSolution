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
                INSERT INTO Orders(ClientId, OrderDate, OrderTime, NumOfPeople, OrderStatus)
                VALUES
                (
                    @ClientId, @OrderDate, @OrderTime, @NumOfPeople, @OrderStatus
                )";

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
                reader.Read();
                return this.modelFactory.OrderCreator.CreateModel(reader);
            }
        }
        public List<Order> GetOrdersByStatus(bool? orderStatus)
        {
            string sql = @"SELECT * FROM Orders WHERE OrderStatus = @OrderStatus";
            this.dbContext.AddParameter("@OrderStatus", orderStatus);
            List<Order> orders = new List<Order>();
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    orders.Add(this.modelFactory.OrderCreator.CreateModel(reader));
                }
            }
            return orders;
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
        public bool UpdateOrderStatus(int orderId, bool orderStatus)
        {
            string sql = $@"UPDATE Orders
                            SET OrderStatus = @OrderStatus
                            WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderStatus", orderStatus);
            this.dbContext.AddParameter("@OrderId", orderId);
            return this.dbContext.Update(sql) > 0;
        }

        public bool AddMealToOrder(string mealId, string orderId, int price, int quantity, string? notes ="")
        {
            string sql = @$"INSERT INTO MealsOrders (MealID, OrderID, Quantity, MealPrice, MealNotes)
                            VALUES (@MealID, @OrderID, @Quantity, @MealPrice, @MealNotes)";
            this.dbContext.AddParameter("@MealID", mealId);
            this.dbContext.AddParameter("@OrderID", orderId);
            this.dbContext.AddParameter("@Quantity", quantity);
            this.dbContext.AddParameter("@MealPrice", price);
            this.dbContext.AddParameter("@MealNotes", notes);
            return this.dbContext.Insert(sql) > 0;
        }
        public bool RemoveMealFromOrder(string mealId, string orderId)
        {
            string sql = $@"DELETE FROM MealsOrders WHERE MealID= @MealID AND OrderID = @OrderID";
            this.dbContext.AddParameter("@MealID", mealId);
            this.dbContext.AddParameter("@OrderID", orderId);
            return this.dbContext.Delete(sql) > 0;
        }
        public bool CheckoutUpdateStatus(string orderId)
        {
            string sql = $@"UPDATE Orders
                            SET OrderStatus = True
                            WHERE OrderId = @OrderId;";
            this.dbContext.AddParameter("@OrderId", orderId);
            return this.dbContext.Update(sql) > 0;
        }
        public List<CartItem> GetCart(string clientId)
        {
            string sql = @"SELECT Meals.MealId, Meals.MealName, Meals.MealPhoto, Meals.MealDescription, Meals.MealPrice, Meals.MealStatus, MealsOrders.Quantity, MealsOrders.OrderID, Orders.ClientId, Orders.OrderStatus
                           FROM Orders INNER JOIN (Meals INNER JOIN MealsOrders ON Meals.MealId = MealsOrders.MealID) ON Orders.OrderId = MealsOrders.OrderID
                           WHERE Orders.ClientId = @ClientId AND Orders.OrderStatus = False;";
            this.dbContext.AddParameter("@ClientId", clientId);
            List<CartItem> cartItems = new List<CartItem>();
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    CartItem cartItem = new CartItem();
                    cartItem.Meal = this.modelFactory.MealCreator.CreateModel(reader);
                    cartItem.Quantity = Convert.ToInt16(reader["Quantity"]);
                    cartItems.Add(cartItem);
                }
            }
            return cartItems;
        }
        public int GetTotalOrdersInDateRange(string fromDate, string toDate)
        {
            string sql = @"SELECT 
                          COUNT(*) AS TotalOrders
                          FROM Orders
                          WHERE 
                               CDate(Orders.OrderDate) 
                          BETWEEN CDate('2025-09-05') AND CDate('2025-09-09');";
            this.dbContext.AddParameter("@FromDate", fromDate);
            this.dbContext.AddParameter("@ToDate", toDate);
            int totalOrders = 0;
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                if (reader.Read())
                {
                    totalOrders = Convert.ToInt32(reader["TotalOrders"]);
                }
            }
            return totalOrders;
        }
        public double GetTotalIncomeInDateRange(string fromDate, string toDate)
        {
            string sql = @"SELECT 
                         SUM(MealsOrders.Quantity * MealsOrders.MealPrice) AS TotalIncome
                         FROM Orders
                         INNER JOIN MealsOrders
                              ON Orders.OrderId = MealsOrders.OrderID
                         WHERE 
                             CDate(Orders.OrderDate) 
                                 BETWEEN CDate(@FromDate) AND CDate(@ToDate);";

            this.dbContext.AddParameter("@FromDate", fromDate);
            this.dbContext.AddParameter("@ToDate", toDate);
            double totalIncome = 0;
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                if (reader.Read())
                {
                    if (reader["TotalIncome"] != DBNull.Value) //רק אם יש הכנסות- הערך שונה מכלום
                        totalIncome = Convert.ToDouble(reader["TotalIncome"]);
                }
            }
            return totalIncome;
        }
    }
}
