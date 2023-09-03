using Mongo.Api.Domain.Entities;
using MongoDB.Driver;
using RestaurantCrudMongoDb.Data.Schemas;

namespace RestaurantCrudMongoDb.Data.Repositories
{
    public class RestaurantRepository
    {
        IMongoCollection<RestaurantSchema> _restaurants;

        public RestaurantRepository(MongoDb mongoDB)
        {
            _restaurants = mongoDB.DB.GetCollection<RestaurantSchema>("restaurant");
        }

        public void Insert(Restaurant restaurant)
        {
            var document = new RestaurantSchema
            {
                Name = restaurant.Name,
                Kitchen = restaurant.Kitchen,
                Address = new AddressSchema
                {
                    PuplicPlace = restaurant.Address.PublicPlace,
                    Number = restaurant.Address.Number,
                    City = restaurant.Address.City,
                    PostalCode = restaurant.Address.PostalCode,
                    State = restaurant.Address.State,
                }
            };

            _restaurants.InsertOne(document);
        }
    }
}
