namespace Lorux0r.RPG.Core;

public interface ICurrentDateProvider
{
    DateTime UtcNow { get; }
}