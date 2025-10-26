using System.Diagnostics.Contracts;

namespace PlateUpWS
{
    public abstract class Repository
    {
        protected OledbContext dbContext;
        protected ModelFactory modelFactory;

        public Repository(OledbContext dbContext, ModelFactory modelFactory)
        {
            this.dbContext = dbContext;
            this.modelFactory = modelFactory;
        }
    }
}
