using Lorux0r.RPG.Core.ECS;
using UnityEngine;

namespace Lorux0r.RPG.Console;

public class UnityCharacterInputProvider : ICharacterInputProvider
{
    public Vector2 GetMovementAxis() => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
}