using Arch.Core;
using Arch.System;
using Lorux0r.RPG.Core.ECS.Combat;

namespace Lorux0r.RPG.Core.ECS.Itemization;

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
    [All(typeof(Corpse))]
    [None(typeof(ItemDropResult))]
    private void Execute(in Entity entity, ref ItemDropTags dropTags, ref Position position)
    {
        var tags = dropTags.Tags;
        var drops = new List<ItemId>(dropTags.Iterations);
        
        for (var i = 0; i < dropTags.Iterations; i++)
        {
            var dropItemId = SolveDrop(tags);
            if (!dropItemId.HasValue) continue;
            world.Create(new ItemDrop(world.Reference(entity), dropItemId.Value),
                new Position(position.Current),
                new Collectable(null));
            drops.Add(dropItemId.Value);
        }
        
        world.Add(entity, new ItemDropResult(drops));
    }

    private ItemId? SolveDrop(IEnumerable<string> tags)
    {
        foreach (var tag in tags)
        {
            var dropItemId = SolveDrop(tag);
            
            if (dropItemId.HasValue)
                return dropItemId;
        }

        return null;
    }

    private ItemId? SolveDrop(string tag)
    {
        var drops = itemDropTableProvider.GetByTag(tag);
        var random = randomGenerator.NextDouble();
        double weight = 0;

        foreach (var drop in drops)
        {
            weight += drop.Ratio;

            if (random <= weight)
                return drop.ItemId;
        }

        return null;
    }
}