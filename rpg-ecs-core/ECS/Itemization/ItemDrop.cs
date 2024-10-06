using Arch.Core;

namespace Lorux0r.RPG.Core.ECS.Itemization;

public struct ItemDrop
{
    public EntityReference DroppedBy { get; }
    public ItemId ItemId { get; }
    
    public ItemDrop(EntityReference droppedBy, ItemId itemId)
    {
        DroppedBy = droppedBy;
        ItemId = itemId;
    }
}