namespace Lorux0r.RPG.Core;

public interface IProfileGateway
{
    public event Action<Profile> OnProfileUpdated;

    Task<Profile?> GetProfile(string profileId, CancellationToken cancellationToken);
    Task UpdateProfile(Profile profile, CancellationToken cancellationToken);
}