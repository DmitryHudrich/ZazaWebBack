using Zaza.Notes.Entities;

namespace Zaza.Notes.Controllers;

public class UserController(ZazaDb)
{
    public IReadOnlyList<User> Users => (IReadOnlyList<User>)UserRepository.Get();

    public async Task<User?> AddAsync(User user)
    {
        await Users.Write(user);
        await Console.Out.WriteLineAsync(user.Name);
        return user;
    }

    public User? Get(Guid? Id) => Users.FirstOrDefault(obj => obj.Id == Id);
    public Guid? Find(string login) => Users.ToList().Find(obj => obj.Login == login)?.Id;
    public bool Remove(User user) => Users.ToList().Remove(user);
    public void Remove(Guid old) => Users.ToList().ChangeById(old);
    public void Change(Guid old, User young) => Users.ToList().ChangeById(old, young);
}
