using Models;
using System.Data;

namespace PlateUpWS
{
    public class CityCreator: IModelCreator<City>
    {
        public City CreateModel(IDataReader reader)
        {
            return new City()
            {
                CityId = Convert.ToUInt16(reader["CityId"]),
                CityName = Convert.ToString(reader["CityName"])
            };
        }
    }
}
