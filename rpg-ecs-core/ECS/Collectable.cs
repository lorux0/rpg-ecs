using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public struct Collectable
{
    public bool Collected => CollectedBy != null;
    public EntityReference? CollectedBy { get; }
    
    public Collectable(EntityReference? collectedBy)
    {
        CollectedBy = collectedBy;
    }
}