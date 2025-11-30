using Models;
using System.Data;
namespace PlateUpWS
{
    public class ClientCreator : IModelCreator<Client>
    {
        public Client CreateModel(IDataReader reader)
        {
            return new Client()
            {
                ClientId = Convert.ToString(reader["ClientId"]),
                ClientName = Convert.ToString(reader["ClientName"]),
                ClientLastName = Convert.ToString(reader["ClientLastName"]),
                ClientEmail = Convert.ToString(reader["ClientEmail"]),
                Password = Convert.ToString(reader["ClientPassword"]),
                ClientAddress = Convert.ToString(reader["ClientAddress"]),
                ClientPhoneNumber = Convert.ToString(reader["ClientPhoneNumber"]),
                CityId = Convert.ToUInt16(reader["CityId"]),
                ClientSalt = Convert.ToString(reader["ClientSalt"])
            };
        }
    }
}
