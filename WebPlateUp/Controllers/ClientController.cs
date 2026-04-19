using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol.Core.Types;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApiClient;
using WebAppPlateUp.Models;
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
            WebClient<LoginViewModel> client = new WebClient<LoginViewModel>();
            LoginViewModel loginViewModel = client.Login(loginModel);

            // 4. בדיקת התוצאה
            if (loginViewModel != null)
            {
                // הצלחה- שומרים בסשן ועוברים לדף הבית
                HttpContext.Session.SetString("clientId", loginViewModel.ClientId);
                HttpContext.Session.SetString("clientName", loginViewModel.Name);
                TempData["SuccessMessage"] = "Successfully logged in!";
                return RedirectToAction("HomePage", "Guest");
            }
            else
            {
                // כישלון - נחזור לדף ההתחברות עם הודעת שגיאה
                TempData["ErrorMessage"] = "Invalid email or password.";
                return View("ViewLogin");
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Successfully logged out.";
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
            if (ok)
            { //האם שליחה למסד נתונים עבדה
                TempData["SuccessMessage"] = "Review submitted successfully! Thank you for your feedback.";
                return Redirect(Url.Action("HomePage", "Guest") + "#reviews-section");
            }                
            else
                TempData["ErrorMessage"] = "Failed to submit review. Please try again later..";
            return View("HomePage", "Guest");
        }
        [HttpGet]
        public IActionResult DeleteReview(string reviewId)
        {
            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/RemoveReview";
            client.AddParameter("reviewID", reviewId);
            bool success = client.Get();
            if (success)
                TempData["SuccessMessage"] = "Review deleted successfully.";      
            else
                TempData["ErrorMessage"] = "Failed to delete the review. Please try again.";
            return RedirectToAction("HomePage", "Guest");
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
        public IActionResult MakeAReservation()
        {
            Order order = new Order();
            order.ClientId =  HttpContext.Session.GetString("clientId");
            order.OrderDate = HttpContext.Session.GetString("reservationDate");
            order.OrderTime = HttpContext.Session.GetString("reservationTime");
            order.NumOfPeople = int.Parse(HttpContext.Session.GetString("reservationPeople"));
            order.OrderStatus = true;
            order.OrderPlace = HttpContext.Session.GetString("reservationPlace");

            WebClient<Order> client = new WebClient<Order>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/MakeAnOrder";
            bool ok = client.Post(order);

            if (ok)
            {
                TempData["SuccessMessage"] = "Your reservation is confirmed!";
                return RedirectToAction("HomePage", "Guest");
            }
            TempData["ErrorMessage"] = "Reservation failed. Please try again.";
            return RedirectToAction("CheckoutView");
        }
        [HttpPost]
        public IActionResult AddMealToOrder(int mealId,int quantity, string? mealNotes, string clientId)
        {
            WebClient<AddMealRequest> client = new WebClient<AddMealRequest>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/AddMealToOrder";
            AddMealRequest addMeal = new AddMealRequest();
            addMeal.MealId = mealId;
            addMeal.Quantity = quantity;
            addMeal.MealNotes = mealNotes;
            addMeal.ClientId = HttpContext.Session.GetString("clientId");
            bool ok = client.Post(addMeal);
            if (ok)
                TempData["SuccessMessage"] = "Successfully added to cart!";
            else
                TempData["ErrorMessage"] = "Failed to add meal.";
            return RedirectToAction("MealDetails", "Guest", new { id = mealId });
        }
        [HttpGet]
        public IActionResult RemoveMealFromOrder(string mealId, string orderId)
        {
            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/RemoveMealFromOrder";
            client.AddParameter("mealId", mealId);
            client.AddParameter("orderId", orderId);
            bool success = client.Get();
            if (!success)
                TempData["Message"] = "Failed to remove item.";
            return RedirectToAction("Cart");
        }
        [HttpGet]
        public IActionResult CheckoutView(string orderId, string totalPrice)
        {
            ViewBag.OrderId = orderId;
            ViewBag.TotalPrice = totalPrice;
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(string orderId)
        {
            WebClient<string> client = new WebClient<string>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/CheckoutUpdateStatus";
            bool ok = client.Post(orderId);

            if (ok)
            {
                TempData["SuccessMessage"] = "Your order is on its way!";
                return RedirectToAction("HomePage", "Guest");
            }
            else
            {
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                return RedirectToAction("Checkout", new { orderId = orderId, totalPrice = TempData["TotalPrice"] });
            }
        }//checkout for Home Delivery
        [HttpGet]
        public IActionResult ViewCheckoutReservation(Order order)
        {
            if (!ModelState.IsValid)
                return View("ViewTableReservation", order);

            HttpContext.Session.SetString("reservationDate", DateTime.Parse(order.OrderDate).ToString("dd/MM/yyyy"));
            HttpContext.Session.SetString("reservationTime", order.OrderTime.ToString());
            HttpContext.Session.SetString("reservationPeople", order.NumOfPeople.ToString());
            HttpContext.Session.SetString("reservationPlace", order.OrderPlace);

            return RedirectToAction("CheckoutView");
        }
        [HttpGet]
        public IActionResult UpdateQuantity(int mealId, int orderId, int quantity)
        {
            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/UpdateQuantity";
            client.AddParameter("mealId", mealId.ToString());
            client.AddParameter("orderId", orderId.ToString());
            client.AddParameter("quantity", quantity.ToString());
            bool success = client.Get();
            if (success)
                TempData["SuccessMessage"] = "Quantity updated successfully!";
            else
                TempData["ErrorMessage"] = "Failed to update quantity. Please try again.";

            return RedirectToAction("Cart");
        }
        [HttpGet]
        public IActionResult Cart(string clientId)
        {
            WebClient<CartViewModel> client = new WebClient<CartViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Client/GetCart";
            clientId = HttpContext.Session.GetString("clientId");
            client.AddParameter("clientId", clientId);
            CartViewModel vm = client.Get();
            return View(vm);
        }

        //פעולת עזר שמביאה משירות רשת חיצוני את פרטי מזג אוויר
        [HttpGet]
        public async Task<IActionResult> GetWeather(string date, string time)
        {
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
                return Json(null);
            try
            { 
                string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Ashkelon,Israel/{date}T{time}:00?key=YXLRG4K97Z69YFDVRLKP9GNUS&unitGroup=metric&include=hours";

                using HttpClient client = new HttpClient();
                string json = await client.GetStringAsync(url);

                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement firstDay = doc.RootElement.GetProperty("days")[0];
                JsonElement hours = firstDay.GetProperty("hours");

                string targetHour = time + ":00";
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                foreach (JsonElement hour in hours.EnumerateArray())
                {
                    if (hour.GetProperty("datetime").GetString() == targetHour)
                    {
                        Hour weatherData = JsonSerializer.Deserialize<Hour>(hour.GetRawText(), options);                  
                        return Json(weatherData);
                    }
                }
            }
            catch
            {
                return Json(null);
            }
            return Json(null);
        }

    }
}
