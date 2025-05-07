using HeavyLiftApi.Data;
using HeavyLiftApi.DTO;
using HeavyLiftApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HeavyLiftApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        [Authorize]
        public async Task<IActionResult> LogIn()
        {
            int userId = -1;
            bool succes = int.TryParse(User.FindFirst("UserID")?.Value, out userId);
            if (succes)
            {
                return await GetUserProfile(userId);
            }
            return BadRequest(new { message = "Error occured while reading userID" });
        }

        [HttpGet("GetProfile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            if (userId <= 0) {
                return BadRequest(new { message = "Invalid userId." });
            }
            user UserData = null;

            try
            {
                UserData = await _context.users.FirstOrDefaultAsync(u => u.id == userId);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            if( UserData == null)
            {
                return NotFound(new { message = "This user dont exist" });
            }

            UserProfileData UserProfile = new UserProfileData(UserData);

            return Ok(UserProfile);
        }

        [HttpPost("CreateAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterAccountDTO registerAccount)
        {
            if( string.IsNullOrEmpty(registerAccount.Email) || string.IsNullOrEmpty(registerAccount.Nickname) || string.IsNullOrEmpty(registerAccount.Password))
            {
                return BadRequest("Email, password, nickname are empty.");
            }
            if (!IsValidEmail(registerAccount.Email))
            {
                return BadRequest("Email is invalid");
            }
            try
            {
                //Check if user with this email already exist
                var accoutn = await _context.users.FirstOrDefaultAsync(u => u.email == registerAccount.Email);
                if(accoutn != null)
                {
                    return Conflict("User with this email exist.");
                }
            }
            catch {
                return StatusCode(500, "An error occurred while processing your request.");
            }
            if (!IsValidPassword(registerAccount.Password))
            {
                return BadRequest("Password must have number and apitalized letter");
            }

            //Making user rekord
            user User = new user
            {
                email = registerAccount.Email,
                nickname = registerAccount.Nickname,
                password = HashPassword(registerAccount.Password),
                status = '0',
                createdat = DateTime.Today,
                profilepicture = null
            };
            try
            {
                await _context.users.AddAsync(User);
                _context.SaveChanges();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
            return Ok("Account has benn created");
        }


        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
            {
                return false;
            }

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasDigit = password.Any(char.IsDigit);

            return hasUpperCase && hasDigit;
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
