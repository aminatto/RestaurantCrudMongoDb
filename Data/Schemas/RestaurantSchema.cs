using Mongo.Api.Domain.Enums;
using MongoDB.Bson;

namespace RestaurantCrudMongoDb.Data.Schemas
{
    public class RestaurantSchema
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public EKitchen Kitchen { get; set; }

        public AddressSchema Address { get; set; }
    }
}
