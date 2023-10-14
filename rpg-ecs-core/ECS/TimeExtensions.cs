using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public static class TimeExtensions
{
    private static readonly Entity[] temp = new Entity[1];

    public static Time GetTime(this World world)
    {
        world.GetEntities(new QueryDescription().WithAll<Time>(), temp);
        return world.Get<Time>(temp[0]);
    }
}