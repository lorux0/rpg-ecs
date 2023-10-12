using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public struct Damage
{
    public EntityReference Target { get; }
    public float Value { get; }

    public Damage(EntityReference target, float value)
    {
        Target = target;
        Value = value;
    }
}