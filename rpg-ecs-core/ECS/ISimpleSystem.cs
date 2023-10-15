namespace Lorux0r.RPG.Core.ECS;

public interface ISimpleSystem : IDisposable
{
    void Initialize();
    void Update();
}