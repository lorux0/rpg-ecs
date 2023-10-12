using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class DamageSystem
{
    private readonly World world;

    public DamageSystem(World world)
    {
        this.world = world;
    }
    
    [Query]
    [Any(typeof(Damage))]
    [None(typeof(DestroyEntitySchedule))]
    public void Tick(in Entity entity, ref Damage damage)
    {
        var targetEntity = damage.Target.Entity;
        // TODO: is there any better way of connecting components and entities?
        var health = targetEntity.Get<Health>();
        
        if (!health.IsDead)
        {
            var newHealth = Math.Max(0, health.Current - damage.Value);
            world.Set(targetEntity, new Health(newHealth, health.Max));
        }

        world.Add<DestroyEntitySchedule>(entity);
    }
}