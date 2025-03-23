namespace AuthenticationAPI.Api.Application.Identity.Keycloak;

internal sealed class KeycloakService
{
    private readonly HttpClient _httpClient;

    public KeycloakService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<string> RegisterUserAsync(UserRepresentation userRepresentation)
    {
        var response = await _httpClient.PostAsJsonAsync("users", userRepresentation);

        response.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(response);
    }

    private string ExtractIdentityIdFromLocationHeader(
    HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
}
