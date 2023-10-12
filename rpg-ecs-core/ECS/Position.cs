using System.Numerics;

namespace Lorux0r.RPG.Core.ECS;

public struct Position
{
    public Vector3 Current { get; }

    public Position(Vector3 current)
    {
        Current = current;
    }
}