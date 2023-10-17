namespace Lorux0r.RPG.Core;

public class ProfileDisplayController
{
    private readonly IProfileDisplayView view;
    private readonly IProfileGateway profileGateway;

    private CancellationTokenSource? changeNameCancellationToken;

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
        changeNameCancellationToken?.Cancel();
        changeNameCancellationToken?.Dispose();
    }

    private async void RequestNameChange(string profileId, string name)
    {
        changeNameCancellationToken?.Cancel();
        changeNameCancellationToken?.Dispose();
        changeNameCancellationToken = new CancellationTokenSource();
        
        var profile = await profileGateway.GetProfile(profileId, changeNameCancellationToken.Token);

        if (profile != null)
            await profileGateway.UpdateProfile(profile with { Name = name }, changeNameCancellationToken.Token);
    }

    private void ShowProfile(Profile profile) =>
        view.Show(profile.Id, profile.Name, profile.CurrentHealth, profile.MaxHealth);
}