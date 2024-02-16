using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using ZazaNotes;

var builder = WebApplication.CreateBuilder();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder => {
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
});

app.UseStaticFiles();

app.MapGet("api/users/login={login}", handler: (string login) => Results.Json(UserController.Get(UserController.Find(login))));
app.MapGet("api/users/id={id:guid}", handler: (string id) => Results.Json(UserController.Get(Guid.Parse(id))));
app.MapGet("api/users", () => Results.Json(UserController.Users));
app.MapPost("api/users", (UserInfo user) => UserController.Add(user.User));
app.MapPut("api/users/id={id:guid}", (UserInfo user, Guid id) => UserController.Change(id, new User(id, user.Name, user.Login, new Password(user.Password))));
app.MapDelete("api/users/id={id:guid}", (Guid id) => UserController.Remove(id));

app.Run();

internal record UserInfo {
    public required string Name { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }

    public User User => new User(Guid.NewGuid(), Name, Login, new Password(Password));
}