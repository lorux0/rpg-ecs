using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class ApplyResistancesFromEquipmentSystem : ISimpleSystem
{
    private readonly World world;

    public ApplyResistancesFromEquipmentSystem(World world)
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
        ResetPoisonResistanceQuery(world);
        ApplyPoisonResistanceQuery(world);
    }

    [Query]
    [All(typeof(Health), typeof(PoisonResistance))]
    private void ResetPoisonResistance(in Entity entity, ref Health health, ref PoisonResistance resistance)
    {
        resistance.Reset();
        world.Set(entity, resistance);
    }

    [Query]
    [All(typeof(Equipment), typeof(PoisonResistance))]
    private void ApplyPoisonResistance(ref Equipment equipment, ref PoisonResistance resistance)
    {
        if (!equipment.IsEquipped) return;
        var owner = equipment.Owner;

        if (!world.Has<PoisonResistance>(owner))
        {
            world.Add(owner, new PoisonResistance(resistance.Ratio));
        }
        else
        {
            var newResistance = world.Get<PoisonResistance>(owner);
            newResistance.Add(resistance);
            world.Set(owner, newResistance);
        }
    }
}