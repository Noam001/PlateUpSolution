using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Models;

namespace PlateUpWS
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        RepositoryFactory repositoryFactory;

        public ClientController()
        {
            this.repositoryFactory = new RepositoryFactory();
        }
        [HttpPost]
        public LoginViewModel LoginGetId([FromBody]LoginModel loginModel)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                LoginViewModel loginViewModel = this.repositoryFactory.ClientRepository.Login(loginModel);
                return loginViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpGet]
        public UpdateProfileViewModel GetUpdateProfileViewModel(string clientId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                UpdateProfileViewModel vm = new UpdateProfileViewModel();
                vm.Client = this.repositoryFactory.ClientRepository.GetById(clientId);
                vm.City = this.repositoryFactory.CityRepository.GetAll();
                return vm;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpPost]
        public bool UpdateProfile(Client client)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.ClientRepository.Update(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpPost]
        public bool MakeAnOrder(Order order)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.Create(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpPost]
        public bool AddMealToOrder(string mealId, string orderId, int price, int quantity, string? notes ="")
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.AddMealToOrder(mealId,orderId,price,quantity,notes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpGet]
        public bool RemoveMealFromOrder(string mealId, string orderId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.RemoveMealFromOrder(mealId, orderId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpPost]
        public bool CheckoutUpdateStatus(string orderId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.CheckoutUpdateStatus(orderId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }

        [HttpPost]
        public bool LeaveAReview(Review review)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.ReviewRepository.Create(review);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpGet]
        public bool RemoveReview(string reviewID)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.ReviewRepository.Delete(reviewID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpGet]
        public List<CartItem> GetCart(string clientId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.GetCart(clientId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
    }
}
