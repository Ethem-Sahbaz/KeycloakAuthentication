
namespace AuthenticationAPI.Api.Application.Identity;

internal interface IIdentityProvider
{
    Task<string> RegisterUserAsync(string firstName, string lastName, string email, string password);
    Task<string> LoginUserAsync(string userName, string password);
}