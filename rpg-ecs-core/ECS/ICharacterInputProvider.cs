using UnityEngine;

namespace Lorux0r.RPG.Core.ECS;

public interface ICharacterInputProvider
{
    Vector2 GetMovementAxis();
}