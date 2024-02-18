using Microsoft.EntityFrameworkCore;
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

    public ZazaContext() {
        string connectionString = Environment.GetEnvironmentVariable("MONGODB_URL")!;
        string databaseName = Environment.GetEnvironmentVariable("MONGODB_NAME")!;
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
        _usersCollection = _database.GetCollection<User>("users");
    }
}

