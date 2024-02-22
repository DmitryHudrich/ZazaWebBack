using Telegram.Bot.Types;
using Zaza.Telegram.Components;

namespace Zaza.Telegram.CommandSystem;

public class Component(ComponentHandler action, string? command = null) {
    private readonly string? command = command;
    public Component? Next { get; set; }
    public string? Command => command;
    public ComponentHandler? CAction => action;
    public bool Execute(Update update, out Component? executed) {
        if (command == null || command == update.Message?.Text) {
            CAction?.Invoke(update);
            executed = this;
            return true;
        }
        executed = null;
        return false;
    }
}
