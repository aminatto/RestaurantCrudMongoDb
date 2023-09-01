using Mongo.Api.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RestaurantCrudMongoDb.Data.Schemas;

namespace RestaurantCrudMongoDb.Data
{
    public class MongoDb
    {
        public IMongoDatabase DB { get; }

        public MongoDb(IConfiguration configuration)
        {
            try
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));
                var client = new MongoClient(settings);
                DB = client.GetDatabase(configuration["DbName"]);
                MapClasses();
            }
            catch (Exception e)
            {

                throw new MongoException("Unable to connect to MongoDB", e);
            }
            
        }

        private void MapClasses()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(RestaurantSchema)))
            {
                BsonClassMap.RegisterClassMap<RestaurantSchema>(i =>
                {
                    i.AutoMap();
                    i.MapIdMember(k => k.Id);
                    i.MapMember(k => k.Kitchen).SetSerializer(new EnumSerializer<EKitchen>(BsonType.Int32));
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}
