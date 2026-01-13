using Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Security.Cryptography;

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
                         ClientEmail, ClientPassword, ClientAddress, ClientPhoneNumber, CityId, ClientSalt)
                         VALUES
                         (
                             @ClientId, @ClientName, @ClientLastName, @ClientEmail, @ClientPassword, 
                             @ClientAddress, @ClientPhoneNumber, @CityId, @ClientSalt
                         )";
            string salt = GenerateSalt();
            this.dbContext.AddParameter("@ClientId", item.ClientId);
            this.dbContext.AddParameter("@ClientName", item.ClientName);
            this.dbContext.AddParameter("@ClientLastName", item.ClientLastName);
            this.dbContext.AddParameter("@ClientEmail", item.ClientEmail);
            this.dbContext.AddParameter("@ClientPassword", CalculateHash(item.Password, salt));
            this.dbContext.AddParameter("@ClientAddress", item.ClientAddress);
            this.dbContext.AddParameter("@ClientPhoneNumber", item.ClientPhoneNumber);
            this.dbContext.AddParameter("@CityId", item.CityId);
            this.dbContext.AddParameter("@ClientSalt", salt);

            return this.dbContext.Insert(sql) > 0;
        }
        private string GenerateSalt()
        {
            byte[] saltbytes = new byte[16];
            RandomNumberGenerator.Fill(saltbytes);
            return Convert.ToBase64String(saltbytes);
        }
        private string CalculateHash(string password, string salt)
        {
            string s = password + salt;
            byte[] pass = System.Text.Encoding.UTF8.GetBytes(s);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(pass);
                return Convert.ToBase64String(bytes);
            }
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

        public Client GetById(string id)
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
        public LoginViewModel Login(LoginModel login)
        {

            string sql = @"SELECT ClientId, ClientPassword, ClientSalt FROM Clients 
                         WHERE ClientEmail = @ClientEmail";
            this.dbContext.AddParameter("@ClientEmail", login.Email);
            string hash = string.Empty;
            string salt = string.Empty;
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.Name = GetById(loginViewModel.ClientId).ClientName;
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                if (reader.Read())
                {
                    salt = reader["ClientSalt"].ToString();
                    hash = reader["ClientPassword"].ToString();
                    loginViewModel.ClientId = reader["ClientId"].ToString();
                }
                if (hash == CalculateHash(login.Password, salt))
                {
                    if (login.IsAdmin == false)
                        return loginViewModel;
                    else
                    {
                        sql = "Select AdminId FROM ADMINS WHERE AdminId = @AdminId";
                        this.dbContext.AddParameter("@AdminId", loginViewModel.ClientId);
                        return this.dbContext.GetValue(sql) != null ? loginViewModel : null;
                    }
                }
                return null;
            }

        }
    }
}
