using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class TimeUpdaterSystem : ISimpleSystem
{
    private readonly World world;
    private readonly ICurrentDateProvider dateProvider;
    private DateTime lastUpdatedTimestamp;

    public TimeUpdaterSystem(World world,
        ICurrentDateProvider dateProvider)
    {
        this.world = world;
        this.dateProvider = dateProvider;
        lastUpdatedTimestamp = dateProvider.UtcNow;
    }

    public void Update()
    {
        TickQuery(world);
    }

    [Query]
    [All(typeof(Time))]
    private void Tick(in Entity entity, ref Time time)
    {
        var now = DateTime.UtcNow;
        var elapsed = time.Elapsed;
        var delta = (now - lastUpdatedTimestamp) * time.Scale;
        time.Delta = delta;
        time.Elapsed = elapsed.Add(delta);
        lastUpdatedTimestamp = now;
    }
}