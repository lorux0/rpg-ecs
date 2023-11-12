namespace Lorux0r.RPG.Core.ECS.Combat;

public struct DamageOffRangeAmplifier
{
    public float Distance { get; }
    public float AdditionMultiplier { get; private set; }
    
    public DamageOffRangeAmplifier(float distance, float additionMultiplier)
    {
        Distance = distance;
        AdditionMultiplier = additionMultiplier;
    }

    public void Reset() =>
        AdditionMultiplier = 0;

    public void Add(DamageOffRangeAmplifier amplifier) =>
        AdditionMultiplier += amplifier.AdditionMultiplier;
}