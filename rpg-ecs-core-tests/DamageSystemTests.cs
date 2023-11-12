using System.Linq;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using Lorux0r.RPG.Core.ECS.Combat;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class DamageSystemTests
{
    private World world = null!;
    private DamageSystem system = null!;
    private Entity target;
    private Entity attacker;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new DamageSystem(world);
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
    public void HitOnce()
    {
        var entity = world.Create(new Damage(target.Reference(), attacker.Reference(), 10));

        system.Update();

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
            world.Create(new Damage(target.Reference(), attacker.Reference(), 10)),
            world.Create(new Damage(target.Reference(), attacker.Reference(), 5)),
            world.Create(new Damage(target.Reference(), attacker.Reference(), 30))
        };
        
        system.Update();

        var health = world.Get<Health>(target);
        Assert.AreEqual(55, health.Current);
        Assert.IsFalse(health.IsDead);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }

    [Test]
    public void Kill()
    {
        var entity = world.Create(new Damage(target.Reference(), attacker.Reference(), 130));

        system.Update();

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
            world.Create(new Damage(target.Reference(), attacker.Reference(), 30)),
            world.Create(new Damage(target.Reference(), attacker.Reference(), 50)),
            world.Create(new Damage(target.Reference(), attacker.Reference(), 20))
        };

        system.Update();

        var health = world.Get<Health>(target);
        Assert.AreEqual(0, health.Current);
        Assert.IsTrue(health.IsDead);
        Assert.IsTrue(entities.All(entity => world.Has<DestroyEntitySchedule>(entity)));
    }
}