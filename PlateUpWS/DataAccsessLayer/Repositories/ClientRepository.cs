using Models;
using System.Collections.Generic;
using System.Data;

namespace PlateUpWS
{
    public class ClientRepository : Repository, IRepository<Client>
    {
        public ClientRepository(OledbContext dbContext, ModelFactory modelFactory) : base(dbContext, modelFactory)
        {
        }

        public bool Create(Client item)
        {
            //string sql = @$"INSERT INTO Clients(ClientId, ClientName, ClientLastName,
            //                ClientEmail, ClientPassword, ClientAddress, ClientPhoneNumber,
            //                CityId) VALUES ({item.ClientId}, {item.ClientName}, 
            //               {item.ClientLastName}, {item.ClientEmail} ,
            //               {item.Password}, {item.ClientAddress}, {item.ClientPhoneNumber},
            //               {item.CityId})";
            string sql = @$"INSERT INTO Clients(ClientId, ClientName, ClientLastName, 
                         ClientEmail, ClientPassword, ClientAddress, ClientPhoneNumber,CityId)
                         VALUES
                         (
                             @ClientId, @ClientName, @ClientLastName, @ClientEmail, @ClientPassword, 
                             @ClientAddress, @ClientPhoneNumber, @CityId
                         )";
            this.dbContext.AddParameter("@ClientId", item.ClientId);
            this.dbContext.AddParameter("@ClientName", item.ClientName);
            this.dbContext.AddParameter("@ClientLastName", item.ClientLastName);
            this.dbContext.AddParameter("@ClientAddress", item.ClientAddress);
            this.dbContext.AddParameter("@ClientPhoneNumber", item.ClientPhoneNumber);
            this.dbContext.AddParameter("@CityId", item.CityId);

            return this.dbContext.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = $@"DELETE FROM Clients WHERE ClientId = @ClientId";
            this.dbContext.AddParameter("@ClientId", id);
            return this.dbContext.Delete(sql) > 0;
        }

        public List<Client> GetAll()
        {
            List<Client> clients = new List<Client>();
            string sql = $@"SELECT * FROM [Client]";
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while(reader.Read())
                {
                    clients.Add(this.modelFactory.ClientCreator.CreateModel(reader));   
                }
            }
            return clients;
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
