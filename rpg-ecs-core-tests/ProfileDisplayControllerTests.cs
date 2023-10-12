using System;
using System.Numerics;
using Arch.Core;
using Lorux0r.RPG.Core;
using Lorux0r.RPG.Core.ECS;
using NSubstitute;
using NUnit.Framework;
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
    }

    [TearDown]
    public void AfterEachTest()
    {
        controller.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void ApplyChangesFromECSToUI()
    {
        var wizard = world.Create(new Health(20, 50),
            new ECSProfile("wiz", "Wizard"),
            new Position(new Vector3(1, 5, 7)));
        var hunter = world.Create(new Health(30, 70),
            new ECSProfile("hunt", "Hunter"),
            new Position(new Vector3(8, 3, 5)));

        system.BroadcastToUI(wizard, ref world.Get<ECSProfile>(wizard), ref world.Get<Health>(wizard), ref world.Get<Position>(wizard));
        system.BroadcastToUI(hunter, ref world.Get<ECSProfile>(hunter), ref world.Get<Health>(hunter), ref world.Get<Position>(hunter));
        
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
        var hunter = world.Create(new Health(30, 70),
            new ECSProfile("hunt", "Hunter"),
            new Position(new Vector3(8, 3, 5)));

        view.ChangeNameRequested += Raise.Event<Action<string, string>>("wiz", EXPECTED_NAME);

        // ECSProfileAntiCorruptionLayer creates an entity with UpdateProfileRequest component
        // So we need to query the world and then update the system with it so the request is processed
        // Probably there is a better way of doing it
        world.Query(in new QueryDescription().WithAll<UpdateProfileRequest>(),
            (in Entity entity, ref UpdateProfileRequest request) =>
            {
                system.UpdateProfileFromUI(entity, ref request);
            });

        // Get the profile ECS state
        ECSProfile updatedProfile;
        world.Query(in new QueryDescription().WithAll<ECSProfile>(), (in Entity entity, ref ECSProfile profile) =>
        {
            // TODO: is there any way of adding the id filter into the query?
            if (profile.Id == "wiz")
                updatedProfile = profile;
        });
        
        Assert.AreEqual(EXPECTED_NAME, updatedProfile.Name);
    }
}