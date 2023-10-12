using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class UpdateProfileToUISystem
{
    private readonly World world;
    private readonly ECSProfileAntiCorruptionLayer profileBridge;

    public UpdateProfileToUISystem(World world,
        ECSProfileAntiCorruptionLayer profileBridge)
    {
        this.world = world;
        this.profileBridge = profileBridge;
    }

    [Query]
    [All(typeof(Profile), typeof(Health))]
    public void BroadcastToUI(in Entity entity, ref Profile profile, ref Health health, ref Position position)
    {
        // TODO: how we can check that components (profile & health) have just changed so we can avoid broadcasting on every query?
        // broadcast update event to ui
        // avoid sending ref so the ecs state is not compromised
        profileBridge.BroadcastProfileChanges(profile, health, position);
    }

    [Query]
    [All(typeof(UpdateProfileRequest))]
    [None(typeof(DestroyEntitySchedule))]
    public void UpdateProfileFromUI(in Entity entity, ref UpdateProfileRequest request)
    {
        var requestId = request.Id;
        var requestName = request.Name;

        // TODO: can we improve this instead of making a manual query here?
        world.Query(in new QueryDescription().WithAll<Profile>(), (in Entity pe, ref Profile profile) =>
        {
            if (profile.Id != requestId) return;
            profile.Name = requestName;
        });
        
        entity.FlagToDestroy(world);
    }
}