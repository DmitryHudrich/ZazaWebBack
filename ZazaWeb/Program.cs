using Zaza.Db;
using Zaza.Db.Repository;
using Zaza.Notes.Controllers;
using Zaza.Notes.Entities;
using Zaza.Web;

const string DB_STRING = "mongodb://localhost:27017";
const string DB_NAME = "NotesData";
Environment.SetEnvironmentVariable("MONGODB_NAME", DB_NAME);
Environment.SetEnvironmentVariable("MONGODB_URL", DB_STRING);

var builder = WebApplication.CreateBuilder();

builder.Services.AddCors();
builder.Services.AddSingleton<ZazaContext>();
builder.Services.AddSingleton<UserRepository>();

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
var userRepository = services.GetRequiredService<UserRepository>();
var responses = new Responses(userRepository);
responses.UserRegister(app);

app.UseCors(builder => {
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
});
app.UseStaticFiles();
app.Run();

internal record UserInfo {
    public required string Name { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }

    public User User => new User(Guid.NewGuid(), Name, Login, new Password(Password));
}