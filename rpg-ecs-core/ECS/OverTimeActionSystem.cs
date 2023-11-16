using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public abstract class OverTimeActionSystem<T> : BaseSystem<World, Time> where T : IOverTimeAction
{
    private readonly QueryDescription queryDescription = new QueryDescription().WithAll<T>().WithNone<DestroyEntitySchedule>();
    
    public OverTimeActionSystem(World world) : base(world)
    {
    }

    public override void Update(in Time time)
    {
        base.Update(in time);

        var query = World.Query(in queryDescription);

        foreach (ref var chunk in query.GetChunkIterator())
        {
            ref var entityFirstElement = ref chunk.Entity(0);
            ref var actionFirstElement = ref chunk.GetFirst<T>();
            
            foreach (var entityIndex in chunk)
            {
                ref readonly var entity = ref Unsafe.Add(ref entityFirstElement, entityIndex);
                ref var action = ref Unsafe.Add(ref actionFirstElement, entityIndex);
                
                Tick(time, entity, ref action);
            }
        }
    }

    protected virtual void Tick(in Time time, in Entity entity, ref T action)
    {
        var delta = time.Delta > action.Remaining ? action.Remaining : time.Delta;
        
        action.Elapsed = action.Elapsed.Add(delta);
        action.Remaining = action.Remaining.Subtract(delta);

        while (action.Elapsed >= action.Interval)
        {
            action.Elapsed = action.Elapsed.Subtract(action.Interval);
            Execute(time, entity, action);
        }

        if (action.Remaining.Ticks <= 0)
            entity.FlagToDestroy(World);
    }

    protected abstract void Execute(in Time time, in Entity entity, T action);
}