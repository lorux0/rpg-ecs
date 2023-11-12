namespace Lorux0r.RPG.Core.ECS;

public interface IItemDropTableProvider
{
    ItemDropChance[] GetByTag(string tag);
}