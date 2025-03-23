namespace AuthenticationAPI.Api.Models;

internal sealed record RegisterRequest(string UserName, string Email, string password);
