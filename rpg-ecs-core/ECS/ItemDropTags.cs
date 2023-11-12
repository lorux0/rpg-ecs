namespace Lorux0r.RPG.Core.ECS;

public struct ItemDropTags
{
    public string[] Tags { get; }
    public int Iterations { get; }

    public ItemDropTags(string[] tags, int iterations)
    {
        Tags = tags;
        Iterations = iterations;
    }
}