using Arch.Core;
using Arch.System;
using UnityEngine;

namespace Lorux0r.RPG.Core.ECS;

public partial class ApplyCharacterMovementInputToPhysicsSystem : BaseSystem<World, Time>
{
    public ApplyCharacterMovementInputToPhysicsSystem(World world) : base(world)
    {
    }
    
    [Query]
    [All(typeof(CharacterAxisInput), typeof(ICharacterPhysics), typeof(Movable))]
    private void Execute([Data] in Time time, ref CharacterAxisInput input, ref ICharacterPhysics physics,
        ref Movable movable)
    {
        if (!physics.IsGrounded) return;
        var velocity = input.Axis * movable.Velocity * (float) time.Delta.TotalSeconds;
        physics.Move(new Vector3(velocity.x, 0, velocity.y));
    }
}