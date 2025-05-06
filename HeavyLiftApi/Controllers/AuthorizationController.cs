using HeavyLiftApi.Data;
using HeavyLiftApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HeavyLiftApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthorizationController: Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthorizationController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("GetAuthorization")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            user User = null;
            if(string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }
            try
            {
                User = await _context.users.FirstOrDefaultAsync(u => u.email == request.Email && u.password == HashPassword(request.Password));
            }
            catch (Exception ex)
            {
                return StatusCode(500,"An error occurred while processing your request.");
            }
            if (User == null)
            {
                return Unauthorized();
            }
            var token = GenerateJwtToken(User.id);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(int userid)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
                claims: new[] { new Claim( "UserID" , userid.ToString()) },
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
