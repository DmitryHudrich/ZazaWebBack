using System.Text.Json.Serialization;

namespace ZazaNotes;

public static class UserController {
    private static List<User> users = [];
    public static IReadOnlyList<User> Users => users;

    public static User? Add(User user) {
        foreach (var usr in UserController.Users) {
            if (usr.Login == user.Login) {
                return null;
            }
        }
        users.Add(user);
        return user;
    }

    public static User? Get(Guid? Id) => users.FirstOrDefault(obj => obj.Id == Id);
    public static Guid? Find(string login) => users.Find(obj => obj.Login == login)?.Id;
    public static bool Remove(User user) => users.Remove(user);
    public static void Remove(Guid old) => users.ChangeById(old);
    public static void Change(Guid old, User young) => users.ChangeById(old, young);
}

public record class User(Guid Id, string Name, string Login, Password Password) : IIdentifiable {
    public List<Note> Notes { get; set; } = [];
}

public class Password {
    [JsonConstructor]
    public Password(string password) {
        Hash = password.CreateSHA256();
    }
    public string Hash { get; }
}