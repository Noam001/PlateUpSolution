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
                INSERT INTO Meals(MealId, MealName, MealPhoto, MealDescription, MealPrice, MealStatus)
                VALUES
                (
                    @MealId, @MealName, @MealPhoto, @MealDescription, @MealPrice, @MealStatus
                )";

            this.dbContext.AddParameter("@MealId", item.MealId);
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
            List<Meal> meals = new List<Meal>();
            string sql = @"SELECT * FROM Meals";

            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    meals.Add(this.modelFactory.MealCreator.CreateModel(reader));
                }
            }

            return meals;
        }

        public Meal GetById(int id)
        {
            string sql = @"SELECT * FROM Meals WHERE MealId = @MealId";
            this.dbContext.AddParameter("@MealId", id);
            using (IDataReader reader = this.dbContext.Select(sql))
            {
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

            this.dbContext.AddParameter("@MealId", item.MealId);
            this.dbContext.AddParameter("@MealName", item.MealName);
            this.dbContext.AddParameter("@MealPhoto", item.MealPhoto);
            this.dbContext.AddParameter("@MealDescription", item.MealDescription);
            this.dbContext.AddParameter("@MealPrice", item.MealPrice);
            this.dbContext.AddParameter("@MealStatus", item.MealStatus);

            return this.dbContext.Update(sql) > 0;
        }
    }
}
