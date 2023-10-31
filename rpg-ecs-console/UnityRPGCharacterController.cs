using Lorux0r.RPG.Core.ECS;
using UnityEngine;

namespace Lorux0r.RPG.Console;

public class UnityRPGCharacterController : MonoBehaviour, ICharacterPhysics
{
    [SerializeField] private CharacterController characterController = null!;

    public bool IsGrounded => characterController.isGrounded;
    
    public void Move(Vector3 velocity)
    {
        characterController.Move(velocity);
    }
}