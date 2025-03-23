using AuthenticationAPI.Api.Application.Identity.Keycloak;
using Microsoft.Extensions.Options;
using System.Threading;

namespace AuthenticationAPI.Api.Application.Identity;

internal sealed class IdentityProvider : IIdentityProvider
{
    private readonly KeycloakService _keycloakService;
    private readonly IHttpClientFactory _clientFactory;
    private readonly KeycloakOptions _options;

    public IdentityProvider(
        KeycloakService keycloakService,
        IHttpClientFactory clientFactory,
        IOptions<KeycloakOptions> options)
    {
        _keycloakService = keycloakService;
        _clientFactory = clientFactory;
        _options = options.Value;
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

    /// <summary>
    /// Demo
    /// </summary>
    public async Task<string> LoginUserAsync(string userName, string password)
    {
        var httpClient = _clientFactory.CreateClient();

        var formData = new KeyValuePair<string, string>[]
        {
            new("client_id", "public-client"),
            new("username", userName),
            new("password", password),
            new("scope", "email openid"),
            new("grant_type", "password")
        };

        using var formDataContent = new FormUrlEncodedContent(formData);

        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _options.TokenUrl);

        httpRequestMessage.Content = formDataContent;

        using HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

        var accessTokenModel = await response.Content.ReadFromJsonAsync<AccessTokenModel>();

        return accessTokenModel.AccessToken;

    }
}
