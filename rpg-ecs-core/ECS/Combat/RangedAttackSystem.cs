using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS.Combat;

public partial class RangedAttackSystem : ISimpleSystem
{
    private readonly World world;

    public RangedAttackSystem(World world)
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
    [None(typeof(DestroyEntitySchedule))]
    private void Apply(in Entity entity, ref RangedPhysicalAttack attack)
    {
        var attackerEntity = attack.Attacker.Entity;
        var targetEntity = attack.Target.Entity;
        var attackerPosition = world.Get<Position>(attackerEntity).Current;
        var targetPosition = world.Get<Position>(targetEntity).Current;

        if ((targetPosition - attackerPosition).sqrMagnitude <= attack.Range * attack.Range)
        {
            ref var accumulatedPhysicalDamage = ref world.TryGetRef<AccumulatedPhysicalDamage>(targetEntity, out var exists);
            
            if (exists)
                accumulatedPhysicalDamage.All.Add(new PhysicalDamage(attack.Damage));
        }
        
        entity.FlagToDestroy(world);
    }
}