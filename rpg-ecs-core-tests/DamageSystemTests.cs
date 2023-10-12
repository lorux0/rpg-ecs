using System.Linq;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class DamageSystemTests
{
    private World world = null!;
    private DamageSystem system = null!;
    private Entity target;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new DamageSystem(world);
        target = world.Create(new Health(100, 100));
    }

    [TearDown]
    public void AfterEachTest()
    {
        World.Destroy(world);
    }
    
    [Test]
    public void HitOnce()
    {
        var entity = world.Create(new Damage(target.Reference(), 10));

        system.Tick(in entity, ref world.Get<Damage>(entity));

        var health = world.Get<Health>(target);
        Assert.AreEqual(90, health.Current);
        Assert.IsFalse(health.IsDead);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void HitMultipleTimes()
    {
        var entities = new[]
        {
            world.Create(new Damage(target.Reference(), 10)),
            world.Create(new Damage(target.Reference(), 5)),
            world.Create(new Damage(target.Reference(), 30))
        };

        foreach (var entity in entities)
            system.Tick(entity, ref world.Get<Damage>(entity));

        var health = world.Get<Health>(target);
        Assert.AreEqual(55, health.Current);
        Assert.IsFalse(health.IsDead);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }

    [Test]
    public void Kill()
    {
        var entity = world.Create(new Damage(target.Reference(), 130));

        system.Tick(entity, ref world.Get<Damage>(entity));

        var health = world.Get<Health>(target);
        Assert.AreEqual(0, health.Current);
        Assert.IsTrue(health.IsDead);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(entity));
    }

    [Test]
    public void KillAfterMultipleHits()
    {
        var entities = new[]
        {
            world.Create(new Damage(target.Reference(), 30)),
            world.Create(new Damage(target.Reference(), 50)),
            world.Create(new Damage(target.Reference(), 20))
        };

        foreach (var entity in entities)
            system.Tick(entity, ref world.Get<Damage>(entity));

        var health = world.Get<Health>(target);
        Assert.AreEqual(0, health.Current);
        Assert.IsTrue(health.IsDead);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }
}