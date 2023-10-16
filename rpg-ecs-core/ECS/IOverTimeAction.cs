namespace Lorux0r.RPG.Core.ECS;

public interface IOverTimeAction
{
    TimeSpan Interval { get; }
    TimeSpan Remaining { get; set; }
    TimeSpan Elapsed { get; set; }
}