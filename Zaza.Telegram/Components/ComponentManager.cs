using Telegram.Bot.Types;

namespace Zaza.Telegram.Components;

public delegate void ComponentHandler(Update update);

internal static class ComponentManager
{
    private static Component? previous;
    private static readonly List<Component> components = [];
    public static IReadOnlyList<Component> Components => components;

    public static ComponentChain Begin(ComponentHandler action, string command)
    {
        var comp = new Component(command, action);
        var res = new ComponentChain();
        res.Components.Add(comp);
        return res;
    }

    public static ComponentChain Then(this ComponentChain chain, ComponentHandler action, string command)
    {
        chain.Components.Add(new Component(command, action));
        return chain;
    }

    public static Component End(this ComponentChain componentChain)
    {
        var res = componentChain.Components[0];
        var cache = res;
        for (int i = 1; i < componentChain.Components.Count; i++)
        {
            cache.Next = componentChain.Components[i];
            cache = cache.Next;
        }
        components.Add(res);
        return res;
    }

    public static void ExecuteAll(Update update)
    {
        foreach (var component in components)
        {
            if (previous?.Next != null)
            {
                exec(previous.Next);
                return;
            }
            exec(component);
        }

        bool exec(Component? component)
        {
            if (component != null && component.Execute(update, out Component? prev))
            {
                previous = prev;
                return true;
            }
            return false;
        }
    }
}
