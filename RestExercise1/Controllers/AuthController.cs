using AktieHandelRepositoryLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestExercise1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IAuthenticator _authenticator;

        public AuthController(IConfiguration config, IAuthenticator authenticator)
        {
            _config = config;
            _authenticator = authenticator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            // 1. Validate the user (In a real scenario, check your database here)
            Role? loginRole = _authenticator.Authenticate(login.Username, login.Password);
            if (loginRole.HasValue)
            {
                var token = GenerateJwtToken(login.Username,(Role)loginRole);
                return Ok(new { token });
            }

            return Unauthorized("Invalid username or password.");
        }

        private string GenerateJwtToken(string username, Role loginRole)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims are the pieces of information "baked" into the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, loginRole.ToString()) // You can add roles here
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Token is valid for 2 hours
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Helper class to receive JSON data
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
