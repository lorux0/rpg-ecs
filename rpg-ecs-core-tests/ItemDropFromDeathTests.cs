using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using Lorux0r.RPG.Core.ECS.Combat;
using Lorux0r.RPG.Core.ECS.Itemization;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace Lorux0r.Core.Tests;

public class ItemDropFromDeathTests
{
    private const string GUARANTEED_DROP_TABLE = "guaranteed";
    private const string NOTHING_DROP_TABLE = "guaranteed";
    private const string ITEM_ID_RUNE_EL = "rune_el";

    private CreateItemDropOnDeathSystem system = null!;
    private World world = null!;
    private IItemDropTableProvider itemDropTableProvider = null!;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        itemDropTableProvider = Substitute.For<IItemDropTableProvider>();
        itemDropTableProvider.GetByTag(GUARANTEED_DROP_TABLE).Returns(new[]
        {
            new ItemDropChance(ITEM_ID_RUNE_EL, 1)
        });
        itemDropTableProvider.GetByTag(NOTHING_DROP_TABLE).Returns(new[]
        {
            new ItemDropChance(ITEM_ID_RUNE_EL, 0)
        });
        system = new CreateItemDropOnDeathSystem(world, new Random(98), itemDropTableProvider);
        system.Initialize();
    }

    [TearDown]
    public void AfterEachTest()
    {
        system.Dispose();
    }

    [Test]
    public void DropOneItem()
    {
        var enemyEntity = world.Create(new Health(0, 100),
            new ItemDropTags(new[] {GUARANTEED_DROP_TABLE}, 1),
            new Position(new Vector3(5, 3, 2)));

        system.Update();

        var entities = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<ItemDrop, Position, Collectable>(), entities);
        var dropEntity = entities[0];
        var drop = world.Get<ItemDrop>(dropEntity);
        var position = world.Get<Position>(dropEntity);
        var collectable = world.Get<Collectable>(dropEntity);
        Assert.AreEqual(1, entities.Count);
        Assert.AreEqual(ITEM_ID_RUNE_EL, drop.ItemId);
        Assert.AreEqual(enemyEntity, drop.DroppedBy.Entity);
        Assert.AreEqual(new Vector3(5, 3, 2), position.Current);
        Assert.AreEqual(false, collectable.Collected);
        Assert.IsNull(collectable.CollectedBy);

        entities = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<ItemDropProcessed>(), entities);
        Assert.AreEqual(enemyEntity, entities[0]);
        Assert.AreEqual(1, entities.Count);
    }

    [Test]
    public void DropMultipleItems()
    {
        var enemyEntity = world.Create(new Health(0, 100),
            new ItemDropTags(new[] {GUARANTEED_DROP_TABLE}, 2),
            new Position(new Vector3(5, 3, 2)));

        system.Update();

        var entities = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<ItemDrop, Position, Collectable>(), entities);
        
        Assert.AreEqual(2, entities.Count);

        foreach (var dropEntity in entities)
        {
            var drop = world.Get<ItemDrop>(dropEntity);
            var position = world.Get<Position>(dropEntity);
            var collectable = world.Get<Collectable>(dropEntity);
            
            Assert.AreEqual(ITEM_ID_RUNE_EL, drop.ItemId);
            Assert.AreEqual(enemyEntity, drop.DroppedBy.Entity);
            Assert.AreEqual(new Vector3(5, 3, 2), position.Current);
            Assert.AreEqual(false, collectable.Collected);
            Assert.IsNull(collectable.CollectedBy);
        }

        entities = new List<Entity>();
        world.GetEntities(in new QueryDescription().WithAll<ItemDropProcessed>(), entities);
        Assert.AreEqual(enemyEntity, entities[0]);
        Assert.AreEqual(1, entities.Count);
    }

    [Test]
    public void SkipDropsWhenAlreadyProcessed()
    {
        world.Create(new Health(0, 100),
            new ItemDropTags(new[] {GUARANTEED_DROP_TABLE}, 1),
            new Position(new Vector3(5, 3, 2)),
            new ItemDropProcessed());

        system.Update();

        ThenNoDropsHasBeenGenerated();
    }

    [Test]
    public void SkipDropsWhenIsAlive()
    {
        world.Create(new Health(1, 100),
            new ItemDropTags(new[] {GUARANTEED_DROP_TABLE}, 1),
            new Position(new Vector3(5, 3, 2)));

        system.Update();

        ThenNoDropsHasBeenGenerated();
    }

    [Test]
    public void DropNothingWhenOutOfChance()
    {
        world.Create(new Health(0, 100),
            new ItemDropTags(new[] {NOTHING_DROP_TABLE}, 10),
            new Position(new Vector3(5, 3, 2)));

        system.Update();
        
        ThenNoDropsHasBeenGenerated();
    }

    private void ThenNoDropsHasBeenGenerated()
    {
        Assert.AreEqual(0, world.CountEntities(in new QueryDescription().WithAny<ItemDrop>()));
    }
}