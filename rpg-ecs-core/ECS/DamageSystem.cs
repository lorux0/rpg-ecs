using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class DamageSystem : ISimpleSystem
{
    private readonly World world;

    public DamageSystem(World world)
    {
        this.world = world;
    }

    public void Initialize()
    {
    }
    
    public void Dispose()
    {
    }

    public void Update()
    {
        ApplyQuery(world);
    }

    [Query]
    [Any(typeof(Damage))]
    [None(typeof(DestroyEntitySchedule))]
    private void Apply(in Entity entity, ref Damage damage)
    {
        var targetEntity = damage.Target.Entity;
        ref var health = ref targetEntity.Get<Health>();

        if (!health.IsDead)
            health.Current = Math.Max(0, health.Current - damage.Value);

        world.Add<DestroyEntitySchedule>(entity);
    }
}