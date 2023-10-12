namespace Lorux0r.RPG.Core;

public class ProfileDisplayController
{
    private readonly IProfileDisplayView view;
    private readonly IProfileGateway profileGateway;

    public ProfileDisplayController(IProfileDisplayView view,
        IProfileGateway profileGateway)
    {
        this.view = view;
        this.profileGateway = profileGateway;

        profileGateway.OnProfileUpdated += ShowProfile;
        view.ChangeNameRequested += RequestNameChange;
    }

    public void Dispose()
    {
        view.ChangeNameRequested -= RequestNameChange;
        profileGateway.OnProfileUpdated -= ShowProfile;
    }

    private void RequestNameChange(string profileId, string name)
    {
        async Task RequestNameChangeAsync(string profileId, string name, CancellationToken cancellationToken)
        {
            var profile = await profileGateway.GetProfile(profileId, cancellationToken);
            
            if (profile != null)
                await profileGateway.UpdateProfile(profile with {Name = name}, cancellationToken);
        }

        // TODO: make proper cancellation token handling
        RequestNameChangeAsync(profileId, name, default);
    }

    private void ShowProfile(Profile profile) =>
        view.Show(profile.Id, profile.Name, profile.CurrentHealth, profile.MaxHealth);
}