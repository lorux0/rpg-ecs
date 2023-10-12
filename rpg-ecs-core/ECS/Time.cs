namespace Lorux0r.RPG.Core.ECS;

public struct Time
{
    // TODO: TimeSpan could be just floats for performance purposes, but i liked it fancy for this example :D 
    public TimeSpan Elapsed { get; set; }
    public TimeSpan Delta { get; set; }
    public float Scale { get; set; }

    public Time(TimeSpan elapsed, TimeSpan delta, float scale)
    {
        Elapsed = elapsed;
        Delta = delta;
        Scale = scale;
    }
}