using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS.Combat;

public partial class PhysicalDamageSystem : ISimpleSystem
{
    private readonly World world;

    public PhysicalDamageSystem(World world)
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
        ExecuteQuery(world);
    }

    [Query]
    [None(typeof(DestroyEntitySchedule), typeof(Corpse))]
    private void Execute(ref AccumulatedPhysicalDamage accumulatedPhysicalDamage, ref Health health)
    {
        if (health.IsDead) return;
        
        foreach (var damage in accumulatedPhysicalDamage.All)
        {
            health.Current = Math.Max(0, health.Current - damage.Value);
        }
    }
}