namespace Lorux0r.RPG.Core.ECS.Itemization;

public readonly struct ItemId
{
    private readonly string value;

    public ItemId(string value) =>
        this.value = value;
    
    public bool Equals(ItemId other) => value == other.value;

    public override bool Equals(object? obj) => obj is ItemId other && Equals(other);

    public override int GetHashCode() => value.GetHashCode();

    public static implicit operator ItemId(string s) => new(s);
    public static implicit operator string(ItemId myString) => myString.value;
}