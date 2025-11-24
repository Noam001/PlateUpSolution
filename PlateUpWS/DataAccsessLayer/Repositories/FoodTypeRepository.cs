using Models;
using System.Collections.Generic;
using System.Data;

namespace PlateUpWS
{
    public class FoodTypeRepository : Repository, IRepository<FoodType>
    {
        public FoodTypeRepository(OledbContext dbContext, ModelFactory modelFactory) : base(dbContext, modelFactory)
        {
        }
        public bool Create(FoodType item)
        {
            string sql = @"INSERT INTO FoodTypes (FoodTypeName)
                           VALUES (@FoodTypeName)";

            this.dbContext.AddParameter("@FoodTypeName", item.FoodTypeName);
            return this.dbContext.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"DELETE FROM FoodTypes WHERE FoodTypeId = @FoodTypeId";
            this.dbContext.AddParameter("@FoodTypeId", id);
            return this.dbContext.Delete(sql) > 0;
        }

        public List<FoodType> GetAll()
        {
            List<FoodType> foodTypes = new List<FoodType>();
            string sql = @"SELECT * FROM FoodTypes";
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    foodTypes.Add(this.modelFactory.FoodTypeCreator.CreateModel(reader));
                }
            }
            return foodTypes;
        }

        public FoodType GetById(int id)
        {
            string sql = @"SELECT * FROM FoodTypes WHERE FoodTypeId = @FoodTypeId";
            this.dbContext.AddParameter("@FoodTypeId", id);
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                reader.Read();
                return this.modelFactory.FoodTypeCreator.CreateModel(reader);
            }
        }

        public bool Update(FoodType item)
        {
            string sql = @"UPDATE FoodTypes
                           SET FoodTypeName = @FoodTypeName
                           WHERE FoodTypeId = @FoodTypeId";
            this.dbContext.AddParameter("@FoodTypeName", item.FoodTypeName);
            this.dbContext.AddParameter("@FoodTypeId", item.FoodTypeId);

            return this.dbContext.Update(sql) > 0;
        }
    }
}
