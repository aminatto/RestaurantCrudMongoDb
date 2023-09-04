using ThirdParty.BouncyCastle.Utilities.IO.Pem;

namespace RestaurantCrudMongoDb.Controllers.Inputs
{
    public class RestaurantWholeEdit
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Kitchen { get; set; }

        public string PublicPlace { get; set; }

        public string Number { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
    }
}
