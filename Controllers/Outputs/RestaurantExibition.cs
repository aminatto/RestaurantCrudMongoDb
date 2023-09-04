namespace RestaurantCrudMongoDb.Controllers.Outputs
{
    public class RestaurantExibition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Kitchen { get; set; }

        public AddressExibition Address { get; set; }
    }
}
