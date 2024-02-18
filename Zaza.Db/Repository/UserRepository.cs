using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using Zaza.Notes.Entities;

namespace Zaza.Db.Repository;

public class UserRepository(ZazaContext context) {
    public async Task<List<User>> GetUserAsync() => await context.Users.FindAsync<User>(new BsonDocument()).Result.ToListAsync();
    public async Task<List<User>> GetUserByLoginAsync(string login) => await context.Users.FindAsync<User>(new BsonDocument { {"Login", login } }).Result.ToListAsync();
    public async Task<List<User>> GetUserByIdAsync(Guid id) => await context.Users.FindAsync<User>(new BsonDocument { {"_id", new BsonBinaryData(id, GuidRepresentation.Standard) } }).Result.ToListAsync();

    public async Task InsertUserAsync(User person) => await context.Users.InsertOneAsync(person);

    public async Task ChangeUserAsync(Guid old, User newUser) => await context.Users.FindOneAndReplaceAsync<User>(new BsonDocument { { "_id", new BsonBinaryData(old, GuidRepresentation.Standard) } }, newUser );
    public async Task DeleteUserAsync(Guid old) => await context.Users.FindOneAndDeleteAsync<User>(new BsonDocument { { "_id", new BsonBinaryData(old, GuidRepresentation.Standard)} });
}
