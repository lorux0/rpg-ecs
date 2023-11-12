using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using Lorux0r.RPG.Core.ECS.Combat;
using Lorux0r.RPG.Core.ECS.Combat.Poison;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class PoisonOverTimeAttackSystemTests
{
    private PoisonOverTimeAttackSystem system = null!;
    private World world = null!;
    private Entity target;
    private Entity attacker;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new PoisonOverTimeAttackSystem(world);
        target = world.Create(new Health(100, 100));
        attacker = world.Create(new Health(100, 100));
        system.Initialize();
    }

    [TearDown]
    public void AfterEachTest()
    {
        system.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void HitOnceBeforeExpiring()
    {
        var entity = world.Create(new PoisonOverTimeAttack(target.Reference(), attacker.Reference(), 10,
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(0.3)));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1.2), 1);
        system.BeforeUpdate(time);
        system.Update(time);
        system.AfterUpdate(time);

        var attack = world.Get<PoisonOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(8.8), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0.5), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<PoisonDamage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.IsFalse(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void DontHitWhenIntervalIsNotElapsed()
    {
        var entity = world.Create(new PoisonOverTimeAttack(target.Reference(), attacker.Reference(), 10,
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(0.3)));

        var time = new Time(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(0.6), 1);
        system.BeforeUpdate(time);
        system.Update(time);
        system.AfterUpdate(time);

        var attack = world.Get<PoisonOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(9.4), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0.9), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<PoisonDamage>(), damages);
        CollectionAssert.IsEmpty(damages);
        Assert.IsFalse(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void HitManyTimesBeforeExpiring()
    {
        var entity = world.Create(new PoisonOverTimeAttack(target.Reference(), attacker.Reference(), 10,
            TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(0)));

        var time = new Time(TimeSpan.FromSeconds(100), TimeSpan.FromSeconds(10), 1);
        system.BeforeUpdate(time);
        system.Update(time);
        system.AfterUpdate(time);

        var attack = world.Get<PoisonOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(10), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<PoisonDamage>(), damages);
        Assert.AreEqual(2, damages.Count);
        Assert.IsFalse(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void HitAndExpire()
    {
        var entity = world.Create(new PoisonOverTimeAttack(target.Reference(), attacker.Reference(), 10,
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.3)));

        var time = new Time(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 1);
        system.BeforeUpdate(time);
        system.Update(time);
        system.AfterUpdate(time);

        var attack = world.Get<PoisonOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(0), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0.3), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<PoisonDamage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(entity));
    }
}