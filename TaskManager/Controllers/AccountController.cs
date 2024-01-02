using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskManager.Services.Contracts;
using NuGet.Protocol.Plugins;
using TaskManager.Models.Requests;

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

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _accountManager.RegisterAsync(request.Username, request.Email, request.Password);

            if (result.Succeeded)
            {
                return Ok("Registration successful");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _accountManager.LoginAsync(request.Username, request.Password, request.RememberMe);

            if (result.Succeeded)
            {
                return Ok("Login successful");
            }

            return BadRequest("Invalid login attempt");
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountManager.LogoutAsync();
            return Ok("Logout successful");
        }
    }
}
