using Zaza.Db.Repository;
using Zaza.Notes;
using Zaza.Notes.Controllers;
using Zaza.Notes.Entities;

namespace Zaza.Web;

public class Responses(UserRepository repos) {
    public void UserRegister(WebApplication app) {
        // юзеры
        app.MapGet("api/users/login={login}", handler: async (string login) => Results.Json(await repos.GetUserByLoginAsync(login)));
        app.MapGet("api/users/id={id:guid}", handler: async (Guid id) => Results.Json(await repos.GetUserByIdAsync(id)));
        app.MapGet("api/users", async () => Results.Json(await repos.GetUserAsync()));
        app.MapPost("api/users", async (UserInfo user) => await repos.InsertUserAsync(user.User));
        app.MapPut("api/users/id={id:guid}", async (UserInfo user, Guid id) => await repos.ChangeUserAsync(id,
            new User(id, user.Name, user.Login, new Password(user.Password))));
        app.MapDelete("api/users/id={id:guid}", async (Guid id) => await repos.DeleteUserAsync(id));

    }
}
