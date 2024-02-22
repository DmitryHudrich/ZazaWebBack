namespace Zaza.Notes.Entities;

public record class Note(Guid Id, string Title, string Description, User Author) : IIdentifiable {
    public DateTime Created { get; } = DateTime.Now;
    public DateTime Updated { get; set; }
}
