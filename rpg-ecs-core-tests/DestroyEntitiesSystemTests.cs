using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class DestroyEntitiesSystemTests
{
    private DestroyEntitiesSystem system = null!;
    private World world = null!;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new DestroyEntitiesSystem(world);
        system.Initialize();
    }

    [TearDown]
    public void AfterEachTest()
    {
        system.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void DestroyAllScheduledEntities()
    {
        world.Create(new DestroyEntitySchedule());
        world.Create(new DestroyEntitySchedule());
        world.Create(new DestroyEntitySchedule());

        system.Update();

        var entities = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<DestroyEntitySchedule>(), entities);
        CollectionAssert.IsEmpty(entities);
    }
}