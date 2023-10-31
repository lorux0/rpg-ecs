using UnityEngine;

namespace Lorux0r.RPG.Core.ECS;

public interface ICharacterPhysics
{
    bool IsGrounded { get; }
    void Move(Vector3 velocity);
}