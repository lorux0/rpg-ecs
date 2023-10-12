namespace Lorux0r.RPG.Core.ECS;

public struct UpdateProfileRequest
{
    public string Id { get; }
    public string Name { get; }

    public UpdateProfileRequest(string id, string name)
    {
        Id = id;
        Name = name;
    }
}