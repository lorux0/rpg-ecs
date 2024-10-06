using Arch.Core;
using Arch.System;
using Lorux0r.RPG.Core.ECS.Combat;

namespace Lorux0r.RPG.Core.ECS;

public partial class SetAsCorpseOnDeadSystem : ISimpleSystem
{
    private readonly World world;

    public SetAsCorpseOnDeadSystem(World world)
    {
        this.world = world;
    }
    
    public void Initialize()
    {
    }

    public void Dispose()
    {
    }

    public void Update()
    {
    }

    [Query]
    [None(typeof(Corpse))]
    private void Execute(in Entity entity, ref Health health)
    {
        if (!health.IsDead) return;
        world.Add(entity, new Corpse());
    }
}