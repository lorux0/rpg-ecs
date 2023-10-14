using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Lorux0r.RPG.Console;
using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;
using ECSProfile = Lorux0r.RPG.Core.ECS.Profile;

const int TICK_HZ = 1;

var world = World.Create();
world.Create(new Time(TimeSpan.Zero, TimeSpan.Zero, 1));
world.Create(new Health(50, 50),
    new ECSProfile(Guid.NewGuid().ToString(), "Wizard"),
    new Position(Vector3.Zero));
world.Create(new Health(70, 70),
    new ECSProfile(Guid.NewGuid().ToString(), "Hunter"),
    new Position(new Vector3(1, 0, 0)));
var warrior = world.Create(new Health(100, 100),
    new ECSProfile(Guid.NewGuid().ToString(), "Warrior"),
    new Position(new Vector3(2, 0, 0)));
world.Create(new DamageOverTimeAttack(warrior.Reference(), 5, 
    TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.Zero));

var timedSystems = new Group<Time>(
    new DamageOverTimeSystem(world)
);

var ecsProfileGateway = new ECSProfileAntiCorruptionLayer(world);
var simpleSystems = new ISimpleSystem[]
{
    new TimeUpdaterSystem(world, new SystemDateProvider()),
    new DamageSystem(world),
    new RangedAttackSystem(world),
    new UpdateProfileToUISystem(world, ecsProfileGateway),
    new DestroyEntitiesSystem(world),
};

var interval = TimeSpan.FromMilliseconds(1000D / TICK_HZ);
var timer = new PeriodicTimer(interval);
var cancellationToken = new CancellationTokenSource();

var profileDisplayUI = new ProfileDisplayController(new ConsoleProfileDisplayView(), ecsProfileGateway);

timedSystems.Initialize();

while (await timer.WaitForNextTickAsync() && !cancellationToken.IsCancellationRequested)
{
    try
    {
        foreach (var system in simpleSystems)
            system.Update();
        
        var time = world.GetTime();
        
        timedSystems.BeforeUpdate(time);
        timedSystems.Update(time);
        timedSystems.AfterUpdate(time);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
}

timedSystems.Dispose();
profileDisplayUI.Dispose();