using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Lorux0r.RPG.Console;
using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;
using UnityEngine;
using ECSProfile = Lorux0r.RPG.Core.ECS.Profile;
using Random = System.Random;
using Time = Lorux0r.RPG.Core.ECS.Time;

const int TICK_HZ = 1;

var world = World.Create();
world.Create(new Time(TimeSpan.Zero, TimeSpan.Zero, 1));
// Characters
var wizard = world.Create(new Health(50, 50),
    new ECSProfile(Guid.NewGuid().ToString(), "Wizard"),
    new Position(Vector3.zero),
    new Movable(1),
    (ICharacterPhysics) new DummyCharacterPhysics(),
    new CharacterAxisInput());
var hunter = world.Create(new Health(70, 70),
    new ECSProfile(Guid.NewGuid().ToString(), "Hunter"),
    new Position(new Vector3(7, 0, 0)),
    new Movable(2),
    (ICharacterPhysics) new DummyCharacterPhysics(),
    new ItemDropTags(new[] {"rune_t1", "rune_t2"}, 1));
var warrior = world.Create(new Health(100, 100),
    new ECSProfile(Guid.NewGuid().ToString(), "Warrior"),
    new Position(new Vector3(2, 0, 0)),
    new Movable(1.5f),
    (ICharacterPhysics) new DummyCharacterPhysics());
// Equipment
world.Create(new Equipment(warrior.Reference(), true), new PoisonResistance(0.1f));
world.Create(new Equipment(hunter.Reference(), true), new DamageOffRangeAmplifier(1, 1.1f));
// Attacks
world.Create(new DamageOverTimeAttack(wizard.Reference(), hunter.Reference(), 5,
    TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.Zero));
world.Create(new PoisonOverTimeAttack(warrior.Reference(), hunter.Reference(), 10,
    TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.Zero));

var timedSystems = new Group<Time>(
    new DamageOverTimeSystem(world),
    new PoisonOverTimeAttackSystem(world),
    new ApplyCharacterMovementInputToPhysicsSystem(world)
);

var ecsProfileGateway = new ECSProfileAntiCorruptionLayer(world);
var simpleSystems = new ISimpleSystem[]
{
    new TimeUpdaterSystem(world, new SystemDateProvider()),
    new ApplyMovementInputFromInputSystem(world, new DummyCharacterInputProvider()),
    new ApplyResistancesFromEquipmentSystem(world),
    new ApplyDamageOffRangeFromEquipmentSystem(world),
    new RangedAttackSystem(world),
    new DamageOffRangeAmplifierSystem(world),
    new PoisonDamageSystem(world),
    new DamageSystem(world),
    new CreateItemDropOnDeathSystem(world, Random.Shared, new HardcodedItemDropTableProvider()),
    new UpdateProfileToUISystem(world, ecsProfileGateway),
    new DestroyEntitiesSystem(world),
};

var interval = TimeSpan.FromMilliseconds(1000D / TICK_HZ);
var timer = new PeriodicTimer(interval);
var cancellationToken = new CancellationTokenSource();

var profileDisplayUI = new ProfileDisplayController(new ConsoleProfileDisplayView(), ecsProfileGateway);

foreach (var system in simpleSystems)
    system.Initialize();

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

foreach (var system in simpleSystems)
    system.Dispose();

timedSystems.Dispose();
profileDisplayUI.Dispose();