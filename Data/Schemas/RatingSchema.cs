using Mongo.Api.Domain.Entities;
using Mongo.Api.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RestaurantCrudMongoDb.Domain.ValueObjects;

namespace RestaurantCrudMongoDb.Data.Schemas
{
    public class RatingSchema
    {
        public ObjectId Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string RestaurantId { get; set; }

        public int Stars { get; set; }

        public string Comment { get; set; }
    }

    public static class RatingSchemaExtension
    {
        public static Rating ConvertToDomain(this RatingSchema document)
        {
            return new Rating(document.Stars, document.Comment);
        }

    }
}
