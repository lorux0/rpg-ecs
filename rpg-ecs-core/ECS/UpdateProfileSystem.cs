using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class UpdateProfileSystem
{
    private readonly World world;
    private readonly QueryDescription query = new QueryDescription().WithAny<Profile>();

    public UpdateProfileSystem(World world)
    {
        this.world = world;
    }
    
    [Query]
    [Any(typeof(ProfileChangeRequest))]
    [None(typeof(DestroyEntitySchedule))]
    public void UpdateProfile(in Entity entity, ProfileChangeRequest profileChangeRequest)
    {
        world.Query(in query, (ref Profile profile) =>
        {
            // TODO: is there any better way of making a query that filters by profile id?
            if (profile.Id != profileChangeRequest.Id) return;
            profile.Name = profileChangeRequest.Name;
        });

        entity.FlagToDestroy(world);
    }
}