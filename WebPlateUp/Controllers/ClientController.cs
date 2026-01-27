using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol.Core.Types;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApiClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebPlateUp.Controllers
{
    public class ClientController : Controller
    {
        [HttpGet]
        public IActionResult ViewLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            LoginModel loginModel = new LoginModel();
            loginModel.Email = email;
            loginModel.Password = password;
            loginModel.IsAdmin = false;
            LoginViewModel loginViewModel = ClientLogin(loginModel);

            // 4. בדיקת התוצאה
            if (loginViewModel.ClientId != null)
            {
                // הצלחה- שומרים בסשן ועוברים לדף הבית
                HttpContext.Session.SetString("clientId", loginViewModel.ClientId);
                HttpContext.Session.SetString("clientName", loginViewModel.Name);
                return RedirectToAction("HomePage", "Guest");
            }
            else
            {
                // כישלון - נחזור לדף ההתחברות עם הודעת שגיאה
                ViewBag.ErrorMessage = "Invalid Email or Password.";
                return View("ViewLogin");
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("HomePage", "Guest");
        }
        [HttpPost]
        public IActionResult LeaveAReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                WebClient<HomePageViewModel> clientReviews = new WebClient<HomePageViewModel>();
                clientReviews.Schema = "http";
                clientReviews.Host = "localhost";
                clientReviews.Port = 5035;
                clientReviews.Path = "api/Guest/GetReviews";
                HomePageViewModel viewModel = clientReviews.Get();
                viewModel.Review = review;

                return View("~/Views/Guest/HomePage.cshtml", viewModel);
            }
            WebClient<Review> client = new WebClient<Review>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/LeaveAReview";

            bool ok = client.Post(review);
            if (ok) //האם שליחה למסד נתונים עבדה
                return RedirectToAction("HomePage", "Guest");
            ViewBag.ErrorMessage = "Review Request faild, Try Again.";
            return View("HomePage", "Guest");
        }
        [HttpPost]
        public IActionResult UpdateProfile(Client client)
        {
            ModelState.Remove("client.Password");
            if (!ModelState.IsValid) //בדיקת תקינות הקלט
            {
                TempData["client"] = JsonSerializer.Serialize<Client>(client);
                return RedirectToAction("ViewRegistration", "Guest");
            }
            client.Password = "XXXXX5"; // נדרשת סיסמה ברירת מחדל למילוי שדות החובה לצורך עדכון המשתמש
            WebClient<Client> webClient = new WebClient<Client>();
            webClient.Schema = "http";
            webClient.Host = "localhost";
            webClient.Port = 5035;
            webClient.Path = "api/Client/UpdateProfile";
            bool ok = webClient.Post(client);
            if (ok) //האם שליחה למסד נתונים עבדה
            {
                HttpContext.Session.SetString("clientName", client.ClientName);
                return RedirectToAction("HomePage", "Guest");
            }
            ViewBag.Error = true;
            return RedirectToAction("ViewRegistration", "Guest");
        }
        [HttpGet]
        public IActionResult ViewTableReservation()
        {
            ViewBag.ClientId = HttpContext.Session.GetString("clientId");
            return View();
        }
        [HttpPost]
        public IActionResult MakeAReservation(Order order)
        {
            WebClient<Order> client = new WebClient<Order>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/MakeAnOrder";
            order.OrderStatus = false;
            if (!ModelState.IsValid) //בדיקת תקינות הקלט
                return View("ViewTableReservation",order);
            bool ok = client.Post(order);
            if(ok)
            {
                return RedirectToAction("HomePage", "Guest");
            }
            ViewBag.ErrorMessage = "Reservation Request faild, Try Again.";
            return View("ViewTableReservation", order);
        }
        [HttpPost]
        public IActionResult AddMealToOrder(string mealId, string orderId, int price, int quantity, string? notes = "")
        {
            WebClient<Meal> client = new WebClient<Meal>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/AddMealToOrder";

            return View();
        }
        private LoginViewModel ClientLogin(LoginModel loginModel)
        {

            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                HttpClient httpClient = new HttpClient();   
                requestMessage.Method = HttpMethod.Post;
                requestMessage.RequestUri = new Uri("http://localhost:5035/api/Client/LoginGetId");
                string jsondata = JsonSerializer.Serialize(loginModel); //מעביר את הפורמט של האובייקט לפורמט גייסון
                requestMessage.Content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                using (HttpResponseMessage responseMessage = httpClient.SendAsync(requestMessage).Result)
                {
                    if (responseMessage.IsSuccessStatusCode == true) //האם הבקשה הצליחה(קיבלה קוד 200)
                    {
                        string result = responseMessage.Content.ReadAsStringAsync().Result;
                        JsonSerializerOptions options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        LoginViewModel loginViewModel = JsonSerializer.Deserialize<LoginViewModel>(result, options); //העברת פורמט מגייסון לאובייקט הספציפי
                        return loginViewModel;
                    }
                    return null;
                }
            }
        }
    }
}
