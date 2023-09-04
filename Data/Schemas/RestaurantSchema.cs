using Mongo.Api.Domain.Entities;
using Mongo.Api.Domain.Enums;
using Mongo.Api.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.CompilerServices;

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

    public static class RestaurantSchemaExtension
    {
        public static Restaurant ConvertToDomain(this RestaurantSchema document)
        {
            var restaurant = new Restaurant(document.Id, document.Name, document.Kitchen);
            var address = new Address(document.Address.PuplicPlace, document.Address.Number, document.Address.City, document.Address.State, document.Address.PostalCode);
            restaurant.AssignAddress(address);

            return restaurant;
        }

    }
}
