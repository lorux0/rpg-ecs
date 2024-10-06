namespace Lorux0r.RPG.Core.ECS.Combat;

public struct AccumulatedPhysicalDamage
{
    public List<PhysicalDamage> All { get; }

    public AccumulatedPhysicalDamage()
    {
        All = new List<PhysicalDamage>();
    }
    
    public AccumulatedPhysicalDamage(List<PhysicalDamage> all)
    {
        All = all;
    }
}