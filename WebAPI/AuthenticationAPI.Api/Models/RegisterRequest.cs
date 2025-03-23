namespace AuthenticationAPI.Api.Models;

internal sealed record RegisterRequest(string FirstName, string LastName, string Email, string Password);