using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS.Combat;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class PhysicalDamageSystemTests
{
    private World world = null!;
    private PhysicalDamageSystem system = null!;
    private Entity target;
    private List<PhysicalDamage> targetAccumulatedDamage;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new PhysicalDamageSystem(world);
        targetAccumulatedDamage = new List<PhysicalDamage>();
        target = world.Create(new Health(100, 100), new AccumulatedPhysicalDamage(targetAccumulatedDamage));
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
        targetAccumulatedDamage.Add(new PhysicalDamage(10));

        system.Update();

        var health = world.Get<Health>(target);
        Assert.AreEqual(90, health.Current);
        Assert.IsFalse(health.IsDead);
    }

    [Test]
    public void HitMultipleTimes()
    {
        targetAccumulatedDamage.Add(new PhysicalDamage(10));
        targetAccumulatedDamage.Add(new PhysicalDamage(5));
        targetAccumulatedDamage.Add(new PhysicalDamage(30));
        
        system.Update();

        var health = world.Get<Health>(target);
        Assert.AreEqual(55, health.Current);
        Assert.IsFalse(health.IsDead);
    }

    [Test]
    public void Kill()
    {
        targetAccumulatedDamage.Add(new PhysicalDamage(130));

        system.Update();

        var health = world.Get<Health>(target);
        Assert.AreEqual(0, health.Current);
        Assert.IsTrue(health.IsDead);
    }

    [Test]
    public void KillAfterMultipleHits()
    {
        targetAccumulatedDamage.Add(new PhysicalDamage(30));
        targetAccumulatedDamage.Add(new PhysicalDamage(50));
        targetAccumulatedDamage.Add(new PhysicalDamage(20));

        system.Update();

        var health = world.Get<Health>(target);
        Assert.AreEqual(0, health.Current);
        Assert.IsTrue(health.IsDead);
    }
}