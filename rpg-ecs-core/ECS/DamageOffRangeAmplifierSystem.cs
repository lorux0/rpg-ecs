using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class DamageOffRangeAmplifierSystem : ISimpleSystem
{
    private readonly World world;

    public DamageOffRangeAmplifierSystem(World world)
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
    [All(typeof(Damage))]
    [None(typeof(DestroyEntitySchedule))]
    private void Apply(in Entity entity, ref Damage damage)
    {
        var attacker = damage.Attacker;
        var target = damage.Target;
        if (!world.Has<DamageOffRangeAmplifier>(attacker)) return;
        if (!world.Has<Position>(attacker)) return;
        if (!world.Has<Position>(target)) return;
        
        var attackerPosition = world.Get<Position>(attacker);
        var targetPosition = world.Get<Position>(target);
        var amplifier = world.Get<DamageOffRangeAmplifier>(attacker);
        
        if ((attackerPosition.Current - targetPosition.Current).sqrMagnitude < amplifier.Distance * amplifier.Distance) return;

        world.Create(new Damage(damage.Target, damage.Attacker, damage.Value * amplifier.AdditionMultiplier));
    }
}