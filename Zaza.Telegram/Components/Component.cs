using Telegram.Bot.Types;

namespace Zaza.Telegram.Components;

public class Component(string command, ComponentHandler action)
{
    private readonly string command = command;
    public Component? Next { get; set; }
    public string Command => command;
    public ComponentHandler? CAction => action;
    public bool Execute(Update update, out Component? executed)
    {
        if (command == update.Message?.Text)
        {
            CAction?.Invoke(update);
            executed = this;
            return true;
        }
        executed = null;
        return false;
    }
}
