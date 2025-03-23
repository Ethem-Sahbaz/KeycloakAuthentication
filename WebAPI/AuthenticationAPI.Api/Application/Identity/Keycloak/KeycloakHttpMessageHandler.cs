
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading;

namespace AuthenticationAPI.Api.Application.Identity.Keycloak;

internal sealed class KeycloakHttpMessageHandler : DelegatingHandler
{
    private readonly KeycloakOptions _options;

    public KeycloakHttpMessageHandler(IOptions<KeycloakOptions> options)
    {
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get own access token for confidential client with client id and client secret.

        string accessToken = await GetConfidentialClientAccessTokenAsync(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }

    private async Task<string> GetConfidentialClientAccessTokenAsync(CancellationToken cancellationToken)
    {
        var formData = new KeyValuePair<string, string>[]
{
            new("client_id", _options.ConfidentialClientId),
            new("client_secret", _options.ConfidentialClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials")
};

        using var formDataContent = new FormUrlEncodedContent(formData);

        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _options.TokenUrl);

        httpRequestMessage.Content = formDataContent;

        using HttpResponseMessage response = await base.SendAsync(httpRequestMessage, cancellationToken);

        string accessToken = string.Empty;

        if (response.IsSuccessStatusCode)
        {
            var accessTokenModel = await response.Content.ReadFromJsonAsync<AccessTokenModel>(cancellationToken);

            if (accessTokenModel != null && !string.IsNullOrEmpty(accessTokenModel.AccessToken))
            {
                accessToken = accessTokenModel.AccessToken;
            }
        }

        return accessToken;
    }
}


internal sealed class AccessTokenModel
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }
}
