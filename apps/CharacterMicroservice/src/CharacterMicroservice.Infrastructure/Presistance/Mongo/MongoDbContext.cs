using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo;

public class MongoDbContext
{

    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        _database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
        => _database.GetCollection<T>(name);

}
