using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PlateUpWS
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
        public List<Review> GetReviews()
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.ReviewRepository.GetAll();
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

        [HttpGet]
        public MenuViewModel GetMenu(string foodTypeId = "-1", int pageNumber = 0, string mealNameSearch = "", bool? priceSort =null, int pages = 0)
        {
            MenuViewModel menuViewModel = new MenuViewModel();
            int mealperPage = 8;
            menuViewModel.FoodTypeId = foodTypeId;
            menuViewModel.PageNumber = pageNumber;
            menuViewModel.MealNameSearch = mealNameSearch;
            menuViewModel.Pages = pages;
            try//נסה לעשות את פקודות אלו
            {
                this.repositoryFactory.ConnectDb();

                // מקרה 1: לא נבחר שום סינון
                if (foodTypeId == "-1" && pageNumber == 0 && mealNameSearch == "" && priceSort == null)
                    menuViewModel.Meals = repositoryFactory.MealRepository.GetAll();

                // --- מקרה 2: רק עמוד נבחר ---
                else if (foodTypeId == "-1" && pageNumber > 0 && mealNameSearch == "" && priceSort == null)
                {
                    menuViewModel.Meals = repositoryFactory.MealRepository.FilterByPage(pageNumber, mealperPage);
                }

                // ---  מקרה 3: נבחר גם סוג אוכל וסינון לפי מחיר ---
                else if (foodTypeId != "-1" && pageNumber == 0 && mealNameSearch == "" && priceSort != null)
                {
                    menuViewModel.Meals = repositoryFactory.MealRepository.SortByPriceFoodType(foodTypeId, priceSort);
                }

                // --- מקרה 4: רק סוג אוכל נבחר ---
                else if (foodTypeId != "-1" && pageNumber == 0 && mealNameSearch == "" && priceSort == null)
                {
                    menuViewModel.Meals = repositoryFactory.MealRepository.GetMealsByFoodType(foodTypeId);
                }

                // --- מקרה 5: חיפוש לפי שם מנה ---
                else if (mealNameSearch != "")
                {
                    Meal meal = repositoryFactory.MealRepository.GetMealByName(mealNameSearch);
                    menuViewModel.Meals = new List<Meal>() { meal };                
                }

                // ---  מקרה 7: מיון לפי מחיר ועמוד נבחר ---
                else if (foodTypeId == "-1" && pageNumber > 0 && mealNameSearch == "" && priceSort != null)
                {
                    menuViewModel.Meals = repositoryFactory.MealRepository.SortByPriceFilterByPage(pageNumber, mealperPage, priceSort);
                }
                // ---  מקרה 6: מיון לפי מחיר ---
                else if (priceSort != null) 
                {
                    menuViewModel.Meals = repositoryFactory.MealRepository.SortByPrice(priceSort);
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

        [HttpPost]
        public bool Registration(Client client)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.ClientRepository.Create(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }


    }
}
