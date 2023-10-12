using System.Numerics;

namespace Lorux0r.RPG.Core;

public record Profile(string Id, string Name, float CurrentHealth, float MaxHealth, Vector3 Position);