using System.Collections.Generic;
using System.Linq;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using Lorux0r.RPG.Core.ECS.Combat;
using NUnit.Framework;
using UnityEngine;

namespace Lorux0r.Core.Tests;

public class RangedPhysicalAttackTests
{
    private RangedAttackSystem system = null!;
    private World world = null!;
    private Entity attacker;
    private Entity target;
    private List<PhysicalDamage> targetAccumulatedDamage;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        attacker = world.Create(new Position(new Vector3(1, 4, 2)), new AccumulatedPhysicalDamage());
        targetAccumulatedDamage = new List<PhysicalDamage>();
        target = world.Create(new Position(new Vector3(1, 3, 2)), new AccumulatedPhysicalDamage(targetAccumulatedDamage));
        system = new RangedAttackSystem(world);
        system.Initialize();
    }

    [TearDown]
    public void AfterEachTest()
    {
        system.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void HitOnce()
    {
        var entity = world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 10, 1));

        system.Update();

        Assert.AreEqual(1, targetAccumulatedDamage.Count);
        Assert.AreEqual(10, targetAccumulatedDamage[0].Value);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void HitMultipleTimes()
    {
        var entities = new[]
        {
            world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 10, 1)),
            world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 5, 1)),
            world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 30, 1))
        };
        
        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<PhysicalDamage>(), damages);
        Assert.AreEqual(3, targetAccumulatedDamage.Count);
        Assert.AreEqual(10, targetAccumulatedDamage[2].Value);
        Assert.AreEqual(5, targetAccumulatedDamage[1].Value);
        Assert.AreEqual(30, targetAccumulatedDamage[0].Value);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }

    [Test]
    public void DontHitWhenOutOfRange()
    {
        world.Set(target, new Position(new Vector3(1, 4, 2)));
        world.Set(attacker, new Position(new Vector3(0, 3, 3)));
        var entity = world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 10, 1));

        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<PhysicalDamage>(), damages);
        CollectionAssert.IsEmpty(damages);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void SkipOutOfRangeAttacksWhenMultipleHits()
    {
        world.Set(target, new Position(new Vector3(1, 4, 2)));
        world.Set(attacker, new Position(new Vector3(0, 3, 3)));

        var entities = new[]
        {
            world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 10, 1)),
            world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 10, 5)),
            world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 10, 2)),
            world.Create(new RangedPhysicalAttack(attacker.Reference(), target.Reference(), 10, 3))
        };
        
        system.Update();

        Assert.AreEqual(3, targetAccumulatedDamage.Count);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }
}