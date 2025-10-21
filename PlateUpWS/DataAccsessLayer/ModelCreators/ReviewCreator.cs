using Models;
using System.Data;
namespace PlateUpWS
{
    public class ReviewCreator : IModelCreator<Review>
    {
        public Review CreateModel(IDataReader reader)
        {
            return new Review
            {
                ClientId = Convert.ToString(reader["ClientId"]),
                ReviewId = Convert.ToUInt16(reader["ReviewId"]),
                ReviewDate = Convert.ToString(reader["ReviewDate"]),
                ReviewComment = Convert.ToString(reader["ReviewComment"]),
                ReviewRating = Convert.ToUInt16(reader["ReviewRating"])
            };
        }
    }
}
