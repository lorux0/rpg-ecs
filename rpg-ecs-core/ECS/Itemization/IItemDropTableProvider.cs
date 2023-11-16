namespace Lorux0r.RPG.Core.ECS.Itemization;

public interface IItemDropTableProvider
{
    ItemDropChance[] GetByTag(string tag);
}