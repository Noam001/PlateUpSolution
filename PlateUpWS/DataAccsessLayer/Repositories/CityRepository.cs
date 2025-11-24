using Models;
using System.Data;

namespace PlateUpWS
{
    public class CityRepository : Repository, IRepository<City>
    {
        public CityRepository(OledbContext dbContext, ModelFactory modelFactory) : base(dbContext, modelFactory)
        {
        }

        public bool Create(City item)
        {
            string sql = @"INSERT INTO Cities (CityName)
                           VALUES (@CityName)";

            this.dbContext.AddParameter("@CityName", item.CityName);
            return this.dbContext.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"DELETE FROM Cities WHERE CityId = @CityId";
            this.dbContext.AddParameter("@CityId", id);
            return this.dbContext.Delete(sql) > 0;
        }

        public List<City> GetAll()
        {
            List<City> cities = new List<City>();
            string sql = @"SELECT * FROM Cities";

            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    cities.Add(this.modelFactory.CityCreator.CreateModel(reader));
                }
            }

            return cities;
        }

        public City GetById(int id)
        {
            string sql = @"SELECT * FROM Cities WHERE CityId = @CityId";
            this.dbContext.AddParameter("@CityId", id);
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                reader.Read();
                return this.modelFactory.CityCreator.CreateModel(reader);
            }
        }
        public bool Update(City item)
        {
            string sql = @"UPDATE Cities
                           SET CityName = @CityName
                           WHERE CityId = @CityId";
            this.dbContext.AddParameter("@CityName", item.CityName);
            this.dbContext.AddParameter("@CityId", item.CityId);
            return this.dbContext.Update(sql) > 0;
        }
    }
}
