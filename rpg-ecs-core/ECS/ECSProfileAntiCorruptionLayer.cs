using Arch.Core;
using Lorux0r.RPG.Core.ECS.Combat;
using UIProfile = Lorux0r.RPG.Core.Profile;

namespace Lorux0r.RPG.Core.ECS;

public class ECSProfileAntiCorruptionLayer : IProfileGateway
{
    private readonly World world;

    public event Action<UIProfile>? OnProfileUpdated;

    public ECSProfileAntiCorruptionLayer(World world)
    {
        this.world = world;
    }

    public Task<UIProfile?> GetProfile(string profileId, CancellationToken cancellationToken)
    {
        UIProfile? result = null;

        world.Query(in new QueryDescription().WithAll<Profile, Health, Position>(),
            (in Entity entity, ref Profile profile, ref Health health, ref Position position) =>
            {
                // TODO: is there any way to configure the query with id filtering?
                if (profile.Id != profileId) return;
                result = ToUIProfile(profile, health, position);
            });

        return Task.FromResult(result);
    }

    public async Task UpdateProfile(UIProfile profile, CancellationToken cancellationToken)
    {
        // TODO: we need to ensure that ecs processed this request before completing the task. Figure out how can we cancel the operation
        var queryResult = new GetProfileEntityByIdQuery(profile.Id, world);
        world.InlineEntityQuery<GetProfileEntityByIdQuery, Profile>(in new QueryDescription().WithAll<Profile>(), ref queryResult);
        if (!queryResult.Success) return; 
        world.Create(new UpdateProfileRequest(queryResult.Result, profile.Name));
    }

    public void BroadcastProfileChanges(Profile profile, Health health, Position position) =>
        OnProfileUpdated?.Invoke(ToUIProfile(profile, health, position));

    private static UIProfile ToUIProfile(Profile profile, Health health, Position position)
    {
        return new UIProfile(profile.Id, profile.Name, health.Current, health.Max, position.Current);
    }
}