using Models;
using System.Collections.Generic;
using System.Data;
namespace PlateUpWS
{
    public class ReviewRepository : Repository, IRepository<Review>
    {
        public ReviewRepository(OledbContext dbContext, ModelFactory modelFactory)
            : base(dbContext, modelFactory)
        {
        }

        public bool Create(Review item)
        {
            string sql = @$"
                INSERT INTO Reviews(ClientId, ReviewDate, ReviewComment, ReviewRating)
                VALUES
                (
                    @ClientId, @ReviewDate, @ReviewComment, @ReviewRating
                )";
            this.dbContext.AddParameter("@ClientId", item.ClientId);
            this.dbContext.AddParameter("@ReviewDate", item.ReviewDate);
            this.dbContext.AddParameter("@ReviewComment", item.ReviewComment);
            this.dbContext.AddParameter("@ReviewRating", item.ReviewRating);

            return this.dbContext.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"DELETE FROM Reviews WHERE ReviewId = @ReviewId";
            this.dbContext.AddParameter("@ReviewId", id);
            return this.dbContext.Delete(sql) > 0;
        }

        public List<Review> GetAll()
        {
            List<Review> reviews = new List<Review>();
            string sql = @"SELECT * FROM Reviews";
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                while (reader.Read())
                {
                    reader.Read();
                    reviews.Add(this.modelFactory.ReviewCreator.CreateModel(reader));
                }
            }
            return reviews;
        }

        public Review GetById(int id)
        {
            string sql = @"SELECT * FROM Reviews WHERE ReviewId = @ReviewId";
            this.dbContext.AddParameter("@ReviewId", id);
            using (IDataReader reader = this.dbContext.Select(sql))
            {
                return this.modelFactory.ReviewCreator.CreateModel(reader);
            }
        }

        public bool Update(Review item)
        {
            string sql = @$"
                UPDATE Reviews
                SET 
                    ClientId = @ClientId,
                    ReviewDate = @ReviewDate,
                    ReviewComment = @ReviewComment,
                    ReviewRating = @ReviewRating
                WHERE 
                    ReviewId = @ReviewId";

            this.dbContext.AddParameter("@ReviewId", item.ReviewId);
            this.dbContext.AddParameter("@ClientId", item.ClientId);
            this.dbContext.AddParameter("@ReviewDate", item.ReviewDate);
            this.dbContext.AddParameter("@ReviewComment", item.ReviewComment);
            this.dbContext.AddParameter("@ReviewRating", item.ReviewRating);

            return this.dbContext.Update(sql) > 0;
        }
    }
}
