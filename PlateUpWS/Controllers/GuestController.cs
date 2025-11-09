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
        public MenuViewModel GetMenu(string foodTypeId="-1", int pageNumber=0, string mealNameSearch="")
        {
            MenuViewModel menuViewModel = new MenuViewModel();
            int mealperPage = 10;
            try//נסה לעשות את פקודות אלו
            {
                this.repositoryFactory.ConnectDb();
                if(foodTypeId=="-1" && pageNumber == 0 && mealNameSearch=="")
                    menuViewModel.Meals = repositoryFactory.MealRepository.GetAll();
                else if(foodTypeId == "-1" && pageNumber > 0 && mealNameSearch == "")
                {
                    menuViewModel.Meals = repositoryFactory.MealRepository.GetAll();
                    int first = (pageNumber - 1) * mealperPage;
                    int last = (pageNumber * mealperPage ) -1;
                    menuViewModel.Meals = menuViewModel.Meals.Skip(first).Take(last).ToList<Meal>();   
                }
                else if(foodTypeId != "-1" && pageNumber > 0 && mealNameSearch == "")
                {

                }
                    
                menuViewModel.FoodTypes = repositoryFactory.FoodTypeRepository.GetAll();
                return menuViewModel;
            }
            catch (Exception ex) //נכנס לפעולה רק במידה ויש שגיאה
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally //לא משנה אם הקוד הצליח או לא, נכנס לפעולה וסוגר את הקשר עם המסד נתונים
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpGet]
        public Meal GetMealDetails(int mealId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.MealRepository.GetById(mealId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }

    }
}
