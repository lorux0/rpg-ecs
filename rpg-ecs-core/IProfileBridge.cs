namespace Lorux0r.RPG.Core;

public interface IProfileBridge
{
    public event Action<Profile> OnProfileUpdated;
}