using Models;
using System.Data;
namespace PlateUpWS
{
    public class FoodTypeCreator : IModelCreator<FoodType>
    {
        public FoodType CreateModel(IDataReader reader)
        {
            return new FoodType
            {
                FoodTypeId = Convert.ToUInt16(reader["FoodTypeId"]),
                FoodTypeName = Convert.ToString(reader["FoodTypeName"])
            };
        }
    }
}
