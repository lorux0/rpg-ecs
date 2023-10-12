using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public static class EntityExtensions
{
    public static void FlagToDestroy(this Entity entity, World world) =>
        world.Add(entity, new DestroyEntitySchedule());
}