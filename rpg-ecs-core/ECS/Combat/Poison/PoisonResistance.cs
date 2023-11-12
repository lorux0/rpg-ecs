namespace Lorux0r.RPG.Core.ECS.Combat.Poison;

public struct PoisonResistance
{
    public float Ratio { get; private set; }

    public PoisonResistance(float ratio)
    {
        Ratio = ratio;
    }

    public void Reset() =>
        Ratio = 0;

    public void Add(PoisonResistance resistance) =>
        Ratio += resistance.Ratio;
}