namespace Lorux0r.RPG.Core;

public class HttpProfileGateway : IProfileGateway
{
    public event Action<Profile>? OnProfileUpdated;

    public Task<Profile> GetProfile(string profileId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GET /api/profiles/:profileId");
    }

    public Task UpdateProfile(Profile profile, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("POST /api/profiles/:profileId");
    }
}