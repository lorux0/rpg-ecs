using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

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
    [Any(typeof(RangedAttack))]
    [None(typeof(Damage))]
    [None(typeof(DestroyEntitySchedule))]
    private void Apply(in Entity entity, ref RangedAttack attack)
    {
        var attackerEntity = attack.Attacker.Entity;
        // TODO: is there any better way of connecting components and entities?
        var attackerPosition = attackerEntity.Get<Position>().Current;
        var targetPosition = attack.Target.Entity.Get<Position>().Current;

        if ((targetPosition - attackerPosition).sqrMagnitude <= attack.Range * attack.Range)
            world.Add(entity, new Damage(attack.Target, attack.Attacker, attack.Damage));

        entity.FlagToDestroy(world);
    }
}