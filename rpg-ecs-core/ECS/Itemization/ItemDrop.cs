using Arch.Core;

namespace Lorux0r.RPG.Core.ECS.Itemization;

public struct ItemDrop
{
    public EntityReference DroppedBy { get; }
    public string ItemId { get; }
    
    public ItemDrop(EntityReference droppedBy, string itemId)
    {
        DroppedBy = droppedBy;
        ItemId = itemId;
    }
}