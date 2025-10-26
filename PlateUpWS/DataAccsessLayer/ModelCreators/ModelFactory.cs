namespace PlateUpWS
{
    public class ModelFactory
    {
        CityCreator cityCreator;
        ClientCreator clientCreator;
        FoodTypeCreator foodTypeCreator;
        MealCreator mealCreator;
        OrderCreator orderCreator;
        ReviewCreator reviewCreator;

        public CityCreator CityCreator
        {
            get
            {
                if (this.cityCreator == null) 
                    this.cityCreator = new CityCreator();
                return this.cityCreator;
            }
        }
        public ClientCreator ClientCreator
        {
            get
            {
                if(this.clientCreator == null)
                    this.clientCreator = new ClientCreator();
                return this.clientCreator;
            }
        }
        public FoodTypeCreator FoodTypeCreator
        {
            get
            {
                if (this.foodTypeCreator == null)
                    this.foodTypeCreator = new FoodTypeCreator();
                return this.foodTypeCreator;
            }
        }
        public MealCreator MealCreator
        {
            get
            {
                if( this.mealCreator == null)
                    this.mealCreator = new MealCreator();
                return this.mealCreator;
            }
        }
        public OrderCreator OrderCreator
        {
            get
            {
                if(this.orderCreator == null)
                    this.orderCreator = new OrderCreator();
                return this.orderCreator;
            }
        }
        public ReviewCreator ReviewCreator
        {
            get
            {
                if(this.reviewCreator == null)
                    this.reviewCreator = new ReviewCreator();
                return this.reviewCreator;
            }
        }
    }
}
