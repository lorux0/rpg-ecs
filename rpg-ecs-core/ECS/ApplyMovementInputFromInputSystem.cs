using Arch.Core;
using Arch.System;

namespace Lorux0r.RPG.Core.ECS;

public partial class ApplyMovementInputFromInputSystem : ISimpleSystem
{
    private readonly World world;
    private readonly ICharacterInputProvider characterInputProvider;

    public ApplyMovementInputFromInputSystem(World world,
        ICharacterInputProvider characterInputProvider)
    {
        this.world = world;
        this.characterInputProvider = characterInputProvider;
    }
    
    public void Initialize()
    {
    }

    public void Dispose()
    {
    }

    public void Update()
    {
        ExecuteQuery(world);
    }

    [Query]
    [All(typeof(CharacterAxisInput))]
    private void Execute(ref CharacterAxisInput characterAxisInput)
    {
        characterAxisInput.Axis = characterInputProvider.GetMovementAxis();
    }
}