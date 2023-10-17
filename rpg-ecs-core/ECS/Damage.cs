using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public struct Damage
{
    public EntityReference Target { get; }
    public EntityReference Attacker { get; }
    public float Value { get; }

    public Damage(EntityReference target, EntityReference attacker, float value)
    {
        Target = target;
        Attacker = attacker;
        Value = value;
    }
}