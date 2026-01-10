using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol.Core.Types;
using System.Net;
using WebApiClient;

namespace WebPlateUp.Controllers
{
    public class ClientController : Controller
    {
        [HttpGet]
        public IActionResult LoginView()
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

    }
}
