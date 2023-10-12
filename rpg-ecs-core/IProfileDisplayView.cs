namespace Lorux0r.RPG.Core;

public interface IProfileDisplayView
{
    event Action<string, string> ChangeNameRequested;
    
    void Show(string id, string name, float health, float max);
}