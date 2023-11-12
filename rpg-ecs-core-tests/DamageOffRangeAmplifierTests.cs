using Arch.Core;
using Lorux0r.RPG.Core.ECS;
using Lorux0r.RPG.Core.ECS.Combat;
using NUnit.Framework;
using UnityEngine;

namespace Lorux0r.Core.Tests;

public class DamageOffRangeAmplifierTests
{
    private World world = null!;
    private ApplyDamageOffRangeFromEquipmentSystem equipmentSystem = null!;
    private DamageOffRangeAmplifierSystem amplifierSystem = null!;
    private Entity target;
    private Entity attacker;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        equipmentSystem = new ApplyDamageOffRangeFromEquipmentSystem(world);
        amplifierSystem = new DamageOffRangeAmplifierSystem(world);
        attacker = world.Create(new Health(100, 100), new Position(new Vector3(3, 4, 4)));
        target = world.Create(new Health(100, 100), new Position(new Vector3(3, 6, 5)));
    }

    [TearDown]
    public void AfterEachTest()
    {
        equipmentSystem.Dispose();
        amplifierSystem.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void ApplyExtraDamageWhenTargetIsOffRange()
    {
        world.Create(new Equipment(world.Reference(attacker), true),
            new DamageOffRangeAmplifier(2, 0.2f));
        world.Create(new Equipment(world.Reference(attacker), true),
            new DamageOffRangeAmplifier(2, 0.3f));

        world.Create(new Damage(world.Reference(target), world.Reference(attacker), 10));
        
        equipmentSystem.Update();
        amplifierSystem.Update();

        var damages = new Entity[2];
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        var additionalDamage = world.Get<Damage>(damages[0]);
        Assert.AreEqual(2, damages.Length);
        Assert.AreEqual(5, additionalDamage.Value);
        Assert.AreEqual(target, additionalDamage.Target.Entity);
        Assert.AreEqual(attacker, additionalDamage.Attacker.Entity);
        Assert.AreEqual(10, world.Get<Damage>(damages[1]).Value);
    }

    [Test]
    public void DontApplyExtraDamageWhenEquipmentIsUnequipped()
    {
        world.Create(new Equipment(world.Reference(attacker), false),
            new DamageOffRangeAmplifier(2, 0.2f));
        
        world.Create(new Damage(world.Reference(target), world.Reference(attacker), 10));
        
        equipmentSystem.Update();
        amplifierSystem.Update();
        
        var damages = new Entity[1];
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Length);
        Assert.AreEqual(10, world.Get<Damage>(damages[0]).Value);
    }

    [Test]
    public void DontApplyExtraDamageWhenIsInRange()
    {
        world.Create(new Equipment(world.Reference(attacker), false),
            new DamageOffRangeAmplifier(5, 0.2f));
        
        world.Create(new Damage(world.Reference(target), world.Reference(attacker), 10));
        
        equipmentSystem.Update();
        amplifierSystem.Update();
        
        var damages = new Entity[1];
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Length);
        Assert.AreEqual(10, world.Get<Damage>(damages[0]).Value);
    }
}