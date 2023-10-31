using Lorux0r.RPG.Core.ECS;
using UnityEngine;

namespace Lorux0r.RPG.Console;

public class DummyCharacterPhysics : ICharacterPhysics
{
    private Vector3 position = Vector3.zero;
    
    public bool IsGrounded => true;
    
    public void Move(Vector3 velocity)
    {
        position += velocity;
    }
}