using Mongo.Api.Domain.Entities;
using Mongo.Api.Domain.Enums;
using Mongo.Api.Domain.ValueObjects;
using MongoDB.Driver;
using RestaurantCrudMongoDb.Data.Schemas;
using System.Linq.Expressions;

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

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            var restaurants = new List<Restaurant>();
            var filter = Builders<RestaurantSchema>.Filter.Empty;
            await _restaurants.Find(filter).ForEachAsync(d =>
            {
                var restaurant = new Restaurant(d.Id, d.Name, d.Kitchen);
                var address = new Address(d.Address.PuplicPlace, d.Address.Number, d.Address.City, d.Address.State, d.Address.PostalCode);
                restaurant.AssignAddress(address);
                restaurants.Add(restaurant);
            });

            return restaurants;
        }

        public Restaurant GetById(string id)
        {
            var restaurant = _restaurants.AsQueryable().FirstOrDefault(x => x.Id == id);

            if (restaurant == null)
                return null;

            return restaurant.ConvertToDomain();
        }

        public IEnumerable<Restaurant> GetByName(string name)
        {
            var restaurants = new List<Restaurant>();

            _restaurants.AsQueryable()
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .ToList()
            .ForEach(d => restaurants.Add(d.ConvertToDomain()));

            if (restaurants == null)
                return null;

            return restaurants;

        }

        public bool EditWholeDocument(Restaurant restaurant)
        {
            var document = new RestaurantSchema
            {
                Id = restaurant.Id,
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

            var result = _restaurants.ReplaceOne(x => x.Id == document.Id, document);

            return result.ModifiedCount > 0;
        }

        //método genérico pra editar entidades.
        public bool EditEntity<T>(string id, T entity, Expression<Func<RestaurantSchema, T>> propertySelector)
        {
            var update = Builders<RestaurantSchema>.Update.Set(propertySelector, entity);

            var result = _restaurants.UpdateOne(x => x.Id == id, update);

            return result.ModifiedCount > 0;
        }


        public bool EditKitchen(string id, EKitchen kitchen)
        {
            var update = Builders<RestaurantSchema>.Update.Set(x => x.Kitchen, kitchen);

            var result = _restaurants.UpdateOne(x => x.Id == id, update);

            return result.ModifiedCount > 0;
        }

    }
}
