using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
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
        [HttpGet]
        public ReportsViewModel GetReports(string fromDate, string toDate)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                ReportsViewModel reportsViewModel = new ReportsViewModel();
                reportsViewModel.Top3MostOrderedMeals = this.repositoryFactory.MealRepository.GetTop3MostOrdered();
                reportsViewModel.Top3LeastOrderedMeals = this.repositoryFactory.MealRepository.GetTop3LeastOrderedMeals();
                reportsViewModel.TotalOrdersInDateRange = this.repositoryFactory.OrderRepository.GetTotalOrdersInDateRange(fromDate, toDate);
                reportsViewModel.TotalIncomeInDateRange = this.repositoryFactory.OrderRepository.GetTotalIncomeInDateRange(fromDate, toDate);
                return reportsViewModel;
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
        //ניהול סוגי מזון- מחיקה, הוספה ועדכון
        [HttpGet]
        public List<FoodType> GetFoodTypes()
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.FoodTypeRepository.GetAll();
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
        public bool AddFoodType(FoodType foodType)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.FoodTypeRepository.Create(foodType);
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
        [HttpPost]
        public bool UpdateFoodType(FoodType foodType)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.FoodTypeRepository.Update(foodType);
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
        [HttpGet]
        public bool RemoveFoodType(string foodTypeId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.FoodTypeRepository.Delete(foodTypeId);
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

        //ניהול סוגי מנות- מחיקה, הוספה ועדכון
        [HttpGet]
        public ManageMenuViewModel GetManageMenuViewModel(string foodTypeId = "-1", string mealNameSearch = "")
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                ManageMenuViewModel gmmvm = new ManageMenuViewModel();
                gmmvm.FoodTypes = repositoryFactory.FoodTypeRepository.GetAll();

                // מקרה 1: לא נבחר שום סינון
                if (foodTypeId == "-1" && mealNameSearch =="")
                    gmmvm.Meals = this.repositoryFactory.MealRepository.GetAll();

                // --- מקרה 2: חיפוש לפי שם מנה ---
                if (foodTypeId == "-1" && mealNameSearch != "")
                {
                    Meal meal = repositoryFactory.MealRepository.GetMealByName(mealNameSearch);
                    gmmvm.Meals = new List<Meal>() { meal };
                }

                // --- מקרה 3: חיפוש לפי סוג מזון ---
                else if (foodTypeId != "-1" && mealNameSearch == "")
                {
                    gmmvm.Meals = repositoryFactory.MealRepository.GetMealsByFoodType(foodTypeId);
                }
                return gmmvm;
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
        public bool AddMeal(Meal meal)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.MealRepository.Create(meal);
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
        [HttpPost]
        public bool UpdateMeal(Meal meal)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.MealRepository.Update(meal);
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
        [HttpGet]
        public bool RemoveMeal(string mealId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.MealRepository.Delete(mealId);
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

        //ניהול ערים- מחיקה, הוספה ועדכון
        [HttpPost]
        public bool AddCity(City city)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.CityRepository.Create(city);
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

        [HttpPost]
        public bool UpdateCity(City city)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.CityRepository.Update(city);
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
        [HttpGet]
        public bool RemoveCity(string cityId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.CityRepository.Delete(cityId);
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

        //ניהול הזמנות- צפייה בהזמנה, עדכון סטטוס ומחיקה
        [HttpGet]
        public ManageOrdersViewModel GetManageOrdersViewModel(int orderID = 0, bool? orderStatus = null)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                ManageOrdersViewModel movm = new ManageOrdersViewModel();
                movm.OrderStatus = orderStatus;
                movm.OrderID = orderID;
                // מקרה 1: לא נבחר שום סינון
                if (orderStatus == null && orderID == 0)
                    movm.Orders = this.repositoryFactory.OrderRepository.GetAll();

                // מקרה 2: חיפוש לפי תעודת זהות של הזמנה
                if (orderStatus == null && orderID > 0)
                {
                    Order order = this.repositoryFactory.OrderRepository.GetById(orderID);
                    movm.Orders = new List<Order>() { order };
                }
                // מקרה 3: חיפוש לפי סטטוס הזמנה- שולם\לא שולם
                if (orderStatus != null && orderID == 0)
                {
                    movm.Orders = this.repositoryFactory.OrderRepository.GetOrdersByStatus(orderStatus);
                }
                return movm;
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
        public bool UpdateOrderStatus(int orderId, bool orderStatus)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.UpdateOrderStatus(orderId, orderStatus);
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
        [HttpGet]
        public Order ViewOrder(int orderID)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.GetById(orderID);
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
        public bool RemoveOrder(string orderID)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.Delete(orderID);
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

        // מחיקת ביקורת
        [HttpGet]
        public bool RemoveReview(string reviewID)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.ReviewRepository.Delete(reviewID);
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
