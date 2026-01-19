using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol.Core.Types;
using System.Net;
using System.Text.Json;
using System.Text;
using WebApiClient;

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
        [HttpPost]
        public IActionResult UpdateProfile(Client client)
        {
            WebClient<Client> webClient = new WebClient<Client>();
            webClient.Schema = "http";
            webClient.Host = "localhost";
            webClient.Port = 5035;
            webClient.Path = "api/Client/UpdateProfile";
            bool ok = webClient.Post(client);
            if (ok) //בדיקת תקינות הקלט
                return RedirectToAction("HomePage", "Guest");
            TempData["client"] = client;
            ViewBag.Error = true;
            return RedirectToAction("ViewRegistration", "Guest");
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
