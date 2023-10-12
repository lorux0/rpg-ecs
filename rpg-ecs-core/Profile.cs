namespace Lorux0r.RPG.Core;

public class Profile
{
    public string Id { get; }
    public string Name { get; }
    public float CurrentHealth { get; }
    public float MaxHealth { get; }
    
    public Profile(string id, string name, float currentHealth, float maxHealth)
    {
        Id = id;
        Name = name;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}