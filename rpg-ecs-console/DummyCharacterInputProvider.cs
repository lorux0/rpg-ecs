using Lorux0r.RPG.Core.ECS;
using UnityEngine;

namespace Lorux0r.RPG.Console;

public class DummyCharacterInputProvider : ICharacterInputProvider
{
    public Vector2 GetMovementAxis() => Vector2.zero;
}