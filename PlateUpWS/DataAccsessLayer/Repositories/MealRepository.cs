using Models;
using System.Collections.Generic;
using System.Data;

namespace PlateUpWS
{
    public class MealRepository : Repository, IRepository<Meal>
    {
        public MealRepository(OledbContext dbContext, ModelFactory modelFactory) : base(dbContext, modelFactory)
        {
        }
        public bool Create(Meal item)
        {
            string sql = @$"
                INSERT INTO Meals(MealName, MealPhoto, MealDescription, MealPrice, MealStatus)
                VALUES
                (
                    @MealName, @MealPhoto, @MealDescription, @MealPrice, @MealStatus
                )";

           
            this.dbContext.AddParameter("@MealName", item.MealName);
            this.dbContext.AddParameter("@MealPhoto", item.MealPhoto);
            this.dbContext.AddParameter("@MealDescription", item.MealDescription);
            this.dbContext.AddParameter("@MealPrice", item.MealPrice);
            this.dbContext.AddParameter("@MealStatus", item.MealStatus);

            return this.dbContext.Insert(sql) > 0;
        }
        public bool Delete(string id)
        {
            string sql = @"DELETE FROM Meals WHERE MealId = @MealId";
            this.dbContext.AddParameter("@MealId", id);
            return this.dbContext.Delete(sql) > 0;
        }
        public List<Meal> GetAll()
        {
            string sql = @"SELECT * FROM Meals";
            return GetMeals(sql);
        }
        private List<Meal> GetMeals(string sql)
        {
            List<Meal> meals = new List<Meal>();
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    meals.Add(this.modelFactory.MealCreator.CreateModel(reader));
                }
            }
            return meals;
        }
        public List<Meal> GetMealsByFoodType(string foodTypeId)
        {
            string sql = $@"SELECT FoodTypesMeals.FoodTypeId, Meals.MealId, Meals.MealName, Meals.MealPhoto, Meals.MealPrice, MealDescription, MealStatus
                          FROM Meals INNER JOIN FoodTypesMeals ON Meals.MealId = FoodTypesMeals.MealId
                          WHERE FoodTypesMeals.FoodTypeId = @FoodTypeId";
            this.dbContext.AddParameter("@FoodTypeId", foodTypeId);
            return GetMeals(sql);
        }
        public List<Meal> FilterByPage(List<Meal> meals, int pageNumber, int mealsPerPage)
        {

            int skip = (pageNumber - 1) * mealsPerPage; //כמות המנות של העמודים הקודמים שצריך לדלג
            return meals.Skip(skip).Take(mealsPerPage).ToList();
        }
        public Meal GetMealByName(string mealName)
        {
            string sql = @"SELECT * FROM Meals WHERE MealName = @MealName";
            this.dbContext.AddParameter("@MealName", mealName);
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                reader.Read();
                return this.modelFactory.MealCreator.CreateModel(reader);
            }
        }
        public List<Meal> SortByPrice(bool? option)
        {
            string sql = $@"SELECT *
                              FROM Meals ORDER BY Meals.MealPrice"; //מחיר הנמוך לגבוה
            if (option == true)
                sql = sql + " DESC"; // מהמחיר הגבוה לנמוך
           return GetMeals(sql);       
        }
        public List<Meal> SortByPriceFoodType(string foodTypeId, bool? option)
        {
            string sql = $@"SELECT Meals.MealId, Meals.MealName, Meals.MealPhoto, Meals.MealDescription, Meals.MealPrice, Meals.MealStatus, FoodTypesMeals.FoodTypeId
                           FROM Meals INNER JOIN FoodTypesMeals ON Meals.MealId = FoodTypesMeals.MealId
                           WHERE (((FoodTypesMeals.FoodTypeId)=@foodTypeId))
                           ORDER BY Meals.MealPrice";
            this.dbContext.AddParameter("@foodTypeId", foodTypeId);
            if (option == true)
                sql = sql + " DESC"; // מהמחיר הגבוה לנמוך
            return GetMeals(sql);
        }
        public List<Meal> SortByPriceFilterByPage(int pageNumber, int mealsPerPage, bool? option)
        {
            List<Meal> meals = FilterByPage(GetAll(),pageNumber, mealsPerPage);
            meals.Sort((meal1, meal2) => meal1.MealPrice.CompareTo(meal2.MealPrice));
            if (option == true)
                meals.Reverse();
            return meals;
        }
        public Meal GetById(string id)
        {
            string sql = @"SELECT * FROM Meals WHERE MealId = @MealId";
            this.dbContext.AddParameter("@MealId", id);
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                reader.Read();
                return this.modelFactory.MealCreator.CreateModel(reader);
            }
        }
        public bool Update(Meal item)
        {
            string sql = @$"
                UPDATE Meals
                SET 
                    MealName = @MealName,
                    MealPhoto = @MealPhoto,
                    MealDescription = @MealDescription,
                    MealPrice = @MealPrice,
                    MealStatus = @MealStatus
                WHERE 
                    MealId = @MealId";

            
            this.dbContext.AddParameter("@MealName", item.MealName);
            this.dbContext.AddParameter("@MealPhoto", item.MealPhoto);
            this.dbContext.AddParameter("@MealDescription", item.MealDescription);
            this.dbContext.AddParameter("@MealPrice", item.MealPrice);
            this.dbContext.AddParameter("@MealStatus", item.MealStatus);
            this.dbContext.AddParameter("@MealId", item.MealId);
            return this.dbContext.Update(sql) > 0;
        }
        public List<Meal> GetTop3MostOrdered()
        {
            string sql = @$"SELECT TOP 3 
                                Meals.MealId,
                                Meals.MealName,
                                Meals.MealPhoto,
                                Meals.MealDescription,
                                Meals.MealPrice,
                                Meals.MealStatus,
                            SUM(MealsOrders.Quantity) AS TotalOrdered
                            FROM Meals
                            INNER JOIN MealsOrders 
                                 ON Meals.MealId = MealsOrders.MealId
                            GROUP BY Meals.MealId, Meals.MealName, Meals.MealPhoto,Meals.MealDescription, 
                            Meals.MealPrice, Meals.MealStatus
                            ORDER BY SUM(MealsOrders.Quantity) DESC";
            List<Meal> meals = new List<Meal>();
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    Meal meal = this.modelFactory.MealCreator.CreateModel(reader);
                    meals.Add(meal);
                }
            }
            return meals;
        }
        public List<Meal> GetTop3LeastOrderedMeals()
        {
            string sql = $@"
                         SELECT TOP 3 Meals.MealId, Meals.MealName, Meals.MealPhoto, Meals.MealDescription,
                         Meals.MealPrice, Meals.MealStatus,
                              SUM(MealsOrders.Quantity) AS TotalOrdered
                         FROM Meals
                         INNER JOIN MealsOrders ON Meals.MealId = MealsOrders.MealId
                         GROUP BY Meals.MealId, Meals.MealName, Meals.MealPhoto, Meals.MealDescription,
                         Meals.MealPrice, Meals.MealStatus
                         ORDER BY SUM(MealsOrders.Quantity) ASC";
            List<Meal> meals = new List<Meal>();
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    Meal meal = this.modelFactory.MealCreator.CreateModel(reader);
                    meals.Add(meal);
                }
            }
            return meals;
        }
    }
}
