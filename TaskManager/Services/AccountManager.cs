using Microsoft.AspNetCore.Identity;
using TaskManager.Data;
using TaskManager.Models.Requests.Account.Results;
using TaskManager.Services.Contracts;

namespace TaskManager.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountManager(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<RegistrationResult> RegisterAsync(string username, string email, string password, bool isUserAuthenticated)
        {
            try
            {
                // Check if the user is already authenticated
                if (isUserAuthenticated)
                {
                    return new RegistrationResult { Succeeded = false, Errors = new[] { "User is already logged in" } };
                }

                // Creating new user with the model properties
                var user = new IdentityUser { UserName = username, Email = email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // If registration is successful, return a custom result with the Id property of the user
                    return new RegistrationResult { Succeeded = true, UserId = user.Id, Email = user.Email, Username = user.UserName };
                }

                return new RegistrationResult { Succeeded = false, Errors = result.Errors.Select(error => error.Description).ToArray() };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<LoginResult> LoginAsync(string username, string password, bool rememberMe, bool isUserAuthenticated)
        {
            try
            {
                // Check if the user is already authenticated
                if (isUserAuthenticated)
                {
                    return new LoginResult { Succeeded = false, Errors = new[] { "User is already logged in" } };
                }

                var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(username);
                    if (user != null)
                    {
                        // Return a custom result with the Id property if login is successful
                        return new LoginResult { Succeeded = true, UserId = user.Id, Email = user.Email, Username = user.UserName };
                    }
                }

                return new LoginResult { Succeeded = false, Errors = new[] { "Invalid login attempt" } };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                // Logout of user
                await _signInManager.SignOutAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            try
            {
                // Searching for user in the DB
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new ArgumentException("Account not found");
                }

                // Deleting user
                await _userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

}
