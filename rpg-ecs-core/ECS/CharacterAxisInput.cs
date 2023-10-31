using UnityEngine;

namespace Lorux0r.RPG.Core.ECS;

public struct CharacterAxisInput
{
    public Vector2 Axis { get; set; }
    
    public CharacterAxisInput(Vector2 axis)
    {
        Axis = axis;
    }
}