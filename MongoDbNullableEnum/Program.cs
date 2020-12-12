using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDbNullableEnum
{
    class Program
    {
        static async Task Main(string[] args)
        {
            BsonClassMap.RegisterClassMap<Pet>(t => 
            {
                t.AutoMap();
                t.MapIdMember(x => x.Name);
                t.MapMember(x => x.Gender).SetSerializer(new NullableSerializer<Gender>(new EnumSerializer<Gender>(BsonType.String)));
            });

            var client = new MongoClient("mongodb://localhost:27017/?readPreference=primary");
            var database = client.GetDatabase("mongo-csharp-driver-nullable-enum");
            var collection = database.GetCollection<Pet>("pet");

            await collection.InsertOneAsync(new Pet { Name = "Cat", Gender = Gender.Male }, options: null);
            await collection.InsertOneAsync(new Pet { Name = "Chimera butterfly", Gender = null }, options: null);

            var cat = await collection.FindAsync(Builders<Pet>.Filter.Eq(x => x.Name, "Cat"), options: null);
        }
    }
}
