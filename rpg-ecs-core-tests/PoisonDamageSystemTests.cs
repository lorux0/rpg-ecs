using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class PoisonDamageSystemTests
{
    private World world = null!;
    private Entity wizard;
    private Entity warrior;
    private PoisonDamageSystem system = null!;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        wizard = world.Create(new Health(50, 50));
        warrior = world.Create(new Health(100, 100), new PoisonResistance(0.1f));
        system = new PoisonDamageSystem(world);
        system.Initialize();
    }

    [TearDown]
    public void AfterEachTest()
    {
        system.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void ApplyDamageWithoutResistance()
    {
        var poison = world.Create(new PoisonDamage(world.Reference(wizard), 10));

        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.AreEqual(10, world.Get<Damage>(damages[0]).Value);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(poison));
    }
    
    [Test]
    public void ApplyDamageWithResistance()
    {
        var poison = world.Create(new PoisonDamage(world.Reference(warrior), 10));

        system.Update();

        var damages = new List<Entity>();
        world.GetEntities(new QueryDescription().WithAll<Damage>(), damages);
        Assert.AreEqual(1, damages.Count);
        Assert.AreEqual(9, world.Get<Damage>(damages[0]).Value);
        Assert.IsTrue(world.Has<DestroyEntitySchedule>(poison));
    }
}