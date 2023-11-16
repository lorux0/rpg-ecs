namespace Lorux0r.RPG.Core.ECS.Combat.Poison;

public struct PoisonCloudArea : IOverTimeAction
{
    public float Radius { get; }
    public float Damage { get; }
    public TimeSpan Interval { get; }
    public TimeSpan Remaining { get; set; }
    public TimeSpan Elapsed { get; set; }

    public PoisonCloudArea(float radius, float damage, TimeSpan interval, TimeSpan remaining, TimeSpan elapsed)
    {
        Radius = radius;
        Damage = damage;
        Interval = interval;
        Remaining = remaining;
        Elapsed = elapsed;
    }
}