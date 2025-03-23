using AuthenticationAPI.Api.Application.Identity.Keycloak;

namespace AuthenticationAPI.Api.Application.Identity;

internal sealed class IdentityProvider : IIdentityProvider
{
    private readonly KeycloakService _keycloakService;

    public IdentityProvider(KeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task<string> RegisterUserAsync(string firstName, string lastName, string email, string password)
    {
        UserRepresentation userRepresentation = new(
            email,
            email,
            firstName,
            lastName,
            true,
            true,
            [new CredentialRepresentation("password", password, false)]);

        var identityId = await _keycloakService.RegisterUserAsync(userRepresentation);

        return identityId;
    }
}
