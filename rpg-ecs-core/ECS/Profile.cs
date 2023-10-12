namespace Lorux0r.RPG.Core.ECS;

public struct Profile
{
    public string Id { get; }
    public string Name { get; set; }

    public Profile(string id, string name)
    {
        Id = id;
        Name = name;
    }
}