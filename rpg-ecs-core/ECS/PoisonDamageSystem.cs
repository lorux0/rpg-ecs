using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class PoisonDamageSystem : ISimpleSystem
{
    private readonly World world;

    public PoisonDamageSystem(World world)
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
        ApplyQuery(world);
    }

    [Query]
    [All(typeof(PoisonDamage))]
    private void Apply(in Entity entity, ref PoisonDamage damage)
    {
        var finalDamage = damage.Damage;
        var target = damage.Target;
        
        if (world.Has<PoisonResistance>(target))
        {
            var resistance = world.Get<PoisonResistance>(target);
            var ratio = Math.Max(0, Math.Min(1, 1 - resistance.Ratio));
            finalDamage *= ratio;
        }

        world.Create(new Damage(target, damage.Attacker, finalDamage));
        
        entity.FlagToDestroy(world);
    }
}