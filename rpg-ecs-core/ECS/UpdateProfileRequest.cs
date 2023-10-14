using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public struct UpdateProfileRequest
{
    public EntityReference Profile { get; }
    public string Name { get; }

    public UpdateProfileRequest(EntityReference profile, string name)
    {
        Profile = profile;
        Name = name;
    }
}