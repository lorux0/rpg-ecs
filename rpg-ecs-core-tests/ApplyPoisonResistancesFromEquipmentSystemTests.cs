using Arch.Core;
using Lorux0r.RPG.Core.ECS;
using NUnit.Framework;

namespace Lorux0r.Core.Tests;

public class ApplyPoisonResistancesFromEquipmentSystemTests
{
    private World world = null!;
    private ApplyResistancesFromEquipmentSystem system = null!;
    private Entity warrior;

    [SetUp]
    public void BeforeEachTest()
    {
        world = World.Create();
        system = new ApplyResistancesFromEquipmentSystem(world);
        system.Initialize();
        warrior = world.Create(new Health(100, 100));
    }

    [TearDown]
    public void AfterEachTest()
    {
        system.Dispose();
        World.Destroy(world);
    }

    [Test]
    public void ApplyResistances()
    {
        world.Create(new Equipment(world.Reference(warrior), true), new PoisonResistance(0.1f));
        world.Create(new Equipment(world.Reference(warrior), false), new PoisonResistance(0.1f));
        world.Create(new Equipment(world.Reference(warrior), true), new PoisonResistance(0.2f));
        
        system.Update();
        system.Update();

        var resistance = world.Get<PoisonResistance>(warrior);
        Assert.AreEqual(0.3f, resistance.Ratio);
    }
}