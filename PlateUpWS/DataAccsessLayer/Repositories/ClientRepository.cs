using Models;
using System.Collections.Generic;

namespace PlateUpWS
{
    public class ClientRepository : Repository, IRepository<Client>
    {
        public ClientRepository(IDBContext dbContext, ModelFactory modelFactory) : base(dbContext, modelFactory)
        {
        }

        public bool Create(Client item)
        {
            string sql = @$"INSERT INTO Clients(ClientId, ClientName, ClientLastName,
                            ClientEmail, ClientPassword, ClientAddress, ClientPhoneNumber,
                            CityId) VALUES ({item.ClientId}, {item.ClientName}, 
                           {item.ClientLastName}, {item.ClientEmail} ,
                           {item.Password}, {item.ClientAddress}, {item.ClientPhoneNumber},
                           {item.CityId})";
            return  this.dbContext.Insert(sql) > 0;
        }

        public bool Delete(Client item)
        {
            string sql = $@"DELETE FROM Clients WHERE ClientId = {item.ClientId}";
            return this.dbContext.Delete(sql) > 0;
        }

        public List<Client> GetAll()
        {
            throw new NotImplementedException();
        }

        public Client GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Client item)
        {
            throw new NotImplementedException();
        }
    }
}
