using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public struct PoisonDamage
{
    public EntityReference Target { get; }
    public float Damage { get; }
    
    public PoisonDamage(EntityReference target, float damage)
    {
        Target = target;
        Damage = damage;
    }
}