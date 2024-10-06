using Lorux0r.RPG.Core.ECS.Itemization;

namespace Lorux0r.RPG.Core;

public class HardcodedItemDropTableProvider : IItemDropTableProvider
{
    public IEnumerable<ItemDropRatio> GetByTag(string tag)
    {
        throw new NotImplementedException();
    }
}