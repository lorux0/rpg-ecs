namespace Lorux0r.RPG.Core.ECS;

public struct ProfileChangeRequest
{
    public string Id { get; }
    public string Name { get; }

    public ProfileChangeRequest(string id, string name)
    {
        Id = id;
        Name = name;
    }
}