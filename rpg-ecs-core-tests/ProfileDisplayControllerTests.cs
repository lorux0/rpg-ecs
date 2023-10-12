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
        var profileBridge = new ECSProfileAntiCorruptionLayer();
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
    public void BroadcastToUI()
    {
        var wizard = world.Create(new Health(20, 50), new ECSProfile("wiz", "Wizard"));
        var hunter = world.Create(new Health(30, 70), new ECSProfile("hunt", "Hunter"));

        system.BroadcastToUI(wizard, ref world.Get<ECSProfile>(wizard), ref world.Get<Health>(wizard));
        system.BroadcastToUI(hunter, ref world.Get<ECSProfile>(hunter), ref world.Get<Health>(hunter));
        
        view.Received(1).Show("wiz", "Wizard", 20, 50);
        view.Received(1).Show("hunt", "Hunter", 30, 70);
    }
}