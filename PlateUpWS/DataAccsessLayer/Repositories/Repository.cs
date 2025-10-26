using System.Diagnostics.Contracts;

namespace PlateUpWS
{
    public abstract class Repository
    {
        protected IDBContext dbContext;
        protected ModelFactory modelFactory;

        public Repository(IDBContext dbContext, ModelFactory modelFactory)
        {
            this.dbContext = dbContext;
            this.modelFactory = modelFactory;
        }
    }
}
