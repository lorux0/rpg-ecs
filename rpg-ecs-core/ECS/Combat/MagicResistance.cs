namespace Lorux0r.RPG.Core.ECS.Combat;

public struct MagicResistance
{
    public float Value;

    public MagicResistance(float value)
    {
        Value = value;
    }

    public void Add(MagicResistance other) => Value += other.Value;
}