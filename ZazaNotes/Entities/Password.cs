using MongoDB.Bson.Serialization.Attributes;

namespace Zaza.Notes.Entities;

public class Password {
    public Password(string password) {
        hash = password.CreateSHA256();
    }

    [BsonElement]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Добавить модификатор только для чтения", 
        Justification = "да че тут говорить) монга не может пароль сохранить если он ридонли, вот такие дела!")]
    private string? hash;
    public string? Hash => hash;
}