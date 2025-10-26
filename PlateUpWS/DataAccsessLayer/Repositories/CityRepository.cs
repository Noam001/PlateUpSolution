using Models;

namespace PlateUpWS
{
    public class CityRepository : Repository, IRepository<City>
    {
        public CityRepository(OledbContext dbContext, ModelFactory modelFactory) : base(dbContext, modelFactory)
        {
        }

        public bool Create(City item)
        {
            string sql = "I"
        }

        public bool Delete(City item)
        {
            throw new NotImplementedException();
        }

        public List<City> GetAll()
        {
            throw new NotImplementedException();
        }

        public City GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(City item)
        {
            throw new NotImplementedException();
        }
    }
}
