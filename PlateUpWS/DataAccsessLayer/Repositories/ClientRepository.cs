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
                while (reader.Read())
                {
                    clients.Add(this.modelFactory.ClientCreator.CreateModel(reader));
                }
            }
            return clients;
        }

        public Client GetById(int id)
        {
            string sql = $"SELECT * FROM Clients where ClientId=@ClientId";
            this.dbContext.AddParameter("@ClientId", id);
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                reader.Read();
                return modelFactory.ClientCreator.CreateModel(reader);
            }
        }

        public bool Update(Client item)
        {
            string sql = @$"
                        UPDATE Clients
                        SET 
                           ClientName = @ClientName,
                           ClientLastName = @ClientLastName,
                           ClientEmail = @ClientEmail,
                           ClientPassword = @ClientPassword,
                           ClientAddress = @ClientAddress,
                           ClientPhoneNumber = @ClientPhoneNumber,
                           CityId = @CityId
                        WHERE 
                           ClientId = @ClientId
                        ";
            this.dbContext.AddParameter("@ClientName", item.ClientName);
            this.dbContext.AddParameter("@ClientLastName", item.ClientLastName);
            this.dbContext.AddParameter("@ClientEmail", item.ClientEmail);
            this.dbContext.AddParameter("@ClientPassword", item.Password);
            this.dbContext.AddParameter("@ClientAddress", item.ClientAddress);
            this.dbContext.AddParameter("@ClientPhoneNumber", item.ClientPhoneNumber);
            this.dbContext.AddParameter("@CityId", item.CityId);
            this.dbContext.AddParameter("@ClientId", item.ClientId);

            return this.dbContext.Update(sql) > 0;
        }
        public string Login(string email, string password, bool isAdmin)
        {

            string sql = @"SELECT ClientId FROM Clients 
                         WHERE ClientEmail = @ClientEmail AND ClientPassword = @ClientPassword";
            this.dbContext.AddParameter("@ClientEmail", email);
            this.dbContext.AddParameter("@ClientPassword", password);
            string id = this.dbContext.GetValue(sql).ToString();
            if (isAdmin)
            {
                sql = "Select AdminId FROM ADMINS WHERE AdminId = @AdminId";
                this.dbContext.AddParameter("@AdminId", id);
                return this.dbContext.GetValue(sql) != null ? id : null;
            }
            return id;
        }
    }
}
