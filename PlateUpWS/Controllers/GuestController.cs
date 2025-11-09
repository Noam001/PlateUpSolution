using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PlateUpWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        RepositoryFactory repositoryFactory;

        public GuestController()
        {
            this.repositoryFactory = new RepositoryFactory();
        }
        [HttpGet]
        public MenuViewModel GetMenu()
        {
            OledbContext oledbcontext = new OledbContext();
            
            oledbcontext.OpenConnection();
            RepositoryFactory repositoryFactory = new RepositoryFactory();

            MenuViewModel menuViewModel = new MenuViewModel();
            menuViewModel.Meals = repositoryFactory.MealRepository.GetAll();
            menuViewModel.FoodTypes = repositoryFactory.FoodTypeRepository.GetAll();
            oledbcontext.CloseConnection();
            return menuViewModel;
        }

    }
}
