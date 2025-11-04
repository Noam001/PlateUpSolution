using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PlateUpWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController : ControllerBase
    {


        [HttpGet]
        public MenuViewModel GetMenu()
        {
            OledbContext oledbcontext = new OledbContext();
            ModelFactory modelFactory = new ModelFactory();
            oledbcontext.OpenConnection();
            RepositoryFactory repositoryFactory = new RepositoryFactory(oledbcontext, modelFactory);

            MenuViewModel menuViewModel = new MenuViewModel();
            menuViewModel.Meals = mealRepository.GetAll();
            menuViewModel.FoodTypes = foodTypeRepository.GetAll();
            oledbcontext.CloseConnection();
            return menuViewModel;
        }
    }
}
