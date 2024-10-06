using Arch.Core;
using Arch.Core.Extensions;

namespace Lorux0r.RPG.Core.ECS;

public struct GetProfileEntityByIdQuery : IForEachWithEntity<Profile>
{
    private readonly string id;
    private readonly World world;

    public EntityReference Result;
    public bool Success;

    public ref Profile Profile => ref world.Get<Profile>(Result.Entity);
    
    public GetProfileEntityByIdQuery(string id, World world)
    {
        this.id = id;
        this.world = world;
        Result = EntityReference.Null;
        Success = false;
    }
        
    public void Update(in Entity entity, ref Profile profile)
    {
        if (profile.Id != id) return;
        Result = world.Reference(entity);
        Success = true;
    }
}