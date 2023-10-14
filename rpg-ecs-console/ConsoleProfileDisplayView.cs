using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;

namespace Lorux0r.RPG.Console;

public class ConsoleProfileDisplayView : IProfileDisplayView
{
    public event Action<string, string> ChangeNameRequested;

    public void Show(string id, string name, float health, float max)
    {
        System.Console.WriteLine($"Profile updated - {id}: {name}, {health}/{max}");
    }
}