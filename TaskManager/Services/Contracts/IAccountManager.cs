using Microsoft.AspNetCore.Identity;
using TaskManager.Models.Requests.Account.Results;

namespace TaskManager.Services.Contracts
{
    public interface IAccountManager
    {
        Task<RegistrationResult> RegisterAsync(string username, string email, string password, bool isUserAuthenticated);

        Task<LoginResult> LoginAsync(string username, string password, bool rememberMe, bool isUserAuthenticated);

        Task LogoutAsync(bool isUserAuthenticated);

        Task DeleteUserAsync(string userId);
    }
}
