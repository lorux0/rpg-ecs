using Arch.Core;

namespace Lorux0r.RPG.Core.ECS.Combat.Poison;

public class PoisonCloudAreaSystem : OverTimeActionSystem<PoisonCloudArea>
{
    private readonly World world;
    private readonly QueryDescription targetQueryByTeam = new QueryDescription().WithAll<Team, Position, Health>();
    private readonly QueryDescription targetQuery = new QueryDescription().WithAll<Position, Health>();

    public PoisonCloudAreaSystem(World world) : base(world)
    {
        this.world = world;
    }

    protected override void Execute(in Time time, in Entity entity, PoisonCloudArea action)
    {
        var cloudPosition = world.Get<Position>(entity);
        
        if (world.Has<Team>(entity))
        {
            var damage = new ExecuteDamageByTeam(world.Get<Team>(entity), cloudPosition, action, world.Reference(entity), world);
            world.InlineEntityQuery<ExecuteDamageByTeam, Team, Position, Health>(in targetQueryByTeam, ref damage);
        }
        else
        {
            var damage = new ExecuteDamage(cloudPosition, action, world.Reference(entity), world);
            world.InlineEntityQuery<ExecuteDamage, Position, Health>(in targetQuery, ref damage);
        }
    }
    
    private readonly struct ExecuteDamage : IForEachWithEntity<Position, Health>
    {
        private readonly Position position;
        private readonly PoisonCloudArea poisonCloudArea;
        private readonly EntityReference cloudEntity;
        private readonly World world;

        public ExecuteDamage(Position position,
            PoisonCloudArea poisonCloudArea,
            EntityReference cloudEntity,
            World world)
        {
            this.position = position;
            this.poisonCloudArea = poisonCloudArea;
            this.cloudEntity = cloudEntity;
            this.world = world;
        }
        
        public void Update(in Entity entity, ref Position position, ref Health health)
        {
            if ((position.Current - this.position.Current).sqrMagnitude > poisonCloudArea.Radius * poisonCloudArea.Radius) return;

            world.Create(new PoisonDamage(world.Reference(entity),
                cloudEntity, poisonCloudArea.Damage));
        }
    }
    
    private readonly struct ExecuteDamageByTeam : IForEachWithEntity<Team, Position, Health>
    {
        private readonly Team team;
        private readonly Position position;
        private readonly PoisonCloudArea poisonCloudArea;
        private readonly EntityReference cloudEntity;
        private readonly World world;

        public ExecuteDamageByTeam(Team team,
            Position position,
            PoisonCloudArea poisonCloudArea,
            EntityReference cloudEntity,
            World world)
        {
            this.team = team;
            this.position = position;
            this.poisonCloudArea = poisonCloudArea;
            this.cloudEntity = cloudEntity;
            this.world = world;
        }
        
        public void Update(in Entity entity, ref Team team, ref Position position, ref Health health)
        {
            if (this.team.Matches(in team)) return;
            if ((position.Current - this.position.Current).sqrMagnitude > poisonCloudArea.Radius * poisonCloudArea.Radius) return;

            world.Create(new PoisonDamage(world.Reference(entity),
                cloudEntity, poisonCloudArea.Damage));
        }
    }
}