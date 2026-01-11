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
        public IActionResult ViewLogin()
        {
            return View();
        }

    }
}
