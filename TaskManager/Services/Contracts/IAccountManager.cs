using Microsoft.AspNetCore.Identity;

namespace TaskManager.Services.Contracts
{
    public interface IAccountManager
    {
        Task<IdentityResult> RegisterAsync(string username, string email, string password);

        Task<SignInResult> LoginAsync(string username, string password, bool rememberMe);

        Task LogoutAsync();
    }
}
