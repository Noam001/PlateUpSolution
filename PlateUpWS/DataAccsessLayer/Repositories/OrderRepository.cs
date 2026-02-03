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

        public Order GetById(string id)
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
        public bool AddMealToOrder(AddMealRequest addMealReq)
        {
            string sql = @"SELECT OrderId FROM Orders 
                             WHERE ClientId = @ClientId AND OrderStatus = False"; // 1. מציאת ה-OrderId של העגלה הפעילה

            this.dbContext.AddParameter("@ClientId", addMealReq.ClientId);
            string orderId = null;
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                if (reader.Read())
                {
                    orderId = reader["OrderId"].ToString();
                }
            }
            if (orderId == null) //יצירת עגלה חדשה אם לא קיימת 
            {
                string sqlCreateOrder = @"INSERT INTO Orders(ClientId, OrderDate, OrderTime, NumOfPeople, OrderStatus)
                                  VALUES (@ClientId, @OrderDate, @OrderTime, @NumOfPeople, @OrderStatus)"; //הוספת עגלה

                this.dbContext.AddParameter("@ClientId", addMealReq.ClientId);
                this.dbContext.AddParameter("@OrderDate", DateTime.Today.ToString("dd/MM/yyyy"));
                this.dbContext.AddParameter("@OrderTime", DateTime.Now.ToString("HH:mm"));
                this.dbContext.AddParameter("@NumOfPeople", 0);
                this.dbContext.AddParameter("@OrderStatus", false);

                if (this.dbContext.Insert(sqlCreateOrder) > 0) //אם נוצר הזמנה חדשה
                {
                    this.dbContext.AddParameter("@ClientId", addMealReq.ClientId);
                    using (IDataReader reader = this.dbContext.Select(sql))
                    {
                        if (reader.Read())
                        {
                            orderId = reader["OrderId"].ToString(); //שמור ID שלה
                        }
                    }
                }
                if (orderId == null) return false; 
            }
            // ניסיון לעדכן מנה קיימת הכמות גדלה, ההערה מתעדכנת להכי חדשה
            string sqlUpdateQuantity = @"UPDATE MealsOrders 
                         SET Quantity = Quantity + @NewQty, 
                             MealNotes = @NewNotes
                         WHERE OrderID = @OID AND MealID = @MID";
            this.dbContext.AddParameter("@NewQty", addMealReq.Quantity);
            this.dbContext.AddParameter("@NewNotes", addMealReq.MealNotes ?? "");
            this.dbContext.AddParameter("@OID", orderId);
            this.dbContext.AddParameter("@MID", addMealReq.MealId);

            // בודק האם קיים מנה באותה עגלה של הלקוח עם אותה מנה והערה זהות לאלה שביקש
            if (this.dbContext.Update(sqlUpdateQuantity) > 0)
            {
                return true;
            }
            string sqlGetPrice = @"SELECT MealPrice FROM Meals WHERE MealId = @MealID";
            this.dbContext.AddParameter("@MealID", addMealReq.MealId);
            double realMealPrice = 0;
            using (IDataReader reader = this.dbContext.Select(sqlGetPrice))
            {
                if (reader.Read())
                {
                    realMealPrice = Convert.ToDouble(reader["MealPrice"]);//שמירת המחיר לתוך המנה החדשה
                }
            }
            string sqlInsertMeal = @"INSERT INTO MealsOrders (MealID, OrderID, Quantity, MealPrice, MealNotes)
                             VALUES (@MealID, @OrderID, @Quantity, @MealPrice, @MealNotes)";
            this.dbContext.AddParameter("@MealID", addMealReq.MealId);
            this.dbContext.AddParameter("@OrderID", orderId);
            this.dbContext.AddParameter("@Quantity", addMealReq.Quantity);
            this.dbContext.AddParameter("@MealPrice", realMealPrice);
            this.dbContext.AddParameter("@MealNotes", addMealReq.MealNotes ?? "");
            return this.dbContext.Insert(sqlInsertMeal) > 0;
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
                            SET                              
                               OrderDate = @OrderDate,
                               OrderTime = @OrderTime,
                               OrderStatus = True
                            WHERE OrderId = @OrderId;";
            this.dbContext.AddParameter("@OrderDate", DateTime.Today.ToString("dd/MM/yyyy"));
            this.dbContext.AddParameter("@OrderTime", DateTime.Now.ToString("HH:mm"));
            this.dbContext.AddParameter("@OrderId", orderId);
            return this.dbContext.Update(sql) > 0;
        }
        public CartViewModel GetCart(string clientId)
        {
            string sql = @"SELECT Meals.MealId, Meals.MealName, MealsOrders.Quantity, MealsOrders.OrderID, MealsOrders.MealPrice, MealsOrders.MealNotes, Orders.ClientId, Orders.OrderStatus
                           FROM Orders INNER JOIN (Meals INNER JOIN MealsOrders ON Meals.MealId = MealsOrders.MealID) ON Orders.OrderId = MealsOrders.OrderID
                           WHERE Orders.ClientId = @ClientId AND Orders.OrderStatus = False;";
            this.dbContext.AddParameter("@ClientId", clientId);
            List<CartItem> cartItems = new List<CartItem>();
            double totalPrice = 0;
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    CartItem cartItem = new CartItem
                    {
                        MealId = Convert.ToInt32(reader["MealId"]),
                        MealName = reader["MealName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        MealPrice = Convert.ToDouble(reader["MealPrice"]),
                        MealNotes = reader["MealNotes"]?.ToString() ?? "",
                        ClientId = reader["ClientId"].ToString()
                    };
                    cartItems.Add(cartItem);
                    totalPrice += cartItem.Quantity * cartItem.MealPrice;
                }
            }
            CartViewModel cart = new CartViewModel();
            cart.CartItems = cartItems;
            cart.TotalItems = cartItems.Sum(x => x.Quantity);
            cart.TotalPrice = totalPrice;
            return cart;
        }
        public bool UpdateQuantity(int mealId, int orderId, int quantity)
        {
            string sql = @"UPDATE MealsOrders 
                   SET Quantity = @Quantity
                   WHERE MealID = @MealID AND OrderID = @OrderID";

            this.dbContext.AddParameter("@Quantity", quantity);
            this.dbContext.AddParameter("@MealID", mealId);
            this.dbContext.AddParameter("@OrderID", orderId);

            return this.dbContext.Update(sql) > 0;
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
