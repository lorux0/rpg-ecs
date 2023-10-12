namespace Lorux0r.RPG.Core;

public class SystemDateProvider : ICurrentDateProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}