namespace Lorux0r.RPG.Core.ECS.Itemization;

public struct ItemDropResult
{
    public List<ItemId> Items { get; }
    
    public ItemDropResult(List<ItemId> items)
    {
        Items = items;
    }
}