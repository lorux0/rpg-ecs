using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class ApplyDamageOffRangeFromEquipmentSystem : ISimpleSystem
{
    private readonly World world;

    public ApplyDamageOffRangeFromEquipmentSystem(World world)
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
        ResetAmplifierQuery(world);
        ApplyAmplifierQuery(world);
    }

    [Query]
    [All(typeof(Health), typeof(Position), typeof(DamageOffRangeAmplifier))]
    [None(typeof(DestroyEntitySchedule))]
    private void ResetAmplifier(in Entity entity, ref Health health, ref Position position,
        ref DamageOffRangeAmplifier amplifier)
    {
        amplifier.Reset();
    }

    [Query]
    [All(typeof(Equipment), typeof(DamageOffRangeAmplifier))]
    [None(typeof(DestroyEntitySchedule))]
    private void ApplyAmplifier(in Entity entity, ref Equipment equipment, ref DamageOffRangeAmplifier amplifier)
    {
        if (!equipment.IsEquipped) return;
        var equipmentOwner = equipment.Owner;
        if (!world.Has<DamageOffRangeAmplifier>(equipmentOwner))
            world.Add<DamageOffRangeAmplifier>(equipmentOwner);
        ref var equipmentOwnerAmplifier = ref world.Get<DamageOffRangeAmplifier>(equipmentOwner);
        equipmentOwnerAmplifier.Add(amplifier);
    }
}