namespace Lorux0r.RPG.Core.ECS;

public struct Movable
{
    public float Velocity { get; }
    
    public Movable(float velocity)
    {
        Velocity = velocity;
    }
}