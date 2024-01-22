using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Models.Requests.Account;
using TaskManager.Services.Contracts;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        //Dependency injection
        private readonly IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        // Private helper function for authentication
        private bool IsUserAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        private string GenerateToken(string userId, string username)
        {
            string yourSecretKey = "3X4mp13_Str0ng_S3cr3t_K3y_!@#$%^&*";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(yourSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
    };

            var token = new JwtSecurityToken(
                issuer: "Task_Api",
                audience: "Xamarin_Mobile_App",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(20),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
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

                if (result.Succeeded)
                {
                    // Generate and return a token
                    var token = GenerateToken(result.UserId, result.Username);

                    return Ok(new { Message = "Login Successful", Token = token, UserId = result.UserId, Email = result.Email, Username = result.Username });
                }

                throw new ArgumentException();
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
                await _accountManager.LogoutAsync(IsUserAuthenticated());
                return Ok("Logout successful");
            }
            catch (Exception)
            {
                return BadRequest("There isn't a logged in user");
            }

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                await _accountManager.DeleteUserAsync(OwnerId);
                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception)
            {
                return BadRequest("User not found");
            }
        }
    }
}
