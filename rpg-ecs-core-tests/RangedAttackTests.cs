using System.Collections.Generic;
using System.Linq;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using NUnit.Framework;
using UnityEngine;

namespace Lorux0r.Core.Tests;

public class RangedAttackTests
{
    private RangedAttackSystem system = null!;
    private World world = null!;
    private Entity attacker;
    private Entity target;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        attacker = world.Create(new Position(new Vector3(1, 4, 2)));
        target = world.Create(new Position(new Vector3(1, 3, 2)));
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
        var entity = world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 10, 1));

        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void HitMultipleTimes()
    {
        var entities = new[]
        {
            world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 10, 1)),
            world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 5, 1)),
            world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 30, 1))
        };
        
        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(3, damages.Count);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }

    [Test]
    public void DontHitWhenOutOfRange()
    {
        world.Set(target, new Position(new Vector3(1, 4, 2)));
        world.Set(attacker, new Position(new Vector3(0, 3, 3)));
        var entity = world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 10, 1));

        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
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
            world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 10, 1)),
            world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 10, 5)),
            world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 10, 2)),
            world.Create(new RangedAttack(attacker.Reference(), target.Reference(), 10, 3))
        };
        
        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(3, damages.Count);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }
}