using Models;
using System.Data;
namespace PlateUpWS
{
    public class MealCreator : IModelCreator<Meal>
    {
        public Meal CreateModel(IDataReader reader)
        {
            return new Meal
            {
                MealId = Convert.ToUInt16(reader["MealId"]),
                MealName = Convert.ToString(reader["MealName"]),
                MealPhoto = Convert.ToString(reader["MealPhoto"]),
                MealDescription = Convert.ToString(reader["MealDescription"]),
                MealPrice = Convert.ToDouble(reader["MealPrice"]),
                MealStatus = Convert.ToString(reader["MealStatus"])
            };
        }
    }
}
