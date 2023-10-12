using System.Numerics;
using Arch.Core;
using Arch.System;
using Lorux0r.RPG.Console;
using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;
using ECSProfile = Lorux0r.RPG.Core.ECS.Profile;

const int TICK_HZ = 30;

var world = World.Create();
world.Create(new Health(50, 50), new ECSProfile(Guid.NewGuid().ToString(), "Wizard"), new Position(Vector3.Zero));
world.Create(new Health(70, 70), new ECSProfile(Guid.NewGuid().ToString(), "Hunter"), new Position(new Vector3(1, 0, 0)));
world.Create(new Health(100, 100), new ECSProfile(Guid.NewGuid().ToString(), "Warrior"), new Position(new Vector3(2, 0, 0)));

// TODO: how to group all systems that do not require any custom parameter?
var timedSystems = new Group<Time>(
    new DamageOverTimeSystem(world)
);

var interval = TimeSpan.FromMilliseconds(1000D / TICK_HZ);
var timer = new PeriodicTimer(interval);
var cancellationToken = new CancellationTokenSource();
var lastUpdatedTimestamp = DateTime.UtcNow;
var time = new Time(TimeSpan.Zero, TimeSpan.Zero, 1);

var damageDisplayUi = new ProfileDisplayController(new ConsoleProfileDisplayView(), new ECSProfileAntiCorruptionLayer());

timedSystems.Initialize();

while (await timer.WaitForNextTickAsync() && !cancellationToken.IsCancellationRequested)
{
    // TODO: time calculation was in a system in the first place, but since its a single instance in ecs, like a singleton, how to ease access?
    var now = DateTime.UtcNow;
    var elapsed = time.Elapsed;
    var delta = (now - lastUpdatedTimestamp) * time.Scale;
    time.Delta = delta;
    time.Elapsed = elapsed.Add(delta);
    lastUpdatedTimestamp = now;
    
    try
    {
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
damageDisplayUi.Dispose();