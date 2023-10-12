using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class DamageOverTimeSystemTests
{
    private DamageOverTimeSystem system = null!;
    private World world = null!;
    private Entity target;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new DamageOverTimeSystem(world);
        target = world.Create(new Health(100, 100));
    }

    [TearDown]
    public void AfterEachTest()
    {
        World.Destroy(world);
    }

    [Test]
    public void HitOnceBeforeExpiring()
    {
        var entity = world.Create(new DamageOverTimeAttack(target.Reference(), 10,
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(0.3)));
        
        system.Tick(new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1.2), 1),
            entity, ref world.Get<DamageOverTimeAttack>(entity));

        var attack = world.Get<DamageOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(8.8), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0.5), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.IsFalse(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void DontHitWhenIntervalIsNotElapsed()
    {
        var entity = world.Create(new DamageOverTimeAttack(target.Reference(), 10,
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(0.3)));
        
        system.Tick(new Time(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(0.6), 1),
            entity, ref world.Get<DamageOverTimeAttack>(entity));

        var attack = world.Get<DamageOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(9.4), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0.9), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        CollectionAssert.IsEmpty(damages);
        Assert.IsFalse(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void HitManyTimesBeforeExpiring()
    {
        var entity = world.Create(new DamageOverTimeAttack(target.Reference(), 10,
            TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(0)));
        
        system.Tick(new Time(TimeSpan.FromSeconds(100), TimeSpan.FromSeconds(10), 1),
            entity, ref world.Get<DamageOverTimeAttack>(entity));

        var attack = world.Get<DamageOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(10), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(2, damages.Count);
        Assert.IsFalse(world.Has<DestroyEntitySchedule>(entity));
    }
    
    [Test]
    public void HitBeforeExpiringAfterMultipleTicks()
    {
        var entity = world.Create(new DamageOverTimeAttack(target.Reference(), 10,
            TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(0)));
        
        system.Tick(new Time(TimeSpan.FromSeconds(100), TimeSpan.FromSeconds(5), 1),
            entity, ref world.Get<DamageOverTimeAttack>(entity));
        system.Tick(new Time(TimeSpan.FromSeconds(105), TimeSpan.FromSeconds(5), 1),
            entity, ref world.Get<DamageOverTimeAttack>(entity));
        system.Tick(new Time(TimeSpan.FromSeconds(110), TimeSpan.FromSeconds(5), 1),
            entity, ref world.Get<DamageOverTimeAttack>(entity));

        var attack = world.Get<DamageOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(5), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(5), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.IsFalse(world.Has<DestroyEntitySchedule>(entity));
    }
    
    [Test]
    public void HitAndExpire()
    {
        var entity = world.Create(new DamageOverTimeAttack(target.Reference(), 10,
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.3)));
        
        system.Tick(new Time(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 1),
            entity, ref world.Get<DamageOverTimeAttack>(entity));

        var attack = world.Get<DamageOverTimeAttack>(entity);
        Assert.AreEqual(TimeSpan.FromSeconds(0), attack.Remaining);
        Assert.AreEqual(TimeSpan.FromSeconds(0.3), attack.Elapsed);
        var damages = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(entity));
    }
}