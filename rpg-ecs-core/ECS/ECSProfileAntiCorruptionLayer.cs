using UIProfile = Lorux0r.RPG.Core.Profile;

namespace Lorux0r.RPG.Core.ECS;

public class ECSProfileAntiCorruptionLayer : IProfileBridge
{
    public event Action<UIProfile>? OnProfileUpdated;

    public ECSProfileAntiCorruptionLayer()
    {
    }

    public void BroadcastProfileChanges(Profile profile, Health health) =>
        OnProfileUpdated?.Invoke(new UIProfile(profile.Id, profile.Name, health.Current, health.Max));
}