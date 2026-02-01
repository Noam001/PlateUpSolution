using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Models;
using System.Text.Json;

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
        public ClientFormViewModel GetUpdateProfileViewModel(string clientId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                ClientFormViewModel vm = new ClientFormViewModel();
                vm.Client = this.repositoryFactory.ClientRepository.GetById(clientId);
                vm.Cities = this.repositoryFactory.CityRepository.GetAll();
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
        public bool AddMealToOrder(OrderItem cartItem)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.AddMealToOrder(cartItem);
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
        public CartViewModel GetCart(string clientId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                CartViewModel vm = this.repositoryFactory.OrderRepository.GetCart(clientId);
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
        [HttpGet]
        public bool UpdateQuantity(int mealId, int orderId, int quantity)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.UpdateQuantity(mealId, orderId, quantity);
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
    }
}
