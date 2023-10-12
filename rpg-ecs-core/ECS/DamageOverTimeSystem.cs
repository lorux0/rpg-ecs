using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class DamageOverTimeSystem : BaseSystem<World, Time>
{
    public DamageOverTimeSystem(World world) : base(world)
    {
    }
    
    [Query]
    [All(typeof(DamageOverTimeAttack))]
    [None(typeof(DestroyEntitySchedule))]
    public void Tick([Data] Time time, in Entity entity, ref DamageOverTimeAttack attack)
    {
        attack.Elapsed = attack.Elapsed.Add(time.Delta);
        attack.Remaining = attack.Remaining.Subtract(time.Delta);
        
        while (attack.Elapsed >= attack.Interval)
        {
            attack.Elapsed = attack.Elapsed.Subtract(attack.Interval);
            World.Create(new Damage(attack.Target, attack.Damage));
        }

        if (attack.Remaining.Ticks <= 0)
            entity.FlagToDestroy(World);
    }
}