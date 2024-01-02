using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services.Contracts;
using System.Threading.Tasks;
using TaskManager.Models.Requests.Account;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        private bool IsUserAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _accountManager.RegisterAsync(request.Username, request.Email, request.Password, IsUserAuthenticated());

            if (result.Succeeded)
            {
                // If registration is successful, return the Id property
                return Ok(new { Message = "Registration successful", UserId = result.UserId, Email = result.Email, Username = result.Username });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _accountManager.LoginAsync(request.Username, request.Password, request.RememberMe, IsUserAuthenticated());

                if (!result.Succeeded)
                {
                    throw new ArgumentException();
                }
                    // If login is successful, return the Id property
                return Ok(new { Message = "Login Successfull", UserId = result.UserId, Email = result.Email, Username = result.Username });
            }
            catch (Exception)
            {
                return BadRequest("Invalid login attempt");
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountManager.LogoutAsync();
                return Ok("Logout successful");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser([FromBody] UserIdRequest request)
        {
            try
            {
                await _accountManager.DeleteUserAsync(request.UserId);
                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception)
            {
                return BadRequest("User not found");
            }
        }
    }
}
