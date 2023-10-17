using Arch.Core;

namespace Lorux0r.RPG.Core.ECS;

public class DamageOverTimeSystem : OverTimeActionSystem<DamageOverTimeAttack>
{
    public DamageOverTimeSystem(World world) : base(world)
    {
    }

    protected override void Execute(in Time time, in Entity entity, DamageOverTimeAttack action)
    {
        World.Create(new Damage(action.Target, action.Attacker, action.Damage));
    }
}