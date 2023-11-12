using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class CreateItemDropOnDeathSystem : ISimpleSystem
{
    private readonly World world;
    private readonly Random randomGenerator;
    private readonly IItemDropTableProvider itemDropTableProvider;

    public CreateItemDropOnDeathSystem(World world,
        Random random,
        IItemDropTableProvider itemDropTableProvider)
    {
        this.world = world;
        randomGenerator = random;
        this.itemDropTableProvider = itemDropTableProvider;
    }

    public void Dispose()
    {
    }

    public void Initialize()
    {
    }

    public void Update()
    {
        ExecuteQuery(world);
    }

    [Query]
    [All(typeof(Health), typeof(ItemDropTags), typeof(Position))]
    [None(typeof(ItemDropProcessed))]
    private void Execute(in Entity entity, ref Health health, ref ItemDropTags dropTags, ref Position position)
    {
        if (!health.IsDead) return;
        var tags = dropTags.Tags;
        
        for (var i = 0; i < dropTags.Iterations; i++)
        {
            var dropItemId = SolveDrop(tags);
            if (string.IsNullOrEmpty(dropItemId)) continue;
            world.Create(new ItemDrop(world.Reference(entity), dropItemId),
                new Position(position.Current),
                new Collectable(null));
        }
        
        world.Add<ItemDropProcessed>(entity);
    }

    private string? SolveDrop(IEnumerable<string> tags)
    {
        foreach (var tag in tags)
        {
            var dropItemId = SolveDrop(tag);

            if (!string.IsNullOrEmpty(dropItemId))
                return dropItemId;
        }

        return null;
    }

    private string? SolveDrop(string tag)
    {
        var drops = itemDropTableProvider.GetByTag(tag);
        var random = randomGenerator.NextDouble();
        double weight = 0;

        foreach (var drop in drops)
        {
            weight += drop.Weight;

            if (random <= weight)
                return drop.ItemId;
        }

        return null;
    }
}