using Arch.Core;

namespace Lorux0r.RPG.Core.ECS.Combat.Poison;

public struct PoisonDamage
{
    public EntityReference Target { get; }
    public EntityReference Attacker { get; }
    public float Damage { get; }
    
    public PoisonDamage(EntityReference target, EntityReference attacker, float damage)
    {
        Target = target;
        Attacker = attacker;
        Damage = damage;
    }
}