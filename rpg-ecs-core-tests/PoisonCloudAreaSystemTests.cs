using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Lorux0r.RPG.Core.ECS;
using Lorux0r.RPG.Core.ECS.Combat;
using Lorux0r.RPG.Core.ECS.Combat.Poison;
using NUnit.Framework;
using UnityEngine;
using Time = Lorux0r.RPG.Core.ECS.Time;

namespace Lorux0r.Core.Tests;

public class PoisonCloudAreaSystemTests
{
    private World world = null!;
    private PoisonCloudAreaSystem system = null!;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new PoisonCloudAreaSystem(world);
        system.Initialize();
    }

    [TearDown]
    public void AfterEachTest()
    {
        system.Dispose();
    }
    
    [Test]
    public void ExecuteMultiplePoisonDamageOverTimeWhenNoTeamIsAssigned()
    {
        world.Create(new PoisonCloudArea(5, 1, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.Zero),
            new Position(new Vector3(7, 3, 9)));

        world.Create(new Team(1 << 2),
            new Position(new Vector3(6, 3, 7)),
            new Health(20, 100));
        
        world.Create(new Position(new Vector3(6, 3, 7)),
            new Health(20, 100));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), 1);
        system.Update(in time);
        system.Update(in time);
        system.Update(in time);

        var damageEntities = new List<Entity>();
        world.GetEntities(new QueryDescription().WithAll<PoisonDamage>(), damageEntities);
        Assert.AreEqual(6, damageEntities.Count);
        Assert.IsTrue(damageEntities.TrueForAll(entity => world.Get<PoisonDamage>(entity).Damage.Equals(1f)));
    }
    
    [Test]
    public void ExecuteDamageToMultipleEnemiesInRange()
    {
        world.Create(new PoisonCloudArea(5, 1, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.Zero),
            new Position(new Vector3(7, 3, 9)),
            new Team(1 << 1));

        world.Create(new Team(1 << 2),
            new Position(new Vector3(6, 3, 7)),
            new Health(20, 100));
        
        world.Create(new Team(1 << 2),
            new Position(new Vector3(6, 4, 7)),
            new Health(20, 100));
        
        world.Create(new Team(1 << 2),
            new Position(new Vector3(7, 3, 7)),
            new Health(20, 100));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), 1);
        system.Update(in time);
        system.Update(in time);
        system.Update(in time);

        var damageEntities = new List<Entity>();
        world.GetEntities(new QueryDescription().WithAll<PoisonDamage>(), damageEntities);
        Assert.AreEqual(9, damageEntities.Count);
        Assert.IsTrue(damageEntities.TrueForAll(entity => world.Get<PoisonDamage>(entity).Damage.Equals(1f)));
    }

    [Test]
    public void ExecuteMultiplePoisonDamageOverTimeWhenIsEnemy()
    {
        world.Create(new PoisonCloudArea(5, 1, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.Zero),
            new Position(new Vector3(7, 3, 9)),
            new Team(1 << 1));

        world.Create(new Team(1 << 2),
            new Position(new Vector3(6, 3, 7)),
            new Health(20, 100));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), 1);
        system.Update(in time);
        system.Update(in time);
        system.Update(in time);

        var damageEntities = new List<Entity>();
        world.GetEntities(new QueryDescription().WithAll<PoisonDamage>(), damageEntities);
        Assert.AreEqual(3, damageEntities.Count);
        Assert.IsTrue(damageEntities.TrueForAll(entity => world.Get<PoisonDamage>(entity).Damage.Equals(1f)));
    }
    
    [Test]
    public void DontDoDamageWhenAreAllies()
    {
        world.Create(new PoisonCloudArea(5, 1, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.Zero),
            new Position(new Vector3(7, 3, 9)),
            new Team(1 << 1));

        world.Create(new Team(1 << 1),
            new Position(new Vector3(6, 3, 7)),
            new Health(20, 100));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), 1);
        system.Update(in time);
        system.Update(in time);
        system.Update(in time);

        ThenNoDamageIsApplied();
    }
    
    [Test]
    public void DontDoDamageWhenTimeIsNotReached()
    {
        world.Create(new PoisonCloudArea(5, 1, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.Zero),
            new Position(new Vector3(7, 3, 9)),
            new Team(1 << 1));

        world.Create(new Team(1 << 1),
            new Position(new Vector3(6, 3, 7)),
            new Health(20, 100));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0.4), 1);
        system.Update(in time);

        ThenNoDamageIsApplied();
    }
    
    [Test]
    public void StopDoingDamageAfterDurationIsExpired()
    {
        world.Create(new PoisonCloudArea(5, 1, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.Zero),
            new Position(new Vector3(7, 3, 9)),
            new Team(1 << 1));

        world.Create(new Team(1 << 2),
            new Position(new Vector3(6, 3, 7)),
            new Health(20, 100));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), 1);
        system.Update(in time);
        system.Update(in time);
        system.Update(in time);

        var damageEntities = new List<Entity>();
        world.GetEntities(new QueryDescription().WithAll<PoisonDamage>(), damageEntities);
        Assert.AreEqual(2, damageEntities.Count);
    }

    [Test]
    public void DontDoDamageOnEnemiesOutOfRange()
    {
        world.Create(new PoisonCloudArea(5, 1, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.Zero),
            new Position(new Vector3(7, 3, 9)),
            new Team(1 << 1));

        world.Create(new Team(1 << 1),
            new Position(new Vector3(100, 3, 7)),
            new Health(20, 100));

        var time = new Time(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), 1);
        system.Update(in time);

        ThenNoDamageIsApplied();
    }

    private void ThenNoDamageIsApplied()
    {
        var damageEntities = new List<Entity>();
        world.GetEntities(new QueryDescription().WithAll<PoisonDamage>(), damageEntities);
        Assert.AreEqual(0, damageEntities.Count);
    }
}