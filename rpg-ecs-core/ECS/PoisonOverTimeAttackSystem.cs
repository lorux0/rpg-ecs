using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class PoisonOverTimeAttackSystem : BaseSystem<World, Time>
{
    public PoisonOverTimeAttackSystem(World world) : base(world)
    {
    }

    [Query]
    [All(typeof(PoisonOverTimeAttack))]
    [None(typeof(DestroyEntitySchedule))]
    private void Apply([Data] Time time, in Entity entity, ref PoisonOverTimeAttack attack)
    {
        attack.Elapsed = attack.Elapsed.Add(time.Delta);
        attack.Remaining = attack.Remaining.Subtract(time.Delta);

        while (attack.Elapsed >= attack.Interval)
        {
            attack.Elapsed = attack.Elapsed.Subtract(attack.Interval);
            World.Create(new PoisonDamage(attack.Target, attack.Damage));
        }

        if (attack.Remaining.Ticks <= 0)
            entity.FlagToDestroy(World);
    }
}