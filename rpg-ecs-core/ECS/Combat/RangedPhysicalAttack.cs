using Arch.Core;

namespace Lorux0r.RPG.Core.ECS.Combat;

public struct RangedPhysicalAttack
{
    public EntityReference Attacker { get; }
    public EntityReference Target { get; }
    public float Damage { get; }
    public float Range { get; }

    public RangedPhysicalAttack(EntityReference attacker, EntityReference target, float damage, float range)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
        Range = range;
    }
}