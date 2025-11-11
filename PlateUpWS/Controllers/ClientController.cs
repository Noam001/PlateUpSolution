using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        

    }
}
