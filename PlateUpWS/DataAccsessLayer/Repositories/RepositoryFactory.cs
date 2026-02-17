
namespace PlateUpWS
{
    public class RepositoryFactory
    {
        CityRepository cityRepository;
        ClientRepository clientRepository;
        FoodTypeRepository foodTypeRepository;
        MealRepository mealRepository;
        OrderRepository orderRepository;
        ReviewRepository reviewRepository;
        OledbContext dbContext;
        ModelFactory modelFactory;
        public RepositoryFactory()
        {
            this.dbContext = new OledbContext();
            this.modelFactory = new ModelFactory();
        }
        public CityRepository CityRepository
        {
            get
            {
                if (this.cityRepository == null)
                    this.cityRepository = new CityRepository(dbContext, modelFactory);
                return this.cityRepository;
            }
        }

        public ClientRepository ClientRepository
        {
            get
            {
                if (this.clientRepository == null)
                    this.clientRepository = new ClientRepository(dbContext, modelFactory);
                return this.clientRepository;
            }
        }

        public FoodTypeRepository FoodTypeRepository
        {
            get
            {
                if (this.foodTypeRepository == null)
                    this.foodTypeRepository = new FoodTypeRepository(dbContext, modelFactory);
                return this.foodTypeRepository;
            }
        }

        public MealRepository MealRepository
        {
            get
            {
                if (this.mealRepository == null)
                    this.mealRepository = new MealRepository(dbContext, modelFactory);
                return this.mealRepository;
            }
        }

        public OrderRepository OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                    this.orderRepository = new OrderRepository(dbContext, modelFactory);
                return this.orderRepository;
            }
        }

        public ReviewRepository ReviewRepository
        {
            get
            {
                if (this.reviewRepository == null)
                    this.reviewRepository = new ReviewRepository(dbContext, modelFactory);
                return this.reviewRepository;
            }
        }
        public void ConnectDb()
        {
            this.dbContext.OpenConnection();
        }
        public void DisconnectDb()
        {
            this.dbContext.CloseConnection();
        }
        public void Opentransaction()
        {
            this.dbContext.BeginTransaction();
        }

        public void Commit()
        {
            this.dbContext.Commit();    
        }

        public void RollBack()
        {
            this.dbContext.RollBack();
        }
    }
}
