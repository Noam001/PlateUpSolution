using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol.Core.Types;
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
        public IActionResult Menu(string foodTypeId = "-1", int pageNumber = 1, string mealNameSearch = "", bool? priceSort = null, int pages = 0)
        {
            //1 get data from Web Server
            WebClient<MenuViewModel> client = new WebClient<MenuViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Guest/GetMenu";
            if (foodTypeId != "-1")
                client.AddParameter("foodTypeId", foodTypeId);

            if (pageNumber > 0)
                client.AddParameter("pageNumber", pageNumber.ToString());

            if (mealNameSearch != "")
                client.AddParameter("mealNameSearch", mealNameSearch);

            if (priceSort != null)
                client.AddParameter("priceSort", priceSort.ToString());

            MenuViewModel menuViewModel = client.Get();
            menuViewModel.FoodTypeId = foodTypeId;
            menuViewModel.PageNumber = pageNumber;
            menuViewModel.MealNameSearch = mealNameSearch;
            menuViewModel.PriceSort = priceSort;


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
            client.AddParameter("mealId", id);
            Meal meal = client.Get();
            return View(meal);
        }

        [HttpGet]
        public IActionResult ViewRegistration()
        {
            WebClient<ClientFormViewModel> client = new WebClient<ClientFormViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Guest/GetRegistrationViewModel";
            string sessionId = HttpContext.Session.GetString("clientId");

            if (sessionId != null)
            {
                client.Path = "api/Client/GetUpdateProfileViewModel";
                client.AddParameter("clientId", sessionId);
            }
            ClientFormViewModel clientFormViewModel = client.Get();
            // תיקון - אתחול אם null
            if (clientFormViewModel.Client == null)
            {
                clientFormViewModel.Client = new Client();
            }
            if (ViewBag.Error != null)
            {
                clientFormViewModel.Client = TempData["client"] as Client;
            }
            return View("ClientForm", clientFormViewModel);
        }
        [HttpPost]

        public IActionResult Registration(Client client)
        {
            if (!ModelState.IsValid) //בדיקת תקינות הקלט
                return View("ClientForm", GetRegistrationViewModel(client));
            bool ok = PostClient(client); //האם שליחה למסד נתונים עבדה
            if (ok)
            {
                HttpContext.Session.SetString("clientId", client.ClientId);
                return RedirectToAction("HomePage", "Guest"); //מעביר דף במידה והצליח
            }
            ViewBag.ErrorMessage = "Registration faild, Try Again."; //אם לא הצליח להירשם, יוסף לו הודעת שגיאה 
            return View("ClientForm", GetRegistrationViewModel(client));
        }
        //Get RegistrationViewModel from WS
        private ClientFormViewModel GetRegistrationViewModel(Client client)
        {
            WebClient<ClientFormViewModel> webClient = new WebClient<ClientFormViewModel>();
            webClient.Schema = "http";
            webClient.Host = "localhost";
            webClient.Port = 5035;
            webClient.Path = "api/Guest/GetRegistrationViewModel";
            ClientFormViewModel signUpVM = webClient.Get();
            signUpVM.Client = client;
            return signUpVM;
        }
        //send client to ws/database
        private bool PostClient(Client client)
        {
            WebClient<Client> webClient = new WebClient<Client>();
            webClient.Schema = "http";
            webClient.Host = "localhost";
            webClient.Port = 5035;
            webClient.Path = "api/Guest/Registration";
            return webClient.Post(client);
        }
    }
}
