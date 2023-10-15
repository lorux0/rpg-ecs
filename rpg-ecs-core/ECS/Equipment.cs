using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public struct Equipment
{
    public EntityReference Owner { get; }
    public bool IsEquipped { get; }

    public Equipment(EntityReference owner, bool isEquipped)
    {
        Owner = owner;
        IsEquipped = isEquipped;
    }
}