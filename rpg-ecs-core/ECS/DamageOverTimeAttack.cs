using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public struct DamageOverTimeAttack : IOverTimeAction
{
    public EntityReference Target { get; }
    public float Damage { get; }
    public TimeSpan Interval { get; }
    public TimeSpan Remaining { get; set; }
    public TimeSpan Elapsed { get; set; }

    public DamageOverTimeAttack(EntityReference target, float damage, TimeSpan interval, TimeSpan remaining, TimeSpan elapsed)
    {
        Target = target;
        Damage = damage;
        Interval = interval;
        Remaining = remaining;
        Elapsed = elapsed;
    }
}