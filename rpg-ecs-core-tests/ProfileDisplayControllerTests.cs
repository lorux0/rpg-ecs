using System;
using Arch.Core;
using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;
using Lorux0r.RPG.Core.ECS.Combat;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ECSProfile = Lorux0r.RPG.Core.ECS.Profile;

namespace Lorux0r.Core.Tests;

public class ProfileDisplayControllerTests
{
    private World world = null!;
    private IProfileDisplayView view = null!;
    private ProfileDisplayController controller = null!;
    private UpdateProfileToUISystem system = null!;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        view = Substitute.For<IProfileDisplayView>();
        var profileBridge = new ECSProfileAntiCorruptionLayer(world);
        controller = new ProfileDisplayController(view, profileBridge);
        system = new UpdateProfileToUISystem(world, profileBridge);
        system.Initialize();
    }

    [TearDown]
    public void AfterEachTest()
    {
        controller.Dispose();
        system.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void ApplyChangesFromECSToUI()
    {
        world.Create(new Health(20, 50),
            new ECSProfile("wiz", "Wizard"),
            new Position(new Vector3(1, 5, 7)));
        world.Create(new Health(30, 70),
            new ECSProfile("hunt", "Hunter"),
            new Position(new Vector3(8, 3, 5)));

        system.Update();

        view.Received(1).Show("wiz", "Wizard", 20, 50);
        view.Received(1).Show("hunt", "Hunter", 30, 70);
    }

    [Test]
    public void ApplyChangesFromUIToECS()
    {
        const string EXPECTED_NAME = "Gandalf";

        var wizard = world.Create(new Health(20, 50),
            new ECSProfile("wiz", "Wizard"),
            new Position(new Vector3(1, 5, 7)));
        world.Create(new Health(30, 70),
            new ECSProfile("hunt", "Hunter"),
            new Position(new Vector3(8, 3, 5)));

        view.ChangeNameRequested += Raise.Event<Action<string, string>>("wiz", EXPECTED_NAME);

        system.Update();

        Assert.AreEqual(EXPECTED_NAME, world.Get<ECSProfile>(wizard).Name);
    }
}
