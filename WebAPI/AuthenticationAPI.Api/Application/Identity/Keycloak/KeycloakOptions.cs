namespace AuthenticationAPI.Api.Application.Identity.Keycloak;

// Options always as class. DI needs parameterless constructor
internal sealed class KeycloakOptions
{
    public string AdminUrl { get; set; }

    public string TokenUrl { get; set; }

    public string ConfidentialClientId { get; set; }

    public string ConfidentialClientSecret { get; set; }

    public string PublicClientId { get; set; }
}
