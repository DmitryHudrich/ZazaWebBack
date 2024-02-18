using Zaza.Db;
using Zaza.Db.Repository;
using Zaza.Notes.Entities;
using Zaza.Telegram;
using Zaza.Web;

var builder = WebApplication.CreateBuilder();

builder.Services.AddCors();
builder.Services.AddSingleton<ZazaContext>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<TelegramApp>();

var app = builder.Build();

string mongoLink = app.Configuration.GetConnectionString("MongoString") ??
    throw new ArgumentNullException(nameof(mongoLink));
string mongoName = app.Configuration.GetValue<string>("MongoName") ??
    throw new ArgumentNullException(nameof(mongoName));

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
var tgTask = app.Services.GetRequiredService<TelegramApp>().Run();
app.Run();
await tgTask;

internal record UserInfo {
    public required string Name { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }

    public User User => new User(Guid.NewGuid(), Name, Login, new Password(Password));
}