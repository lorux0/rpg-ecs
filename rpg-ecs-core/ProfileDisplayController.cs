using Lorux0r.RPG.Core.ECS;

namespace Lorux0r.RPG.Core;

public class ProfileDisplayController
{
    private readonly IProfileDisplayView view;
    private readonly IProfileBridge profileBridge;

    public ProfileDisplayController(IProfileDisplayView view,
        IProfileBridge profileBridge)
    {
        this.view = view;
        this.profileBridge = profileBridge;
        
        profileBridge.OnProfileUpdated += ShowProfile; 
        view.ChangeNameRequested += RequestNameChange;
    }

    public void Dispose()
    {
        view.ChangeNameRequested -= RequestNameChange;
        profileBridge.OnProfileUpdated -= ShowProfile;
    }

    private void RequestNameChange(string profileId, string name)
    {
        // world.Create(new ProfileChangeRequest(profileId, name));
    }

    private void ShowProfile(Profile profile)
    {
        view.Show(profile.Id, profile.Name, profile.CurrentHealth, profile.MaxHealth);
    }
}