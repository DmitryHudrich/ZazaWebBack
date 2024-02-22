using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Zaza.Notes.Entities;

public record class User(Guid Id, string Name, string Login, Password Password) : IIdentifiable
{
    public List<Note> Notes { get; set; } = [];
}
