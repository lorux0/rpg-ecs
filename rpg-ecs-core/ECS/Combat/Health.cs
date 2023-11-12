namespace Lorux0r.RPG.Core.ECS.Combat;

public struct Health
{
    public float Current { get; set; }
    public float Max { get; }
    public bool IsDead => Current <= 0;

    public Health(float current, float max)
    {
        Current = current;
        Max = max;
    }
}