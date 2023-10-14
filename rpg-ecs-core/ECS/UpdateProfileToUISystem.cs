using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;

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

    public void Update()
    {
        BroadcastToUIQuery(world);
        UpdateProfileFromUIQuery(world);
    }

    [Query]
    [All(typeof(Profile), typeof(Health))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void BroadcastToUI(in Entity entity, ref Profile profile, ref Health health, ref Position position)
    {
        // TODO: how we can check that components (profile & health) have just changed so we can avoid broadcasting on every query?
        // broadcast update event to ui
        // avoid sending ref so the ecs state is not compromised
        profileBridge.BroadcastProfileChanges(profile, health, position);
    }

    [Query]
    [All(typeof(UpdateProfileRequest))]
    [None(typeof(DestroyEntitySchedule))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateProfileFromUI(in Entity entity, ref UpdateProfileRequest request)
    {
        var requestId = request.Id;
        var requestName = request.Name;

        // TODO: can we improve this instead of making a manual query here?
        world.Query(in new QueryDescription().WithAll<Profile>(), (in Entity _, ref Profile profile) =>
        {
            if (profile.Id != requestId) return;
            profile.Name = requestName;
        });

        entity.FlagToDestroy(world);
    }
}
