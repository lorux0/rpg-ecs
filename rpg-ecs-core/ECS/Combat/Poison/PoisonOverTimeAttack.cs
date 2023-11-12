using Arch.Core;

namespace Lorux0r.RPG.Core.ECS.Combat.Poison;

public struct PoisonOverTimeAttack : IOverTimeAction
{
    public EntityReference Target { get; }
    public EntityReference Attacker { get; }
    public float Damage { get; }
    public TimeSpan Interval { get; }
    public TimeSpan Remaining { get; set; }
    public TimeSpan Elapsed { get; set; }
    
    public PoisonOverTimeAttack(EntityReference target, EntityReference attacker, float damage, TimeSpan interval, TimeSpan remaining, TimeSpan elapsed)
    {
        Target = target;
        Attacker = attacker;
        Damage = damage;
        Interval = interval;
        Remaining = remaining;
        Elapsed = elapsed;
    }
}