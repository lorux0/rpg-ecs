using Arch.Core;

namespace Lorux0r.RPG.Core.ECS.Combat.Poison;

public class PoisonOverTimeAttackSystem : OverTimeActionSystem<PoisonOverTimeAttack>
{
    public PoisonOverTimeAttackSystem(World world) : base(world)
    {
    }

    protected override void Execute(in Time time, in Entity entity, PoisonOverTimeAttack action)
    {
        World.Create(new PoisonDamage(action.Target, action.Attacker, action.Damage));
    }
}