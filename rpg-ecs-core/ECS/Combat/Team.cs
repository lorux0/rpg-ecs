namespace Lorux0r.RPG.Core.ECS.Combat;

public struct Team
{
    public int Mask { get; }
    
    public Team(int mask)
    {
        Mask = mask;
    }

    public readonly bool Matches(in Team team) =>
        (Mask & team.Mask) != 0;
}