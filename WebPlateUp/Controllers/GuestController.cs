using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using WebApiClient;

namespace WebPlateUp.Controllers
{
    public class GuestController : Controller
    {
        [HttpGet]
        public IActionResult HomePage()
        {
            //1 get data from Web Server
            WebClient<List<Review>> client = new WebClient<List<Review>>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Guest/GetReviews";
            List<Review> reviews = client.Get();

            return View(reviews);
        }
        [HttpGet]
        public IActionResult Menu()
        {
            //1 get data from Web Server
            WebClient<MenuViewModel> client = new WebClient<MenuViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Guest/GetMenu";
            MenuViewModel menuViewModel = client.Get();

            return View(menuViewModel);
        }
        [HttpGet]
        public IActionResult MealDetails(string id)
        {
            WebClient<Meal> client = new WebClient<Meal>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Guest/GetMealDetails";
            Meal meal = client.Get();
            return View(meal);
        }
        [HttpPost]
        public IActionResult SignUp()
        {          
            return View();
        }
        public IActionResult Login()
        {
            WebClient<Client> client = new WebClient<Client>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Guest/LoginGetId";
            Client client1 = client.Get();
            return View(client1);
        }
    }
}
