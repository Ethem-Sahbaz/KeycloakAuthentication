namespace AuthenticationAPI.Api.Application.Identity.Keycloak;

internal sealed record UserRepresentation(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool EmailVerified,
    bool Enabled,
    CredentialRepresentation[] Credentials);

internal sealed record CredentialRepresentation(string Type, string Value, bool Temporary);
