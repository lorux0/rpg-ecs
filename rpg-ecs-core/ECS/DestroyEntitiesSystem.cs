using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public class DestroyEntitiesSystem : ISimpleSystem
{
    private readonly World world;
    private readonly QueryDescription query = new QueryDescription().WithAll<DestroyEntitySchedule>();

    public DestroyEntitiesSystem(World world)
    {
        this.world = world;
    }

    public void Update()
    {
        world.Destroy(in query);
    }
}