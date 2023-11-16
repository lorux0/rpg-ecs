using Arch.Core;
using Arch.System;
using Lorux0r.RPG.Core.ECS.Combat;

namespace Lorux0r.RPG.Core.ECS;

public partial class UpdateProfileToUISystem : ISimpleSystem
{
    private readonly World world;
    private readonly ECSProfileAntiCorruptionLayer profileBridge;

    public UpdateProfileToUISystem(World world,
        ECSProfileAntiCorruptionLayer profileBridge)
    {
        this.world = world;
        this.profileBridge = profileBridge;
    }

    public void Initialize()
    {
    }
    
    public void Dispose()
    {
    }

    public void Update()
    {
        BroadcastToUIQuery(world);
        UpdateProfileFromUIQuery(world);
    }

    [Query]
    [All(typeof(Profile), typeof(Health))]
    private void BroadcastToUI(ref Profile profile, ref Health health, ref Position position)
    {
        // TODO: how we can check that components (profile & health) have just changed so we can avoid broadcasting on every query?
        // broadcast update event to ui
        // avoid sending ref so the ecs state is not compromised
        profileBridge.BroadcastProfileChanges(profile, health, position);
    }

    [Query]
    [All(typeof(UpdateProfileRequest))]
    [None(typeof(DestroyEntitySchedule))]
    private void UpdateProfileFromUI(in Entity entity, ref UpdateProfileRequest request)
    {
        var profile = world.Get<Profile>(request.Profile);
        profile.Name = request.Name;
        world.Set(request.Profile, profile);
        entity.FlagToDestroy(world);
    }
}
