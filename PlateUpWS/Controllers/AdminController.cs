using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Models;
using Models.ViewModels;
using System.Text.Json;
using System.IO;

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
        public ReportsViewModel GetReports(string? fromDate=null, string? toDate=null)
        {
            try
               
            {
                if (fromDate==null && toDate==null)
                {
                    fromDate = DateTime.Now.AddDays(-1).ToShortDateString();
                    toDate = DateTime.Now.AddDays(-1).ToShortDateString(); ;
                }
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


        [HttpGet]
        public OrderReport GetOrderReport(string? fromDate , string? toDate )
        {
            try

            {
              
                this.repositoryFactory.ConnectDb();
                OrderReport reportsViewModel = new OrderReport();
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
        [HttpGet]
        public FoodType GetFoodTypeByMealId(int mealId)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.FoodTypeRepository.GetFTByMealId(mealId);
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

        //ניהול סוגי מנות- מחיקה, הוספה ועדכון
        [HttpGet]
        public MealViewModel GetAddMealViewModel()
        {
            MealViewModel addMealView = new MealViewModel();
            addMealView.Meal = null;
            try
            {
                this.repositoryFactory.ConnectDb();
                addMealView.FoodTypes = this.repositoryFactory.FoodTypeRepository.GetAll();
                return addMealView;
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
        public ManageMenuViewModel GetManageMenuViewModel(string foodTypeId = "-1")
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                ManageMenuViewModel gmmvm = new ManageMenuViewModel();
                gmmvm.FoodTypes = repositoryFactory.FoodTypeRepository.GetAll();

                // מקרה 1: לא נבחר שום סינון
                if (foodTypeId == "-1")
                    gmmvm.Meals = this.repositoryFactory.MealRepository.GetAll();
           
                else
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
        public bool AddMeal()
        {
            string jsonString = Request.Form["data"];
            MealViewModel meal = JsonSerializer.Deserialize<MealViewModel>(jsonString);
            IFormFile file = Request.Form.Files[0];
            try
            {
                this.repositoryFactory.ConnectDb();
                this.repositoryFactory.Opentransaction();

                this.repositoryFactory.MealRepository.Create(meal.Meal);

                int mealId = Convert.ToUInt16(this.repositoryFactory.GetLastInsertedId());
                this.repositoryFactory.MealRepository.AddFoodTypeToMeal(mealId, meal.FoodTypes.First().FoodTypeId);

                //Image saving logic here
                this.repositoryFactory.MealRepository.UpdateMealPhoto(mealId, mealId + meal.Meal.MealPhoto);
                using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DataImages", 
                                                                           mealId + meal.Meal.MealPhoto), FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                this.repositoryFactory.Commit();
                return true;
            }
            catch (Exception ex)
            {
                this.repositoryFactory.RollBack();
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpPost]
        public bool UpdateMeal()
        {
            string jsonString = Request.Form["data"];
            MealViewModel meal = JsonSerializer.Deserialize<MealViewModel>(jsonString);    //MealPhoto = format.     
            try
            {
                this.repositoryFactory.ConnectDb();
                this.repositoryFactory.Opentransaction();

                string oldPhoto = this.repositoryFactory.MealRepository.GetMealPhotoById(meal.Meal.MealId);
                this.repositoryFactory.MealRepository.Update(meal.Meal); //מעדכן את המנה מבלי לעדכן את התמונה
                this.repositoryFactory.MealRepository.UpdateFoodTypeMeal(meal.Meal.MealId, meal.FoodTypes.First().FoodTypeId);
                //Image saving logic here              
                if (meal.Meal.MealPhoto != null)//נבחר תמונה וצריך לעדכן
                {
                    // 1. עדכון שם הקובץ החדש במסד הנתונים
                    string newFileName = meal.Meal.MealId + meal.Meal.MealPhoto;
                    this.repositoryFactory.MealRepository.UpdateMealPhoto(meal.Meal.MealId, newFileName);

                    // 2. נתיבים
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DataImages", oldPhoto);
                    string newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DataImages", newFileName);

                    // 3. שמירת הקובץ החדש (FileMode.Create דורס אוטומטית)
                    IFormFile file = Request.Form.Files[0];
                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // 4. מחיקת הקובץ הישן רק אם השם(הפורמט) באמת השתנה
                    if (oldPhoto != newFileName && System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                this.repositoryFactory.Commit();
                return true;
            }
            catch (Exception ex)
            {
                this.repositoryFactory.RollBack();
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }
        [HttpGet]
        public bool RemoveMeal(string mealId, string mealPhoto)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                this.repositoryFactory.Opentransaction();
                this.repositoryFactory.MealRepository.DeleteFoodTypeMeal(mealId);
                this.repositoryFactory.MealRepository.Delete(mealId);
                this.repositoryFactory.Commit();
                // מחיקת הקובץ רק אחרי שהמסד נתונים הצליח
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DataImages", mealPhoto);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                return true;
            }
            catch (Exception ex)
            {
                this.repositoryFactory.RollBack();
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.repositoryFactory.DisconnectDb();
            }
        }

        //ניהול ערים- מחיקה, הוספה ועדכון
        [HttpGet]
        public List<City> GetCities()
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.CityRepository.GetAll();
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
                    Order order = this.repositoryFactory.OrderRepository.GetById(orderID.ToString());
                    if(order == null)
                        movm.Orders = this.repositoryFactory.OrderRepository.GetAll();
                    else
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
        [HttpGet]
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
        public CartViewModel ViewOrderedMeals(string orderID)
        {
            try
            {
                this.repositoryFactory.ConnectDb();
                return this.repositoryFactory.OrderRepository.GetCart("",orderID);
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
                this.repositoryFactory.Opentransaction();
                this.repositoryFactory.OrderRepository.DeleteFromMealsOrders(orderID);
                this.repositoryFactory.OrderRepository.Delete(orderID);
                this.repositoryFactory.Commit();
                return true;
            }
            catch (Exception ex)
            {
                this.repositoryFactory.RollBack();
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
