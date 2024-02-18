using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using Zaza.Notes.Entities;

namespace Zaza.Db;

public class ZazaContext {
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<User> _usersCollection;

    public IMongoCollection<User> Users => _usersCollection;

    public ZazaContext(IConfiguration configuration) {
        string connectionString = configuration.GetConnectionString("MongoString") ??
            throw new ArgumentNullException(nameof(connectionString));
        string databaseName = configuration.GetSection("MongoName").Value ??
            throw new ArgumentNullException(nameof(databaseName));
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
        _usersCollection = _database.GetCollection<User>("users");
    }
}

