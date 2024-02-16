namespace ZazaNotes;

public static class NoteController {
    public static void Add(User user, Note note) => user.Notes.Add(note);
    public static Note? Get(User user, Guid Id) => user.Notes.FirstOrDefault(obj => obj.Id == Id);
    public static bool Remove(User user, Note note) => user.Notes.Remove(note);
    public static void Remove(User user, Guid removable) => user.Notes.ChangeById(removable);
    public static void Change(User user, Guid old, Note young) => user.Notes.ChangeById(old, young);
}

public record class Note(Guid Id, string Title, string Description, string Author) : IIdentifiable {
    public DateTime Created { get; } = DateTime.Now;
    public DateTime Updated { get; set; }
}
