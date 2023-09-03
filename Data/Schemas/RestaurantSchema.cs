using Mongo.Api.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantCrudMongoDb.Data.Schemas
{
    public class RestaurantSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public EKitchen Kitchen { get; set; }

        public AddressSchema Address { get; set; }
    }
}
