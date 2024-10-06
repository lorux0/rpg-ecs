namespace Lorux0r.RPG.Core.ECS.Itemization;

public interface IItemDropTableProvider
{
    IEnumerable<ItemDropRatio> GetByTag(string tag);
}