using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PlateUpWS
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        RepositoryFactory repositoryFactory;

        public AdminController()
        {
            this.repositoryFactory = new RepositoryFactory();
        }
    }
}
